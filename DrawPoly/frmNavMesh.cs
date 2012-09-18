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
        #region Constants
        const int selectLineDist = 12;
        #endregion

        #region Fields, props, constructers and startups
        private List<Point> points = new List<Point>();
        Graphics g;
        private Point currentDrawPoint;
        private Image image;
        private bool saved = true;

        private List<Polygon> polygons = new List<Polygon>();
        private Polygon selectPoly;
        private Polygon selectedPoly;
        private Polygon[] linkPolys = new Polygon[2];

        private int moveVertexIndex;
        private Point movePolyPoint;

        private List<Link> links = new List<Link>();
        private Line linkStart;
        private Line linkEnd;
        private Line selectLine;

        private List<CNode> cnodes = new List<CNode>();


        public enum Mode
        {
            Select, Draw, Edit, Link, Delete, Unlink
        }

        private enum DragState
        {
            None, Vertex, Polygon
        }

        private Mode mode = Mode.Select;
        private DragState drag = DragState.None;

        public frmNavMesh()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            help.Text = "";
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
                    saved = false;
                    //create new polygon
                    if (points.Count > 2 && MathHelper.GetDistanceBetweenPoints(e.Location, points.First()) <= 20)
                    {
                        Polygon newPoly = new Polygon(points);
                        foreach (Polygon poly in polygons)
                        {
                            if (poly.ID >= newPoly.ID)
                                newPoly.ID = poly.ID + 1;
                        }
                        polygons.Add(newPoly);
                        points.Clear();
                    }
                    else
                        points.Add(currentDrawPoint);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    saved = false;
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
                        ClearMode();
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

            #region Delete Mode
            if (mode == Mode.Delete)
            {
                if (e.Button == MouseButtons.Left && selectPoly != null)
                {
                    if (MessageBox.Show("Deleting polygons cannot be undone.",
                        "Confirm Polycide", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        //delete polygon from list
                        breakAllLinks(selectPoly);
                        polygons.Remove(selectPoly);
                        selectPoly = null;
                        RedrawNodes();
                        Render();
                        saved = false;
                    }
                }
               if (e.Button == MouseButtons.Right)
                {
                    btnDeletePoly.Checked = false;
                    mode = Mode.Select;
                    selectPoly = null;
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
                    selectedPoly = null;
                    linkPolys = new Polygon[2];
                    linkStart = null;
                    linkEnd = null;
                    mode = Mode.Select;
                    ClearMode();
                }
                if (e.Button == MouseButtons.Left)
                {
                    if (linkStart == null && selectLine != null)
                    {
                        //add first edge
                        linkStart = selectLine;
                        linkPolys[0] = selectedPoly;
                    }
                    else if (linkStart != null && linkEnd==null && selectLine != null)
                    {
                        //dont let people add the same polygon twice
                        if (selectedPoly.Centroid != linkPolys[0].Centroid)
                        {
                            //second edge and create link

                            saved = false;
                            linkEnd = selectLine;
                            linkPolys[1] = selectedPoly;
                            Link newLink = new Link(linkStart, linkEnd, linkPolys[0], linkPolys[1]);
                            
                            //dont let the user create the same link twice
                            Boolean linkExists = false;
                            foreach (Link link in links)
                            {
                                if (link.ConnectingLine().Midpoint == newLink.ConnectingLine().Midpoint)
                                    linkExists = true;
                            }

                            if(!linkExists)
                                links.Add(newLink);

                            selectedPoly = null;
                            linkStart = null;
                            linkEnd = null;
                            linkPolys = new Polygon[2];
                        }
                    }
                }
            }
            #endregion

            RedrawNodes();

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
                    help.Text = "Click to complete the polygon";
                }
            }
            #endregion

            #region Select Mode
            if (mode == Mode.Select && polygons.Count < 1)
                help.Text = "No polygons to select yet";
            if (mode == Mode.Select && polygons.Count > 0)
            {
                selectPoly = null;
                help.Text = "Select a polygon";
                foreach (Polygon poly in polygons)
                {
                    if (poly.Intersects(e.Location))
                    {
                        selectPoly = poly;
                        help.Text = "Click to edit this polygon";
                    }
                }
            }
            #endregion

            #region Delete Mode
            if (mode == Mode.Delete && polygons.Count < 1)
                help.Text = "No polygons to delete";
            if (mode == Mode.Delete && polygons.Count > 0)
            {
                selectPoly = null;
                help.Text = "Select a polygon to delete";
                foreach (Polygon poly in polygons)
                {
                    if (poly.Intersects(e.Location))
                    {
                        selectPoly = poly;
                        help.Text = "Click to remove this polygon";
                    }
                }
            }
            #endregion

            #region Edit Mode
            if (mode == Mode.Edit && selectedPoly != null)
                help.Text = "Edit the vertices or move the polygon";

            //move the currently selected vertex
            if (mode == Mode.Edit && drag == DragState.Vertex)
            {
                selectedPoly.Vertices[moveVertexIndex] = e.Location;
                RedrawNodes();
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
                RedrawNodes();
            }
            #endregion

            #region Link Mode
            if (mode == Mode.Link)
            {
                if (linkStart != null && linkEnd == null)
                {
                }
                
                //iterate through all polygon edges
                int distance = 1000000000;
                selectLine = null;
                foreach (Polygon poly in polygons)
                {
                    Line l = poly.getClosestEdge(currentDrawPoint);
                    Point p = l.nearestPoint(currentDrawPoint);

                    int d = (int)MathHelper.GetDistanceBetweenPoints(p, currentDrawPoint);

                    if (l.isPointWithinSegment(p) && d <= selectLineDist)
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
                    saved = false;
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
                    //break links
                    breakAllLinks(selectedPoly);
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
                help.Text = "Click to start drawing a new polygon or right click to exit drawing mode";

            //draw current line
            if (points.Count > 0 && mode == Mode.Draw)
            {
                help.Text = "Click to draw another vertex or right click to delete the previous one";
                if (points.Count > 2 && MathHelper.GetDistanceBetweenPoints(currentDrawPoint, points.First()) <= 20)
                    help.Text = "Click to complete the polygon";
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
                    if (mode == Mode.Select || mode == Mode.Edit || mode== Mode.Delete)
                    {
                        if (poly != selectPoly && poly != selectedPoly)
                            poly.Draw(g);
                        else if (poly == selectPoly)
                        {
                            Color fill = poly.IsConcave() ? Color.FromArgb(100, Color.Red) : Color.FromArgb(100, Color.MediumBlue);
                            g.FillPolygon(new SolidBrush(fill), selectPoly.VerticesArray);

                            Color outline = (mode == Mode.Delete) ? Color.Red : Color.Orange;
                            g.DrawPolygon(new Pen(outline, 4), selectPoly.VerticesArray);
                        }
                    }
                    else
                    {
                        poly.Draw(g);
                    }
                }
            }
            #endregion

            #region Draw Links in list
            if (links.Count > 0)
            {
                foreach (Link link in links)
                {
                    Line l = link.ConnectingLine();
                    g.DrawLine(new Pen(Color.Turquoise, 5), l.Start, l.End);
                }
            }
            #endregion

            #region Draw Selected Polygon
            if (mode == Mode.Edit || mode == Mode.Select && selectedPoly != null)
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

                        help.Text = "Drag this vertex around if you want";
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

            #region Draw Selected Edge for Link mode
            if (mode == Mode.Link)
            {
                help.Text = "Select an edge";
                if (selectLine != null)
                {
                    if(linkPolys[0] == null)
                        g.DrawLine(new Pen(Color.LimeGreen, 3), selectLine.Start, selectLine.End);
                    if(linkPolys[0] != null && linkPolys[0].Centroid != selectedPoly.Centroid)
                        g.DrawLine(new Pen(Color.LimeGreen, 3), selectLine.Start, selectLine.End);
                }
                if (linkStart != null && selectLine != null)
                {
                    g.DrawLine(new Pen(Color.LimeGreen, 3), linkStart.Start, linkStart.End);
                }
            }
            #endregion

            #region Draw Nodes
            if (cnodes.Count > 0 && btnShowNodes.Checked)
            {
                foreach (CNode node in cnodes)
                {
                    node.Draw(g);
                }
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

        #region Methods

        private void breakAllLinks(Polygon poly)
        {
            List<Link> removeLinks = new List<Link>();
            foreach (Link link in links)
            {
                if (link.StartPoly.Centroid == poly.Centroid || link.EndPoly.Centroid == poly.Centroid)
                {
                    removeLinks.Add(link);
                }

            }
            foreach (Link link in removeLinks)
            {
                links.Remove(link);
            }
        }

        private void CreateNodeMap()
        {
            cnodes.Clear();

            //centroids
            foreach (Polygon poly in polygons)
            {
                CNode n = new CNode(poly.Centroid);
                n.Associate(poly);

                cnodes.Add(n);
            }

            //midpoints
            foreach (Link link in links)
            {
                CNode n = new CNode(link.ConnectingLine().Midpoint);
                n.Associate(link.StartPoly);
                n.Associate(link.EndPoly);

                cnodes.Add(n);
            }

            //connect nodes
            foreach (CNode n in cnodes)
            {
                foreach (CNode n2 in cnodes)
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

        private void RedrawNodes()
        {
            if (polygons.Count > 0 && btnShowNodes.Checked)
            {
                CreateNodeMap();
            }
        }

        private void NewMesh()
        {

            polygons.Clear();
            links.Clear();
            cnodes.Clear();
            ClearMode();

            this.Size = new Size(491, 354);
            mode = Mode.Select;
            image = new Bitmap(panel1.Width, panel1.Height);
            redrawGraphics();
            Render();
        }

        private void SaveMesh()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "Save Navmesh";
            save.Filter = "garden Navmesh|*.nav";
            save.ShowDialog();

            if (save.FileName != "")
            {
                NavMesh mesh = new NavMesh(polygons, links);
                mesh.writeToFile(save.FileName);
                saved = true;
            }
        }

        private void LoadMesh()
        {
            OpenFileDialog load = new OpenFileDialog();
            load.Title = "Load Navmesh";
            load.Filter = "garden Navmesh|*.nav|Plaintext File|*.txt;*.xml|All Files|*.*";
            load.ShowDialog();

            if (load.FileName != "")
            {
                NavMesh mesh = new NavMesh();
                mesh.readFromFile(load.FileName);
                LoadFromMesh(mesh);
                saved = true;
            }
        }

        private void LoadFromMesh(NavMesh mesh)
        {
            NewMesh();
            polygons = mesh.Polygons;
            links = mesh.Links;
        }

        private void loadNewBG()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Load image as background";
            open.Filter = "Image Files|*.png;*.jpg;*.bmp;*.gif";
            open.ShowDialog();

            if (open.FileName != "")
            {
                this.image = Image.FromFile(open.FileName);
                this.Size = new Size(image.Size.Width + 18, image.Size.Height + 95);
                redrawGraphics();
                Render();
            }
        }

        private void ClearMode()
        {
            btnLink.Checked = false;
            btnDrawPoly.Checked = false;
            btnDeletePoly.Checked = false;
            btnUnlink.Checked = false;

            selectedPoly = null;
            selectLine = null;
            selectPoly = null;
            linkEnd = null;
            linkStart = null;
            linkPolys = new Polygon[2];
            points.Clear();
        }

        private void Verify()
        {
            bool concave = false;
            bool overlap = false;

            foreach (Polygon poly in polygons)
            {
                if (poly.IsConcave())
                    concave = true;
                foreach (Point v in poly.Vertices)
                {
                    foreach (Polygon testPoly in polygons)
                    {
                        if (testPoly == poly)
                            continue;
                        if (testPoly.Intersects(v))
                            overlap = true;
                    }
                }
            }
            string message = "Navmesh is ok!";

            if (concave)
                message = "Some of your polygons are concave. \nGo back and fix any polys that are showing up red.";
            if (overlap)
                message = "You have overlapping polygons. \nPolys should be placed closely, but not overlapping.";

            if (concave && overlap)
            {
                message = "Some of your polygons are concave. \nGo back and fix any polys that are showing up red.";
                message += "\n\nYou have overlapping polygons. \nPolys should be placed closely, but not overlapping.";
            }


            MessageBox.Show(message, "Verify Mesh");
        }
        #endregion

        #region UI Events

        private void btnDrawPoly_Click(object sender, EventArgs e)
        {
            ClearMode();
            btnDrawPoly.Checked = true;
            mode = Mode.Draw;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
            {
                loadNewBG();
            }
        private void btnLink_Click(object sender, EventArgs e)
        {
            ClearMode();
            btnLink.Checked = true;
            linkStart = null;
            linkEnd = null;
            mode = Mode.Link;
        }
        private void generateNodemapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNodeMap();
        }
        private void btnShowNodes_Click(object sender, EventArgs e)
        {
            CreateNodeMap();
            Render();
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            if (polygons.Count > 0)
            {
                if (MessageBox.Show("Discard current mesh?", "New Mesh", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    NewMesh();
                }
            }
        }
        private void btnClearBG_Click(object sender, EventArgs e)
        {
            this.Size = new Size(651, 434);
            image = new Bitmap(panel1.Width, panel1.Height);
            redrawGraphics();
            Render();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveMesh();
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (polygons.Count > 0 && !saved)
            {
                if (MessageBox.Show("Discard the current mesh?", "Load Mesh", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    LoadMesh();
                }
            }
            else
                LoadMesh();
        }
        private void btnDeletePoly_Click(object sender, EventArgs e)
        {
            ClearMode();
            btnDeletePoly.Checked = true;
            mode = Mode.Delete;
        }
        private void btnUnlink_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This button doesn't work yet. \nIf you need to break a link, moving a polygon will break all links connected to it.", "Temporary Help Robot");
        }
        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Just click around and figure stuff out. Start by clicking the draw polygon tool.\nIf you draw a polygon and it shows up red, that means it's a concave polygon.\n\nThose aren't allowed.","Temporary Help Robot");
        }
        private void btnTest_Click(object sender, EventArgs e)
        {
            Verify();
        }
        #endregion

        #region UI Tooltip
        private void btnNew_MouseMove(object sender, MouseEventArgs e)
        {
            lblMode.Text = "New";
            help.Text = "Discard the current navmesh and create a new one";
        }

        private void btnSave_MouseMove(object sender, MouseEventArgs e)
        {
            lblMode.Text = "Save";
            help.Text = "Save the navmesh to disc";
        }

        private void btnOpen_MouseMove(object sender, MouseEventArgs e)
        {
            lblMode.Text = "Open";
            help.Text = "Open a navmesh file";
        }

        private void btnDrawPoly_MouseMove(object sender, MouseEventArgs e)
        {
            lblMode.Text = "Draw";
            help.Text = "Draw a new polygon which represents a walkable area";
        }

        private void btnDeletePoly_MouseMove(object sender, MouseEventArgs e)
        {
            lblMode.Text = "Delete";
            help.Text = "Remove a polygon from the mesh";
        }

        private void btnLink_MouseMove(object sender, MouseEventArgs e)
        {
            lblMode.Text = "Link";
            help.Text = "Link edges to create a connection between polygons";
        }

        private void btnUnlink_MouseMove(object sender, MouseEventArgs e)
        {
            lblMode.Text = "Unlink";
            help.Text = "Remove a link between polygons";
        }

        private void btnNewBG_MouseMove(object sender, MouseEventArgs e)
        {
            lblMode.Text = "Background";
            help.Text = "Add a background image to draw over";
        }

        private void btnClearBG_MouseMove(object sender, MouseEventArgs e)
        {
            lblMode.Text = "Clear Background";
            help.Text = "Clear the background image";
        }

        private void btnShowNodes_MouseMove(object sender, MouseEventArgs e)
        {
            lblMode.Text = "Nodes";
            help.Text = "Toggles the pathfinding nodemap overlay";
        }

        private void btnTest_MouseMove(object sender, MouseEventArgs e)
        {
            lblMode.Text = "Verify";
            help.Text = "Check the mesh for errors";
        }

        private void btnHelp_MouseMove(object sender, MouseEventArgs e)
        {
            lblMode.Text = "Help";
            help.Text = "Helps you out";
        }
        #endregion


        
    }
}

