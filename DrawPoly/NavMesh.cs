using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Drawing.Design;

namespace DrawPoly
{
    class NavMesh
    {
        #region Fields
        protected List<CNode> nodes = new List<CNode>();
        protected List<Polygon> polygons = new List<Polygon>();
        protected List<Link> links = new List<Link>();

        public List<Polygon> Polygons
        {
            get { return polygons; }
            set { polygons = value; }
        }
        public List<Link> Links
        {
            get { return links; }
            set { links = value; }
        }

        public NavMesh()
        {
        }

        public NavMesh(List<Polygon> polygons, List<Link> links)
        {
            this.links = links;
            this.polygons = polygons;
        }
        #endregion

        #region Methods
        private void CreateNodeMap()
        {

            //centroids
            foreach (Polygon poly in polygons)
            {
                CNode n = new CNode(poly.Centroid);
                n.Associate(poly);

                nodes.Add(n);
            }

            //midpoints
            foreach (Link link in links)
            {
                CNode n = new CNode(link.ConnectingLine().Midpoint);
                n.Associate(link.StartPoly);
                n.Associate(link.EndPoly);

                nodes.Add(n);
            }

            //connect nodes
            foreach (CNode n in nodes)
            {
                foreach (CNode n2 in nodes)
                {
                    if (n == n2)
                        continue;
                    foreach (Polygon poly in n.Polygons)
                    {
                        if (n2.isAssociated(poly))
                            n.Add(n2);
                    }
                }
            }
        }
        public void GenerateNodeMap()
        {
            nodes.Clear();
            CreateNodeMap();
        }
        public void GenerateNodeMap(Point start, Point end)
        {
            nodes.Clear();

            CNode startNode = new CNode(start);
            CNode endNode = new CNode(end);

            foreach (Polygon poly in polygons)
            {
                if (poly.Intersects(startNode.Location))
                    startNode.Associate(poly);
                if (poly.Intersects(endNode.Location))
                    endNode.Associate(poly);
            }

            nodes.Add(startNode);
            nodes.Add(endNode);

            CreateNodeMap();
        }
        public Polygon getPolyByID(int id)
        {
            Polygon found = null;
            foreach (Polygon poly in polygons)
            {
                if (poly.ID == id) 
                found = poly;
            }

            return found;
        }
        public Polygon getPolyByID(string id)
        {
            Polygon found = null;
            foreach (Polygon poly in polygons)
            {
                if (poly.ID == Convert.ToInt16(id))
                    found = poly;
            }

            return found;
        }


        public void Draw(Graphics g)
                {
                    if (nodes.Count > 0)
                    {
                        foreach (CNode node in nodes)
                        {
                            node.Draw(g);
                        }
                    }
                }

        public void writeToFile(string path)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("   ");

            XmlWriter w = XmlWriter.Create(path, settings);
            w.WriteStartDocument();
            w.WriteStartElement("NavMesh");

            //polygons
            w.WriteStartElement("Polygons");
            foreach (Polygon poly in polygons)
            {
                w.WriteStartElement("Polygon");
                w.WriteAttributeString("ID", poly.ID.ToString());
                foreach (Point v in poly.Vertices)
                {
                    w.WriteStartElement("Vertex");
                    {
                        w.WriteAttributeString("X", v.X.ToString());
                        w.WriteAttributeString("Y", v.Y.ToString());
                    }
                    w.WriteEndElement();
                }
                w.WriteEndElement();
            }
            w.WriteEndElement();

            w.WriteStartElement("Links");
            //links
            foreach (Link link in links)
            {
                w.WriteStartElement("Link");

                w.WriteAttributeString("startID", link.StartPoly.ID.ToString());
                w.WriteAttributeString("endID", link.EndPoly.ID.ToString());

                //start line

                w.WriteStartElement("Line");
                {
                    w.WriteAttributeString("Number", "1");
                    w.WriteAttributeString("X1", link.Line1.Start.X.ToString());
                    w.WriteAttributeString("Y1", link.Line1.Start.Y.ToString());
                    w.WriteAttributeString("X2", link.Line1.End.X.ToString());
                    w.WriteAttributeString("Y2", link.Line1.End.Y.ToString());
                }
                w.WriteEndElement();

                w.WriteStartElement("Line");
                {
                    w.WriteAttributeString("Number", "2");
                    w.WriteAttributeString("X1", link.Line2.Start.X.ToString());
                    w.WriteAttributeString("Y1", link.Line2.Start.Y.ToString());
                    w.WriteAttributeString("X2", link.Line2.End.X.ToString());
                    w.WriteAttributeString("Y2", link.Line2.End.Y.ToString());
                }
                w.WriteEndElement();

                //end link
                w.WriteEndElement();
            }
            w.WriteEndElement();

            w.WriteEndElement();
            w.Close();
        }
        public void readFromFile(string path)
        {
            polygons.Clear();
            links.Clear();
            nodes.Clear();

            XmlReaderSettings s = new XmlReaderSettings();
            s.IgnoreComments = true;
            s.IgnoreWhitespace = true;

            XmlReader r = XmlReader.Create(path, s);
            //TODO add file validation

            #region Polygons
            r.ReadToDescendant("Polygon");
            do
            {
                Polygon poly = new Polygon();
                poly.ID = Convert.ToInt16(r["ID"]);

                //vertices
                r.ReadToDescendant("Vertex");
                do
                {
                    Point p = new Point();
                    p.X = Convert.ToInt16(r["X"]);
                    p.Y = Convert.ToInt16(r["Y"]);
                    poly.Vertices.Add(p);
                }
                while(r.ReadToNextSibling("Vertex"));

                polygons.Add(poly);
            }
            while (r.ReadToNextSibling("Polygon"));
            #endregion

            #region Links
            r.ReadToFollowing("Links");
            r.ReadToDescendant("Link");
            do
            {
                Link link = new Link();
                link.StartPoly = getPolyByID(r["startID"]);
                link.EndPoly = getPolyByID(r["endID"]);

                //parse lines
                r.ReadToDescendant("Line");
                do
                {
                    int n = Convert.ToInt16(r["Number"]);
                    Point start = new Point();
                    Point end = new Point();

                    start.X = Convert.ToInt16(r["X1"]);
                    start.Y = Convert.ToInt16(r["Y1"]);

                    end.X = Convert.ToInt16(r["X2"]);
                    end.Y = Convert.ToInt16(r["Y2"]);

                    if(n==1)
                        link.Line1 = new Line(start, end);
                    else if(n==2)
                        link.Line2 = new Line(start, end);
                }
                while (r.ReadToNextSibling("Line"));
                
                //add link
                links.Add(link);
            }
            while (r.ReadToNextSibling("Link"));


            #endregion
            r.Close();
        }
        #endregion

    }
}
