using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace DrawPoly
{
    class Polygon
    {
        #region Fields
        private List<Point> vertices = new List<Point>();
        public List<Point> Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }

        public Point[] VerticesArray
        {
            get
            {
                Point[] a1 = new Point[vertices.Count];
                a1 = vertices.ToArray();
                return a1;
            }
            set
            {
                Point[] a1 = value;
                vertices = a1.ToList<Point>();
            }
        }
        public PointF[] VerticesArrayF
        {
            get
            {
                PointF[] a1 = new PointF[vertices.Count];
                for (int i = 0; i < vertices.Count; i++)
                {
                    a1[i] = new PointF(vertices[i].X, vertices[i].Y);
                }
                return a1;
            }
        }
        public double Area
        {
            get
            {
                return computeArea();
            }
        }
        public Point Centroid
        {
            get
            {
                return computeCentroid();
            }
        }

        public Polygon()
        {
        }

        public Polygon(List<Point> vertices)
        {
            this.vertices = vertices.ToList<Point>();
        }
        #endregion

        public void Draw(Graphics g)
        {
            if (vertices.Count < 3)
                throw new Exception("Polygon must have at least 3 verticies to draw");

            Color fill = IsConcave() ? Color.FromArgb(100, Color.Red) : Color.FromArgb(100, Color.MediumBlue);
            Pen p = IsConcave() ? new Pen(Color.Red, 2) : new Pen(Color.Turquoise, 2);
            g.DrawPolygon(p, this.VerticesArray);
            g.FillPolygon(new SolidBrush(fill), this.VerticesArray);
        }


        #region Calculations

        public bool IsConcave()
        {
            int positive = 0;
            int negative = 0;
            int length = vertices.Count;

            for (int i = 0; i < length; i++)
            {
                Point p0 = vertices[i];
                Point p1 = vertices[(i + 1) % length];
                Point p2 = vertices[(i + 2) % length];

                // Subtract to get vectors
                Point v0 = new Point(p0.X - p1.X, p0.Y - p1.Y);
                Point v1 = new Point(p1.X - p2.X, p1.Y - p2.Y);
                float cross = (v0.X * v1.Y) - (v0.Y * v1.X);

                if (cross < 0)
                {
                    negative++;
                }
                else
                {
                    positive++;
                }
            }

            return (negative != 0 && positive != 0);
        }

        public bool Intersects(Point p)
        {
            bool intersects = false;
            float x = p.X;
            float y = p.Y;

            for (int i = 0, j = vertices.Count - 1; i < vertices.Count; j = i++)
            {
                if ((((vertices[i].Y <= y) && (y < vertices[j].Y)) ||
                        ((vertices[j].Y <= y) && (y < vertices[i].Y))) &&
                    (x < (vertices[j].X - vertices[i].X) * (y - vertices[i].Y) / (vertices[j].Y - vertices[i].Y) + vertices[i].X))

                    intersects = !intersects;
            }
            return intersects;
        }

        private Point computeCentroid()
        {
            PointF centroid = new PointF(0,0);
            float signedArea = 0.0f;
            float x0 = 0.0f; // Current vertex X
            float y0 = 0.0f; // Current vertex Y
            float x1 = 0.0f; // Next vertex X
            float y1 = 0.0f; // Next vertex Y
            float a = 0.0f;  // Partial signed area

            // For all vertices except last
            int i = 0;
            for (i = 0; i < vertices.Count - 1; ++i)
            {
                x0 = vertices[i].X;
                y0 = vertices[i].Y;
                x1 = vertices[i + 1].X;
                y1 = vertices[i + 1].Y;
                a = x0 * y1 - x1 * y0;
                signedArea += a;
                centroid.X += (x0 + x1) * a;
                centroid.Y += (y0 + y1) * a;
            }

            // Do last vertex
            x0 = vertices[i].X;
            y0 = vertices[i].Y;
            x1 = vertices[0].X;
            y1 = vertices[0].Y;
            a = x0 * y1 - x1 * y0;
            signedArea += a;
            centroid.X += (x0 + x1) * a;
            centroid.Y += (y0 + y1) * a;

            signedArea *= 0.5f;
            centroid.X /= (6 * signedArea);
            centroid.Y /= (6 * signedArea);

            return new Point((int)centroid.X, (int)centroid.Y);
        }
    
        private double computeArea()
        {
            Point[] polygon = this.VerticesArray;
            int i, j;
            double area = 0;

            for (i = 0; i < polygon.Length; i++)
            {
                j = (i + 1) % polygon.Length;

                area += polygon[i].X * polygon[j].Y;
                area -= polygon[i].Y * polygon[j].X;
            }

            area /= 2;
            return (area < 0 ? -area : area);
        }

        public static Point findLinkPoint(Polygon poly1, Polygon poly2)
        {
            return new Point();
            //TODO
        }

        public Line getClosestEdge(Point p)
        {
            List<Line> lines = getLines();
            Line closestLine = new Line();
            double minDistance = 10000000;

            //iterate through all the lines, and compare distances
            foreach (Line l in lines)
            {
                Point pointOnLine = Line.nearestPointOnLine(l, p);
                double distance = MathHelper.GetDistanceBetweenPoints(p, pointOnLine);

                //found a closer line...
                if (distance < minDistance)
                {
                    closestLine = l;
                    minDistance = distance;
                }
            }

            return closestLine;
        }

        public List<Line> getLines()
        {
            List<Line> lines = new List<Line>();

            for (int i = 0; i < vertices.Count; i++)
            {
                if (i == vertices.Count - 1)
                    lines.Add(new Line(vertices[i], vertices[0]));
                else
                    lines.Add(new Line(vertices[i], vertices[i + 1]));
            }

            return lines;
        }
        
        #endregion

    }
}
