using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BoardGamePaint
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            //
            Managers.init(this);
        }

        private void pnlSpace_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            //Draw the game objects
            Managers.Display.displayObjects(graphics);
            Managers.Display.displayRectangles(graphics);
            Managers.Display.displayDescription(graphics);
            ////Debug info
            //graphics.DrawString("(" + this.Width + ", " + this.Height + ")",
            //    label1.Font,
            //    new SolidBrush(Color.Black),
            //    0,
            //    0
            //    );
            //graphics.DrawString("(" + mousePosition.X + ", " + mousePosition.Y + ")",
            //    label1.Font,
            //    new SolidBrush(Color.Black),
            //    0,
            //    10
            //    );
        }

        private void pnlSpace_MouseDown(object sender, MouseEventArgs e)
        {
            Managers.Control.mouseDown();
        }

        private void pnlSpace_MouseMove(object sender, MouseEventArgs e)
        {
            Vector mouseVector = e.Location.toVector();
            if (Managers.Control.mouseMove(mouseVector))
            {
                refresh();
            }
        }

        private void pnlSpace_MouseUp(object sender, MouseEventArgs e)
        {
            Managers.Control.mouseUp();
            refresh();
        }

        //2019-07-08: drag and drop copied from https://www.youtube.com/watch?v=d0J3VKBA4Xs
        private void pnlSpace_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void pnlSpace_DragDrop(object sender, DragEventArgs e)
        {
            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop);
            ObjectImportManager.importObjects(filenames);
            refresh();
        }

        private void pnlSpace_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Managers.Control.mouseDoubleClick();
            refresh();
        }

        void refresh()
        {
            //pnlSpace.Invalidate();
            this.Invalidate();
        }

        private void MainForm_MouseHover(object sender, EventArgs e)
        {
            Managers.Control.mouseHover();
        }
    }
}
