using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Rectangle = System.Drawing.Rectangle;

namespace DrawPoly
{
    
    public partial class frmNavMesh : Form
    {
        #region Fields, props, constructers and startups
        private List<Point> points = new List<Point>();
        Graphics g;
        private Point currentDrawPoint;
        private Image image;

        private List<Polygon> polygons = new List<Polygon>();
        private Polygon selectPoly;
        private Polygon selectedPoly;
        private Polygon[] linkPolys = new Polygon[2];

        private int moveVertexIndex;
        private Point movePolyPoint;

        private Link testLink;
        private Line linkStart;
        private Line linkEnd;
        private Line selectLine;


        private List<Point> links = new List<Point>();

        public enum Mode
        {
            Select, Draw, Edit, Link
        }

        private enum DragState
        {
            None, Vertex, Polygon
        }

        Mode mode = Mode.Select;
        DragState drag = DragState.None;

        public frmNavMesh()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            dbg.Text = "";
            redrawGraphics();
            Render();
        }
        #endregion

        #region Input

        private void box_MouseClick(object sender, MouseEventArgs e)
        {

            #region Drawing Mode

            if (mode == Mode.Draw)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (points.Count > 2 && MathHelper.GetDistanceBetweenPoints(e.Location, points.First()) <= 20)
                    {
                        polygons.Add(new Polygon(points));
                        points.Clear();
                    }
                    else
                        points.Add(currentDrawPoint);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (points.Count > 1)
                    {
                        points.RemoveAt(points.Count - 1);
                    }
                    else if (points.Count == 1)
                    {
                        points.Clear();
                    }
                    else if (points.Count == 0)
                    {
                        mode = Mode.Select;
                    }
                }
            }
            #endregion

            #region Select Mode
            if (mode == Mode.Select && selectPoly != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    selectedPoly = selectPoly;
                    selectPoly = null;

                    mode = Mode.Edit;
                }
            }
            #endregion

            #region Edit Mode
            if (mode == Mode.Edit && e.Button == MouseButtons.Right)
            {
                mode = Mode.Select;
                selectedPoly = null;
            }
            #endregion

            #region Link Mode
            if (mode == Mode.Link)
            {
                if (e.Button == MouseButtons.Right)
                {
                    mode = Mode.Select;
                }
                if (e.Button == MouseButtons.Left)
                {
                    if (linkStart == null && selectLine != null)
                    {
                        //add first edge
                        linkStart = selectLine;
                    }
                    else if (linkStart != null && linkEnd==null && selectLine != null)
                    {
                        //second edge
                        linkEnd = selectLine;
                        //TODO: Create new link here
                    }
                }
            }
            #endregion

        }

        private void box_MouseMove(object sender, MouseEventArgs e)
        {
            currentDrawPoint = e.Location;

            #region Drawing mode
            if (mode == Mode.Draw)
            {
                //lock on to the first point if the mouse is close enough and we have enough
                //points to make a closed polygon
                if (points.Count > 2 &&
                    MathHelper.GetDistanceBetweenPoints(e.Location, points.First()) <= 20)
                {
                    currentDrawPoint = points.First();
                    dbg.Text = "Click to complete the polygon";
                }
            }
            #endregion

            #region Select Mode
            if (mode == Mode.Select && polygons.Count > 0)
            {
                selectPoly = null;
                foreach (Polygon poly in polygons)
                {
                    if (poly.Intersects(e.Location))
                    {
                        selectPoly = poly;
                        dbg.Text = "Click to edit this polygon";
                    }
                }
            }
            #endregion

            #region Edit Mode
            if (mode == Mode.Edit && selectedPoly != null)
                dbg.Text = "Edit the vertices or move the polygon";

            //move the currently selected vertex
            if (mode == Mode.Edit && drag == DragState.Vertex)
            {
                selectedPoly.Vertices[moveVertexIndex] = e.Location;
            }
            //drag the whole polygon
            if (mode == Mode.Edit && drag == DragState.Polygon)
            {
                int deltaX = e.X - movePolyPoint.X;
                int deltaY = e.Y - movePolyPoint.Y;

                for (int i = 0; i < selectedPoly.Vertices.Count; i++)
                {
                    Point oldPoint = selectedPoly.Vertices[i];
                    oldPoint.Offset(deltaX, deltaY);
                    selectedPoly.Vertices[i] = oldPoint;
                }
                movePolyPoint = e.Location;
            }
            #endregion

            #region Link Mode
            if (mode == Mode.Link)
            {
                if (linkStart != null && linkEnd == null)
                {
                }
                
                //iterate through all polygon edges
                int distance = 1000000;
                selectLine = null;
                foreach (Polygon poly in polygons)
                {
                    Line l = poly.getClosestEdge(currentDrawPoint);

                    if (linkStart != null && l.isIdentical(linkStart))
                        continue;
                    if (linkEnd != null && l.isIdentical(linkEnd))
                        continue;

                    Point p = l.nearestPoint(currentDrawPoint);

                    int d = (int)MathHelper.GetDistanceBetweenPoints(currentDrawPoint, p);

                    if (l.isPointWithinSegment(p) && d <= 30)
                    {
                        //found a closer edge
                        if (d < distance)
                        {
                            selectLine = l;
                            distance = d;
                            selectedPoly = poly;
                        }
                    }

                }
            }
            #endregion

            this.Render();
        }

        private void box_MouseDown(object sender, MouseEventArgs e)
        {
            
            if (mode == Mode.Edit && e.Button == MouseButtons.Left)
            {

                if (drag == DragState.None)
                {

                    //drag polygon

                    if (selectedPoly.Intersects(e.Location))
                    {
                        movePolyPoint = e.Location;
                        drag = DragState.Polygon;
                    }

                    //drag vertex
                    for (int i = 0; i < selectedPoly.Vertices.Count; i++)
                    {
                        if (MathHelper.GetDistanceBetweenPoints(e.Location, selectedPoly.Vertices[i]) <= 15)
                        {
                            moveVertexIndex = i;
                            drag = DragState.Vertex;
                        }
                    }

               
                }
               

            }
        }

        private void box_MouseUp(object sender, MouseEventArgs e)
        {
            if (drag == DragState.Vertex || drag == DragState.Polygon)
            {
                drag = DragState.None;
            }
        }

        #endregion

        #region Graphics
        private void Render()
        {
            lblMode.Text = mode.ToString();
            g.Clear(Color.CornflowerBlue);
            if (image != null)
            {
                g.DrawImage(image, new Point(0, 0));
            }

            #region Draw Current Polygon
            
            //setup our output messages
            if (mode == Mode.Draw && points.Count == 0)
                dbg.Text = "Click to start drawing a new polygon or right click to exit drawing mode";

            //draw current line
            if (points.Count > 0 && mode == Mode.Draw)
            {
                dbg.Text = "Click to draw another vertex or right click to delete the previous one";
                if (points.Count > 2 && MathHelper.GetDistanceBetweenPoints(currentDrawPoint, points.First()) <= 20)
                    dbg.Text = "Click to complete the polygon";
                g.DrawLine(new Pen(Color.Orange, 3), points.Last<Point>(), currentDrawPoint);
                
                //if more than one point, link them all together
                if (points.Count > 1)
                {
                    g.DrawLines(new Pen(Color.Orange, 2), points.ToArray<Point>());
                }
                foreach (Point p in points)
                {
                    p.Offset(-5, -5);
                    g.FillEllipse(
                        new SolidBrush(Color.Orange),
                        new Rectangle(p, new Size(9, 9)));
                    g.DrawEllipse(
                        new Pen(Color.Orange, 2),
                        new Rectangle(p, new Size(9, 9)));
                }
            }
            #endregion

            #region Draw Polygons in list
            if (polygons.Count > 0)
            {
                foreach (Polygon poly in polygons)
                {
                    if (poly != selectPoly && poly != selectedPoly)
                        poly.Draw(g);
                    else if(poly == selectPoly)
                    {   
                        Color fill = poly.IsConcave() ? Color.FromArgb(100, Color.Red) : Color.FromArgb(100, Color.MediumBlue);
                        g.FillPolygon(new SolidBrush(fill), selectPoly.VerticesArray);

                        g.DrawPolygon(new Pen(Color.Orange, 4), selectPoly.VerticesArray);
                    }
                }
            }
            #endregion

            #region Draw Selected Polygon
            if (mode == Mode.Edit && selectedPoly != null)
            {

                //draw outline
                g.DrawPolygon(new Pen(Color.Orange, 3), selectedPoly.VerticesArray);
                g.FillPolygon(new SolidBrush(Color.FromArgb(50, Color.Orange)), selectedPoly.VerticesArray);

                //draw verts
                foreach (Point p in selectedPoly.Vertices)
                {
                    if (MathHelper.GetDistanceBetweenPoints(p, currentDrawPoint) <= 12)
                    {
                        p.Offset(-7, -7);
                        g.DrawEllipse(
                            new Pen(Color.Orange, 2),
                            new Rectangle(p, new Size(15, 15)));
                        g.FillEllipse(
                            new SolidBrush(Color.FromArgb(100, Color.Orange)),
                            new Rectangle(p, new Size(15, 15)));

                        dbg.Text = "Drag this vertex around if you want";
                        continue;
                    }


                    p.Offset(-5, -5);
                    g.FillEllipse(
                        new SolidBrush(Color.Orange),
                        new Rectangle(p, new Size(9, 9)));
                    g.DrawEllipse(
                        new Pen(Color.Orange, 2),
                        new Rectangle(p, new Size(9, 9)));
                }
            }
            #endregion

            #region Draw Links
            if (mode == Mode.Link)
            {
                dbg.Text = "Select an edge";
                if (selectLine != null)
                    g.DrawLine(new Pen(Color.LimeGreen, 5), selectLine.Start, selectLine.End);
            }
            #endregion

            //draw to the screen
            box.Refresh();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            redrawGraphics();
            this.Render();
        }

        private void redrawGraphics()
            {
                box.Image = new Bitmap(panel1.Width, panel1.Height);
                g = Graphics.FromImage(box.Image);
                g.SmoothingMode = SmoothingMode.HighQuality;
            }

        #endregion

        #region UI Events

        private void loadNewBG()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Load image as background";
            open.Filter = "Image Files|*.png;*.jpg;*.bmp;*.gif";
            open.ShowDialog();

            if (open.FileName != "")
            {
                this.image = Image.FromFile(open.FileName);
                this.Size = new Size(image.Size.Width+18, image.Size.Height+95);
                redrawGraphics();
                Render();
            }
        }

        private void btnDrawPoly_Click(object sender, EventArgs e)
        {
            mode = Mode.Draw;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
            {
                loadNewBG();
            }
        private void linkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            linkStart = null;
            linkEnd = null;
            mode = Mode.Link;
        }
        #endregion
    }
}
