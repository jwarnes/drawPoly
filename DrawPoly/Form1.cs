using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace DrawPoly
{
    //clear up ambiguity between rectangles
    using Rectangle = System.Drawing.Rectangle;
    
    public partial class Form1 : Form
    {
        #region Fields
        private List<Point> points = new List<Point>();
        Graphics g;
        private int dd = 0;
        private Point currentDrawPoint;

        //private Polygon testPoly = new Polygon();

        public enum Mode
        {
            Null, DrawNew
        }

        Mode mode = Mode.Null;
        public Form1()
        {
            InitializeComponent();
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            box.Image = new Bitmap(panel1.Width, panel1.Height);
            g = Graphics.FromImage(box.Image);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            g.Clear(Color.CornflowerBlue);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            box.Image = new Bitmap(panel1.Width, panel1.Height);
            g = Graphics.FromImage(box.Image);
            dbg.Text = panel1.Width.ToString();
        }

        private void drawNewPolyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = Mode.DrawNew;
        }

        private void box_MouseMove(object sender, MouseEventArgs e)
        {
            dbg.Text = points.Count.ToString();
            if (mode == Mode.DrawNew)
            {
                //update the current point 
                currentDrawPoint = e.Location;
                //lock on to the first point if the mouse is close enough and we have enough
                //points to make a closed polygon
                if (points.Count > 2 &&
                    MathHelper.GetDistanceBetweenPoints(e.Location, points.First()) <= 20)
                {
                    currentDrawPoint = points.First();
                }
            }
            this.Render();
        }

        private void box_MouseClick(object sender, MouseEventArgs e)
        {
            if (mode == Mode.DrawNew)
            {
                //create a complete polygon
                if (points.Count > 2 &&
                    MathHelper.GetDistanceBetweenPoints(e.Location, points.First()) <= 20)
                {
                    //testPoly.Points = points;
                    mode = Mode.Null;
                }
                else
                    points.Add(currentDrawPoint);
            }

        }

        private void box_Paint(object sender, PaintEventArgs e)
        {
           
        }
        private void Render()
        {
            g.Clear(Color.CornflowerBlue);

            //draw current line
            if (points.Count > 0 && mode == Mode.DrawNew)
            {
                g.DrawLine(new Pen(Color.Red), points.Last<Point>(), currentDrawPoint);
                GraphicsPath path = new GraphicsPath();

                //if more than one point, link them all together
                if (points.Count > 1)
                {
                    g.DrawLines(new Pen(Color.Red), points.ToArray<Point>());
                }
            }
            foreach (Point p in points)
            {
                g.FillRectangle(
                    new SolidBrush(Color.Red),
                    new Rectangle(p, new Size(3,3)));
            }
            //draw to the screen
            box.Refresh();
        }

    }
}
