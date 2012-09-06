using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace DrawPoly
{
    class Line
    {
        #region Fields, Props, Constructor

        private Point start = new Point();
        private Point end = new Point();

        public Point Start
        {
            get { return start; }
            set { start = value; }
        }
        public Point End
        {
            get { return end; }
            set { end = value; }
        }
        public Point[] Points
        {
            get
            {
                return new Point[] { start, end };
            }
        }
        public Point Midpoint
        {
            get
            {
                return new Point((start.X + end.X) / 2, (start.Y + end.Y) / 2);
            }
        }
        public Line()
        {
        }

        public Line(Point start, Point end)
        {
            this.start = start;
            this.end = end;
        }
        #endregion

        public static Point nearestPointOnLine(Line line, Point point)
        {
            //make some variable shortcuts here
            Point A = line.Start;
            Point B = line.End;
            Point P = point;

            Point AtoP = new Point(P.X - A.X, P.Y - A.Y);
            Point AtoB = new Point(B.X - A.X, B.Y - A.Y);

            float AtoB_Squared = (AtoB.X * AtoB.X) + (AtoB.Y * AtoB.Y);

            float atp_dot_atb = (AtoP.X * AtoB.X) + (AtoP.Y * AtoB.Y);

            float t = atp_dot_atb / AtoB_Squared;

            float x = A.X + AtoB.X * t;
            float y = A.Y + AtoB.Y * t;

            return new Point((int)x, (int)y);
        }

        public Point nearestPoint(Point p)
        {
            return Line.nearestPointOnLine(this, p);
        }
    }
}
