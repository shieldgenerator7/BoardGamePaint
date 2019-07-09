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
        List<GameObject> gameObjects;
        List<WayPoint> wayPoints;

        bool mouseDown = false;
        GameObject selected = null;

        public MainForm()
        {
            InitializeComponent();
            gameObjects = new List<GameObject>();
            for (int i = 0; i < 5; i++)
            {
                gameObjects.Add(new GameObject(pbxImage.Image));
            }
            wayPoints = new List<WayPoint>();
        }

        private void pnlSpace_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = this.pnlSpace.CreateGraphics();
            //Clear the board
            graphics.Clear(Color.Wheat);
            //Draw the waypoints
            foreach (WayPoint wayPoint in wayPoints)
            {
                wayPoint.draw(graphics);
            }
            //Draw the game objects
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.draw(graphics);
            }
        }

        private void pnlSpace_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.containsPosition(e.Location.toVector()))
                {
                    selected = gameObject;
                    selected.pickup(e.Location.toVector());
                    break;
                }
            }
        }

        private void pnlSpace_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                if (selected)
                {
                    selected.moveTo(e.Location.toVector());
                }
                pnlSpace.Refresh();
            }
        }

        private void pnlSpace_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            if (selected)
            {
                WayPoint selectedWayPoint = null;
                foreach (WayPoint wayPoint in wayPoints)
                {
                    if (wayPoint.containsPosition(e.Location.toVector()))
                    {
                        selectedWayPoint = wayPoint;
                        selected.pickup(selected.Position);
                        selected.moveTo(wayPoint.Position);
                        break;
                    }
                }
            }
            selected = null;
            pnlSpace.Refresh();
        }

        //2019-07-08: drag and drop copied from https://www.youtube.com/watch?v=d0J3VKBA4Xs
        private void pnlSpace_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void pnlSpace_DragDrop(object sender, DragEventArgs e)
        {
            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string filename in filenames)
            {
                gameObjects.Add(new GameObject(
                    Image.FromFile(filename)
                    ));
            }
            pnlSpace.Refresh();
        }

        private void pnlSpace_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void pnlSpace_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            wayPoints.Add(new WayPoint(
                pbxWayPoint.Image,
                e.Location.toVector(),
                new Size(100, 100),
                WayPoint.Shape.CIRCLE
                ));
            pnlSpace.Refresh();
        }
    }
}
