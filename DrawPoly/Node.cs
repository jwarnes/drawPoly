using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrawPoly
{
    class Node
    {
        #region Fields, Props, and Constructors
        private List<Node> connections = new List<Node>();
        private bool obstacle = false;
        protected Point location = new Point();

        public bool Obstacle
        {
            get { return obstacle; }
            set { obstacle = value; }
        }

        public List<Node> Connections
        {
            get
            {
                return connections;
            }
        }
        public Point Location
        {
            get { return location; }
            set { location = value; }
        }

        public Node()
        {
        }
        public Node(Point location)
        {
            this.location = location;
        }
        public Node(int x, int y)
        {
            location.X = x;
            location.Y = y;
        }
        #endregion

        public void Add(Node node)
        {
            connections.Add(node);
        }

        protected void Connect(ref Node node)
        {
            connections.Add(node);
            node.Add(this);
        }

        public void Disconnect(ref Node node)
        {
            Unlink(node);
            node.Unlink(this);
        }

        public void Unlink(Node node)
        {
            connections.Remove(node);
        }
    }
}
