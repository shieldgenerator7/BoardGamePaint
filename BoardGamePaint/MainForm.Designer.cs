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
            this.pnlSpace = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlSpace
            // 
            this.pnlSpace.AllowDrop = true;
            this.pnlSpace.AutoSize = true;
            this.pnlSpace.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlSpace.BackColor = System.Drawing.Color.OliveDrab;
            this.pnlSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSpace.Location = new System.Drawing.Point(0, 0);
            this.pnlSpace.Name = "pnlSpace";
            this.pnlSpace.Size = new System.Drawing.Size(800, 450);
            this.pnlSpace.TabIndex = 0;
            this.pnlSpace.Visible = false;
            this.pnlSpace.DragDrop += new System.Windows.Forms.DragEventHandler(this.pnlSpace_DragDrop);
            this.pnlSpace.DragEnter += new System.Windows.Forms.DragEventHandler(this.pnlSpace_DragEnter);
            this.pnlSpace.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pnlSpace_MouseDoubleClick);
            this.pnlSpace.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlSpace_MouseDown);
            this.pnlSpace.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlSpace_MouseMove);
            this.pnlSpace.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlSpace_MouseUp);
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
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pnlSpace_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlSpace_MouseDown);
            this.MouseHover += new System.EventHandler(this.MainForm_MouseHover);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlSpace_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlSpace_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlSpace;
    }
}