namespace DrawPoly
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.polygonsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawNewPolyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editPolyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.dbg = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.box = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.box)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.polygonsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(427, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // polygonsToolStripMenuItem
            // 
            this.polygonsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawNewPolyToolStripMenuItem,
            this.editPolyToolStripMenuItem});
            this.polygonsToolStripMenuItem.Name = "polygonsToolStripMenuItem";
            this.polygonsToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.polygonsToolStripMenuItem.Text = "Polygons";
            // 
            // drawNewPolyToolStripMenuItem
            // 
            this.drawNewPolyToolStripMenuItem.Name = "drawNewPolyToolStripMenuItem";
            this.drawNewPolyToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.drawNewPolyToolStripMenuItem.Text = "Draw New Poly";
            this.drawNewPolyToolStripMenuItem.Click += new System.EventHandler(this.drawNewPolyToolStripMenuItem_Click);
            // 
            // editPolyToolStripMenuItem
            // 
            this.editPolyToolStripMenuItem.Name = "editPolyToolStripMenuItem";
            this.editPolyToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.editPolyToolStripMenuItem.Text = "Edit Poly";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dbg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 245);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(427, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // dbg
            // 
            this.dbg.Name = "dbg";
            this.dbg.Size = new System.Drawing.Size(16, 17);
            this.dbg.Text = "...";
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.box);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(427, 221);
            this.panel1.TabIndex = 2;
            // 
            // box
            // 
            this.box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.box.Location = new System.Drawing.Point(0, 0);
            this.box.Name = "box";
            this.box.Size = new System.Drawing.Size(427, 221);
            this.box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.box.TabIndex = 0;
            this.box.TabStop = false;
            this.box.Paint += new System.Windows.Forms.PaintEventHandler(this.box_Paint);
            this.box.MouseClick += new System.Windows.Forms.MouseEventHandler(this.box_MouseClick);
            this.box.MouseMove += new System.Windows.Forms.MouseEventHandler(this.box_MouseMove);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 267);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Draw Polygons";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.box)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem polygonsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem drawNewPolyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editPolyToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox box;
        private System.Windows.Forms.ToolStripStatusLabel dbg;
    }
}

