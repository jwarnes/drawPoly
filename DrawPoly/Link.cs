using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrawPoly
{
    class Link
    {
        #region Fields, props, constructors

        private Line line1;
        private Line line2;
        private Polygon[] polys = new Polygon[2];

        public Polygon StartPoly
        {
            get { return polys[0]; }
            set { polys[0] = value;}
        }
        public Polygon EndPoly
        {
            get { return polys[1]; }
            set { polys[1] = value; }
        }
        public Line Line1
        {
            get { return line1; }
            set { line1 = value; }
        }

        public Line Line2
        {
            get { return line2; }
            set { line2 = value; }
        }

        public Line shortLine
        {
            get
            {
                if (line1.Length > line2.Length)
                    return line2;
                else
                    return line1;
            }
        }
        public Line longLine
        {
            get
            {
                if (line1.Length > line2.Length)
                    return line1;
                else
                    return line2;
            }
        }

        public Link()
        {
        }

        public Link(Line line1, Line line2, Polygon startP, Polygon endP)
        {
            this.line1 = line1;
            this.line2 = line2;
            polys[0] = startP;
            polys[1] = endP;
        }
        #endregion

        public Line ConnectingLine()
        {
            Point start = this.shortLine.Midpoint;
            Point end = this.longLine.nearestPoint(start);
            if (!this.longLine.isPointWithinSegment(end))
            {
                end = this.longLine.Midpoint;
            }

            return new Line(start, end);
        }

        public void DrawLink(Graphics g)
        {
            Line connectLine = ConnectingLine();
            g.DrawLine(new Pen(Color.LimeGreen, 2), connectLine.Start, connectLine.End);
        }
    }
}
