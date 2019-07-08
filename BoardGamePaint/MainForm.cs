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
        GameObject gameObject;

        public MainForm()
        {
            InitializeComponent();
            gameObject = new GameObject(pbxImage.Image);
        }

        private void pnlSpace_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = this.pnlSpace.CreateGraphics();
            graphics.Clear(Color.Wheat);
            gameObject.draw(graphics);
        }

        private void pbxImage_Click(object sender, EventArgs e)
        {
            gameObject.moveRight();
            pnlSpace.Refresh();
        }
    }
}
