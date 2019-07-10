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
        List<GameObject> gameObjects;
        List<WayPoint> wayPoints;
        List<GameObject> renderOrder;

        bool mouseDown = false;
        GameObject selected = null;
        Point mousePosition;

        readonly BinManager binManager = new BinManager();

        public MainForm()
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            gameObjects = new List<GameObject>();
            for (int i = 0; i < 5; i++)
            {
                gameObjects.Add(new GameObject(pbxImage.Image));
            }
            wayPoints = new List<WayPoint>();
            renderOrder = new List<GameObject>();
        }

        private void pnlSpace_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            //Draw the game objects
            if (renderOrder.Count > 0)
            {
                //Make sure smaller objects are drawn on top
                //Draw the objects
                foreach (GameObject gameObject in renderOrder)
                {
                    gameObject.draw(graphics);
                }
            }
            binManager.draw(graphics);
            graphics.DrawString("(" + this.Width + ", " + this.Height + ")",
                label1.Font,
                new SolidBrush(Color.Black),
                0,
                0
                );
            graphics.DrawString("(" + mousePosition.X + ", " + mousePosition.Y + ")",
                label1.Font,
                new SolidBrush(Color.Black),
                0,
                10
                );
        }

        private void pnlSpace_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            Vector mouseVector = e.Location.toVector();
            if (binManager.containsPosition(mouseVector))
            {
                Bin selectedBin = binManager.getBin(mouseVector);
                if (selectedBin)
                {
                    selected = selectedBin.makeNewObject();
                    addGameObject(selected);
                    selected.moveTo(mouseVector, false);
                    selected.pickup(mouseVector);
                }
                else
                {
                    selected = binManager;
                    selected.pickup(mouseVector);
                }
            }
            if (!selected)
            {
                //Find an object to select
                foreach (GameObject gameObject in gameObjects)
                {
                    if (gameObject.containsPosition(mouseVector))
                    {
                        selected = gameObject;
                        selected.pickup(mouseVector);
                        break;
                    }
                }
            }
        }

        private void pnlSpace_MouseMove(object sender, MouseEventArgs e)
        {
            mousePosition = e.Location;
            refresh();
            if (mouseDown)
            {
                if (selected)
                {
                    selected.moveTo(e.Location.toVector());
                }
                refresh();
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
                        //If twice the waypoint size is bigger than the selected
                        if (wayPoint.Size.toVector() * 2 > selected.Size.toVector())
                        {
                            selectedWayPoint = wayPoint;
                            selected.pickup(selected.Position);
                            selected.moveTo(wayPoint.Position);
                            break;
                        }
                    }
                }
            }
            selected = null;
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
            foreach (string filename in filenames)
            {
                binManager.makeBin(Image.FromFile(filename));
            }
            refresh();
        }

        private void pnlSpace_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void pnlSpace_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            addWayPoint(new WayPoint(
                pbxWayPoint.Image,
                e.Location.toVector(),
                new Size(100, 100),
                WayPoint.Shape.CIRCLE
                ));
            refresh();
        }

        void addGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
            gameObjects.Sort();
            renderOrder.Add(gameObject);
            renderOrder.Sort();
            renderOrder.Reverse();
        }
        void addWayPoint(WayPoint wayPoint)
        {
            wayPoints.Add(wayPoint);
            renderOrder.Add(wayPoint);
            renderOrder.Sort();
            renderOrder.Reverse();
        }

        void refresh()
        {
            //pnlSpace.Invalidate();
            this.Invalidate();
        }
    }
}
