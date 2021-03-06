﻿using System;
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
        public int Length
        {
            get { return (int)MathHelper.GetDistanceBetweenPoints(start, end); }
        }

        public Point LeftPoint
        {
            get
            {
                if (start.X < end.X)
                    return start;
                else
                    return end;
            }
        }
        public Point RightPoint
        {
            get
            {
                if (start.X > end.X)
                    return start;
                else
                    return end;
            }
        }
        public Point TopPoint
        {
            get
            {
                if (start.Y < end.Y)
                    return start;
                else
                    return end;
            }
        }
        public Point BottomPoint
        {
            get
            {
                if (start.Y > end.Y)
                    return start;
                else
                    return end;
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

            Point returnPoint = new Point((int)x, (int)y);

            //dont return a point out of bounds
            if(!line.isPointWithinSegment(point) && false)
            {
                //TODO: Correct pointOnLine function to not return pounts out of segment range
                if (point.X > line.RightPoint.X)
                    returnPoint.X = line.RightPoint.X;
                else if (point.X < line.LeftPoint.X)
                    returnPoint.X = line.LeftPoint.X;

                if (point.Y > line.BottomPoint.Y)
                    returnPoint.Y = line.BottomPoint.Y;
                else if (point.Y < line.TopPoint.Y)
                    returnPoint.Y = line.TopPoint.Y;
            }

            return returnPoint;
        }

        public Point nearestPoint(Point p)
        {
            return Line.nearestPointOnLine(this, p);
        }

        public bool isPointWithinSegment(Point p)
        {
            bool xTest = false;
            bool yTest = false;

            if (start.X <= end.X && start.X <= p.X && p.X <= end.X)
            {
                xTest = true;
            }
            if (start.X >= end.X && start.X >= p.X && p.X >= end.X)
            {
                xTest = true;
            }
            if (start.Y <= end.Y && start.Y <= p.Y && p.Y <= end.Y)
            {
                yTest = true;
            }
            if (start.Y >= end.Y && start.Y >= p.Y && p.Y >= end.Y)
            {
                yTest = true;
            }

            if (xTest && yTest)
                return true;
            else
                return false;
        }

        public bool isIdentical(Line l)
        {
            if (
                l.Start.X == start.X
                && l.Start.Y == start.Y
                && l.End.X == end.X
                && l.End.Y == end.Y)
            {
                return true;
            }else
            {
                return false;
            }
        }
    }
}
