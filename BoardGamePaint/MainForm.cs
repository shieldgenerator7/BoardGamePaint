using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BoardGamePaint
{
    public partial class MainForm: Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void pnlSpace_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = this.pnlSpace.CreateGraphics();
            Image image = pbxImage.Image;
            Rectangle rect = new Rectangle(10, 20, 100, 100);
            graphics.DrawImage(image, rect);
        }
    }
}
