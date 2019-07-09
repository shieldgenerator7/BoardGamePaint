namespace BoardGamePaint
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pnlSpace = new System.Windows.Forms.Panel();
            this.pbxWayPoint = new System.Windows.Forms.PictureBox();
            this.pbxImage = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlSpace.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxWayPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSpace
            // 
            this.pnlSpace.AllowDrop = true;
            this.pnlSpace.AutoSize = true;
            this.pnlSpace.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlSpace.BackColor = System.Drawing.Color.OliveDrab;
            this.pnlSpace.Controls.Add(this.label1);
            this.pnlSpace.Controls.Add(this.pbxWayPoint);
            this.pnlSpace.Controls.Add(this.pbxImage);
            this.pnlSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSpace.Location = new System.Drawing.Point(0, 0);
            this.pnlSpace.Name = "pnlSpace";
            this.pnlSpace.Size = new System.Drawing.Size(800, 450);
            this.pnlSpace.TabIndex = 0;
            this.pnlSpace.Visible = false;
            this.pnlSpace.DragDrop += new System.Windows.Forms.DragEventHandler(this.pnlSpace_DragDrop);
            this.pnlSpace.DragEnter += new System.Windows.Forms.DragEventHandler(this.pnlSpace_DragEnter);
            this.pnlSpace.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlSpace_MouseClick);
            this.pnlSpace.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pnlSpace_MouseDoubleClick);
            this.pnlSpace.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlSpace_MouseDown);
            this.pnlSpace.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlSpace_MouseMove);
            this.pnlSpace.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlSpace_MouseUp);
            // 
            // pbxWayPoint
            // 
            this.pbxWayPoint.Image = ((System.Drawing.Image)(resources.GetObject("pbxWayPoint.Image")));
            this.pbxWayPoint.Location = new System.Drawing.Point(700, 24);
            this.pbxWayPoint.Name = "pbxWayPoint";
            this.pbxWayPoint.Size = new System.Drawing.Size(100, 114);
            this.pbxWayPoint.TabIndex = 1;
            this.pbxWayPoint.TabStop = false;
            // 
            // pbxImage
            // 
            this.pbxImage.Image = ((System.Drawing.Image)(resources.GetObject("pbxImage.Image")));
            this.pbxImage.Location = new System.Drawing.Point(599, 24);
            this.pbxImage.Name = "pbxImage";
            this.pbxImage.Size = new System.Drawing.Size(100, 114);
            this.pbxImage.TabIndex = 0;
            this.pbxImage.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlSpace);
            this.Name = "MainForm";
            this.Text = "Board Game Paint";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.pnlSpace_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.pnlSpace_DragEnter);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlSpace_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlSpace_MouseClick);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pnlSpace_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlSpace_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlSpace_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlSpace_MouseUp);
            this.pnlSpace.ResumeLayout(false);
            this.pnlSpace.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxWayPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlSpace;
        private System.Windows.Forms.PictureBox pbxImage;
        private System.Windows.Forms.PictureBox pbxWayPoint;
        private System.Windows.Forms.Label label1;
    }
}