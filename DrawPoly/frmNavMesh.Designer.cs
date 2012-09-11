namespace DrawPoly
{
    partial class frmNavMesh
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNavMesh));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.help = new System.Windows.Forms.ToolStripStatusLabel();
            this.dbg = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.box = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.btnDrawPoly = new System.Windows.Forms.ToolStripButton();
            this.btnNewBG = new System.Windows.Forms.ToolStripButton();
            this.btnShowNodes = new System.Windows.Forms.ToolStripButton();
            this.btnLink = new System.Windows.Forms.ToolStripButton();
            this.btnUnlink = new System.Windows.Forms.ToolStripButton();
            this.btnDeletePoly = new System.Windows.Forms.ToolStripButton();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClearBG = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.box)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblMode,
            this.help,
            this.dbg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 294);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(475, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblMode
            // 
            this.lblMode.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(38, 17);
            this.lblMode.Text = "Mode";
            // 
            // help
            // 
            this.help.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.help.Name = "help";
            this.help.Size = new System.Drawing.Size(138, 17);
            this.help.Text = "Context specific tool help";
            // 
            // dbg
            // 
            this.dbg.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dbg.Name = "dbg";
            this.dbg.Size = new System.Drawing.Size(61, 17);
            this.dbg.Text = "debug data";
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.box);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(475, 269);
            this.panel1.TabIndex = 2;
            // 
            // box
            // 
            this.box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.box.Location = new System.Drawing.Point(0, 0);
            this.box.Name = "box";
            this.box.Size = new System.Drawing.Size(475, 269);
            this.box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.box.TabIndex = 0;
            this.box.TabStop = false;
            this.box.MouseClick += new System.Windows.Forms.MouseEventHandler(this.box_MouseClick);
            this.box.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_MouseDown);
            this.box.MouseMove += new System.Windows.Forms.MouseEventHandler(this.box_MouseMove);
            this.box.MouseUp += new System.Windows.Forms.MouseEventHandler(this.box_MouseUp);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.saveToolStripButton,
            this.openToolStripButton,
            this.toolStripSeparator,
            this.btnDrawPoly,
            this.btnDeletePoly,
            this.toolStripSeparator1,
            this.btnLink,
            this.btnUnlink,
            this.toolStripSeparator2,
            this.btnNewBG,
            this.btnClearBG,
            this.toolStripSeparator5,
            this.btnShowNodes});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(475, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "&Save";
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDrawPoly
            // 
            this.btnDrawPoly.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDrawPoly.Image = ((System.Drawing.Image)(resources.GetObject("btnDrawPoly.Image")));
            this.btnDrawPoly.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnDrawPoly.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDrawPoly.Name = "btnDrawPoly";
            this.btnDrawPoly.Size = new System.Drawing.Size(23, 22);
            this.btnDrawPoly.Text = "Draw Polygon";
            this.btnDrawPoly.Click += new System.EventHandler(this.btnDrawPoly_Click);
            // 
            // btnNewBG
            // 
            this.btnNewBG.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNewBG.Image = ((System.Drawing.Image)(resources.GetObject("btnNewBG.Image")));
            this.btnNewBG.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNewBG.Name = "btnNewBG";
            this.btnNewBG.Size = new System.Drawing.Size(23, 22);
            this.btnNewBG.Text = "Add backdrop";
            this.btnNewBG.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // btnShowNodes
            // 
            this.btnShowNodes.CheckOnClick = true;
            this.btnShowNodes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnShowNodes.Image = ((System.Drawing.Image)(resources.GetObject("btnShowNodes.Image")));
            this.btnShowNodes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowNodes.Name = "btnShowNodes";
            this.btnShowNodes.Size = new System.Drawing.Size(23, 22);
            this.btnShowNodes.Text = "Show Nodes";
            this.btnShowNodes.Click += new System.EventHandler(this.btnShowNodes_Click);
            // 
            // btnLink
            // 
            this.btnLink.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLink.Image = ((System.Drawing.Image)(resources.GetObject("btnLink.Image")));
            this.btnLink.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLink.Name = "btnLink";
            this.btnLink.Size = new System.Drawing.Size(23, 22);
            this.btnLink.Text = "Link Edges";
            // 
            // btnUnlink
            // 
            this.btnUnlink.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUnlink.Image = ((System.Drawing.Image)(resources.GetObject("btnUnlink.Image")));
            this.btnUnlink.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUnlink.Name = "btnUnlink";
            this.btnUnlink.Size = new System.Drawing.Size(23, 22);
            this.btnUnlink.Text = "Break Link";
            // 
            // btnDeletePoly
            // 
            this.btnDeletePoly.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeletePoly.Image = ((System.Drawing.Image)(resources.GetObject("btnDeletePoly.Image")));
            this.btnDeletePoly.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeletePoly.Name = "btnDeletePoly";
            this.btnDeletePoly.Size = new System.Drawing.Size(23, 22);
            this.btnDeletePoly.Text = "Delete Polygon";
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.newToolStripButton.Text = "&New";
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openToolStripButton.Text = "&Open";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // btnClearBG
            // 
            this.btnClearBG.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClearBG.Image = ((System.Drawing.Image)(resources.GetObject("btnClearBG.Image")));
            this.btnClearBG.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClearBG.Name = "btnClearBG";
            this.btnClearBG.Size = new System.Drawing.Size(23, 22);
            this.btnClearBG.Text = "Clear backdrop";
            // 
            // frmNavMesh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 316);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "frmNavMesh";
            this.Text = "garden Navmesh";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.box)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox box;
        private System.Windows.Forms.ToolStripStatusLabel help;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton btnDrawPoly;
        private System.Windows.Forms.ToolStripButton btnNewBG;
        private System.Windows.Forms.ToolStripStatusLabel lblMode;
        private System.Windows.Forms.ToolStripStatusLabel dbg;
        private System.Windows.Forms.ToolStripButton btnShowNodes;
        private System.Windows.Forms.ToolStripButton btnLink;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripButton btnDeletePoly;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnUnlink;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnClearBG;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    }
}

