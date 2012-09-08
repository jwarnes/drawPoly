using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrawPoly
{
    class CNode : Node
    {
        private List<Polygon> polygons = new List<Polygon>();

        public bool Selected{ get; set; }

        public List<Polygon> Polygons
        {
            get
            { return polygons; }
        }

        public CNode()
        {
        }
        public CNode(Point location)
        {
            this.location = location;
        }
        public CNode(int x, int y)
        {
            location.X = x;
            location.Y = y;
        }

        public bool isAssociated(Polygon poly)
        {
            return polygons.Contains(poly);
        }
        public void Associate(Polygon poly)
        {
            polygons.Add(poly);
        }
        public void Remove(Polygon poly)
        {
            polygons.Remove(poly);
        }

        public void Draw(Graphics g)
        {
            Point p = Location;
            p.Offset(-11, -11);
            Rectangle rect = new Rectangle(p, new Size(21,21));
            Color c = !Selected ? Color.Purple : Color.LimeGreen;

            g.FillEllipse(new SolidBrush(Color.FromArgb(100, c)), rect);
            g.DrawEllipse(new Pen(c, 3), rect); 

            //also draw conneections
            foreach (Node node in Connections)
            {
                g.DrawLine(new Pen(Color.Purple, 1), this.location, node.Location);
            }
        }
    }
}
