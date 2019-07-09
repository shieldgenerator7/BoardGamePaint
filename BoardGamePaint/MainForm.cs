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
    public partial class MainForm : Form
    {
        GameObject gameObject;

        bool mouseDown = false;

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

        private void pnlSpace_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            gameObject.pickup(e.Location.toVector());
        }

        private void pnlSpace_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                gameObject.moveTo(e.Location.toVector());
                pnlSpace.Refresh();
            }
        }

        private void pnlSpace_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}
