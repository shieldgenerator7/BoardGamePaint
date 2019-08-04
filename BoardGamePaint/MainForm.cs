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
        int BRUSH_THICKNESS = 4;
        bool WAYPOINTS_ENABLED = false;

        List<GameObject> gameObjects;
        List<WayPoint> wayPoints;
        List<GameObject> renderOrder;

        bool mouseDown = false;
        GameObject selected = null;
        GameObject mousedOver = null;
        Point mousePosition;
        bool mouseHover = false;

        Pen selectPen;
        Pen deletePen;
        Pen anchorPen;
        Pen changePen;
        Pen createPen;

        public MainForm()
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            //
            selectPen = new Pen(Color.FromArgb(155, 155, 155), BRUSH_THICKNESS);
            deletePen = new Pen(Color.FromArgb(247, 70, 70), BRUSH_THICKNESS);
            anchorPen = new Pen(Color.FromArgb(52, 175, 0), BRUSH_THICKNESS);
            changePen = new Pen(Color.FromArgb(137, 206, 255), BRUSH_THICKNESS);
            createPen = new Pen(Color.FromArgb(255, 230, 107), BRUSH_THICKNESS);
            //
            gameObjects = new List<GameObject>();
            for (int i = 0; i < 5; i++)
            {
                gameObjects.Add(new GameObject(pbxImage.Image));
            }
            wayPoints = new List<WayPoint>();
            renderOrder = new List<GameObject>();
            //
            Managers.init(this);
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
            Managers.Bin.draw(graphics);
            Managers.Command.draw(graphics);
            if (!mousedOver)
            {
                mousedOver = selected;
            }
            if (mousedOver)
            {
                if (mouseDown)
                {
                    Vector mouseVector = mousePosition.toVector();
                    //in the middle of a drag
                    if (!(mousedOver is Tray)
                        && !(mousedOver is TrayComponent)
                        && Managers.Bin.containsPosition(mouseVector))
                    {
                        graphics.DrawRectangle(deletePen, mousedOver.getRect());
                    }
                    else if (mousedOver is Tray)
                    {
                        graphics.DrawRectangle(selectPen, mousedOver.getRect());
                    }
                    else
                    {
                        //check to see if it's going to anchor on anything
                        GameObject anchorObject = null;
                        foreach (GameObject gameObject in gameObjects)
                        {
                            if (gameObject != mousedOver
                                && gameObject.containsPosition(mouseVector)
                                && gameObject > mousedOver)
                            {
                                anchorObject = gameObject;
                                break;
                            }
                        }
                        if (anchorObject)
                        {
                            if (anchorObject is CardDeck
                                && ((CardDeck)anchorObject).fitsInDeck(mousedOver))
                            {
                                graphics.DrawRectangle(changePen, mousedOver.getRect());
                                graphics.DrawRectangle(changePen, anchorObject.getRect());
                            }
                            else
                            {
                                graphics.DrawRectangle(anchorPen, mousedOver.getRect());
                                graphics.DrawRectangle(anchorPen, anchorObject.getRect());
                            }
                        }
                        else
                        {
                            graphics.DrawRectangle(selectPen, mousedOver.getRect());
                        }
                    }
                }
                else
                {
                    //just mousing over things
                    if (mousedOver is Bin || mousedOver.canMakeNewObject(mousePosition.toVector()))
                    {
                        graphics.DrawRectangle(createPen, mousedOver.getRect());
                    }
                    else if (mousedOver.canChangeState())
                    {
                        graphics.DrawRectangle(changePen, mousedOver.getRect());
                    }
                    else
                    {
                        graphics.DrawRectangle(selectPen, mousedOver.getRect());
                    }
                    //Object Description
                    if (true || mouseHover)
                    {
                        graphics.DrawString(mousedOver.Description,
                            label1.Font,
                            new SolidBrush(Color.Black),
                            mousePosition.X - 5,
                            mousePosition.Y - 25
                            );
                    }
                }
            }
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
            mouseHover = false;
            mouseDown = true;
            Vector mouseVector = e.Location.toVector();
            selected = mousedOver;
            selected?.pickup(mouseVector);
            if (selected && selected.canMakeNewObject(mouseVector))
            {
                selected = selected.makeNewObject();
                addGameObject(selected);
                selected.moveTo(mouseVector, false);
                selected.pickup(mouseVector);
                mousedOver = selected;
            }
        }

        private void pnlSpace_MouseMove(object sender, MouseEventArgs e)
        {
            mousePosition = e.Location;
            Vector mouseVector = e.Location.toVector();
            refresh();
            if (mouseDown)
            {
                if (selected)
                {
                    selected.moveTo(mouseVector);
                }
                refresh();
            }
            else
            {
                mousedOver = null;
                mousedOver = checkTrayMouseOver(Managers.Bin, mousedOver, mouseVector);
                mousedOver = checkTrayMouseOver(Managers.Command, mousedOver, mouseVector);
                if (!mousedOver)
                {
                    foreach (GameObject gameObject in gameObjects)
                    {
                        if (gameObject.containsPosition(mouseVector))
                        {
                            mousedOver = gameObject;
                            mousedOver.pickup(mouseVector);
                            break;
                        }
                    }
                }
                if (mousedOver)
                {
                    Cursor.Current = Cursors.SizeAll;
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void pnlSpace_MouseUp(object sender, MouseEventArgs e)
        {
            Vector mouseVector = e.Location.toVector();
            mouseDown = false;
            if (selected)
            {
                if (!(selected is Tray))
                {
                    if (Managers.Bin.containsPosition(mouseVector))
                    {
                        removeGameObject(selected);
                    }
                    else
                    {
                        WayPoint selectedWayPoint = null;
                        foreach (WayPoint wayPoint in wayPoints)
                        {
                            if (wayPoint.containsPosition(mouseVector))
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
                        //Anchoring to other objects
                        GameObject anchorObject = null;
                        foreach (GameObject gameObject in gameObjects)
                        {
                            if (gameObject != selected
                                && gameObject.containsPosition(mouseVector)
                                && gameObject > selected)
                            {
                                anchorObject = gameObject;
                                break;
                            }
                        }
                        if (anchorObject)
                        {
                            if (anchorObject is CardDeck
                                && ((CardDeck)anchorObject).fitsInDeck(selected))
                            {
                                ((CardDeck)anchorObject).acceptCard((CardDeck)selected);
                            }
                            else
                            {
                                selected.anchorTo(anchorObject);
                            }
                        }
                        else
                        {
                            selected.anchorOff();
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
            ObjectImportManager.importObjects(this, filenames);
            refresh();
        }

        private void pnlSpace_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void pnlSpace_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            bool changedObjectState = false;
            Vector mouseVector = e.Location.toVector();
            checkTrayDoubleClick(Managers.Command, mouseVector);
            //Find an object to change its state
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.canChangeState()
                    && gameObject.containsPosition(mouseVector))
                {
                    changedObjectState = true;
                    gameObject.changeState();
                    break;
                }
            }

            if (!changedObjectState
                && WAYPOINTS_ENABLED)
            {
                addWayPoint(new WayPoint(
                    pbxWayPoint.Image,
                    e.Location.toVector(),
                    new Size(100, 100),
                    WayPoint.Shape.CIRCLE
                    ));
            }
            refresh();
        }

        public void addGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
            gameObjects.Sort();
            renderOrder.Add(gameObject);
            renderOrder.Sort();
            renderOrder.Reverse();
        }
        public void removeGameObject(GameObject gameObject)
        {
            gameObjects.Remove(gameObject);
            gameObjects.Sort();
            renderOrder.Remove(gameObject);
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

        private void MainForm_MouseHover(object sender, EventArgs e)
        {
            mouseHover = true;
        }

        GameObject checkTrayMouseOver(Tray tray, GameObject currentMousedOver, Vector mousePos)
        {
            if (currentMousedOver == null)
            {
                if (tray.containsPosition(mousePos))
                {
                    TrayComponent mousedOverComponent = tray.getComponent(mousePos);
                    if (mousedOverComponent)
                    {
                        return mousedOverComponent;
                    }
                    else
                    {
                        return tray;
                    }
                }
            }
            return currentMousedOver;
        }

        void checkTrayDoubleClick(Tray tray, Vector mousePos)
        {
            GameObject currentMousedOver = null;
            if (currentMousedOver == null)
            {
                if (tray.containsPosition(mousePos))
                {
                    TrayComponent mousedOverComponent = tray.getComponent(mousePos);
                    if (mousedOverComponent)
                    {
                        currentMousedOver = mousedOverComponent;
                    }
                    else
                    {
                        currentMousedOver = tray;
                    }
                }
            }
            if (currentMousedOver)
            {
                if (currentMousedOver.canChangeState())
                {
                    currentMousedOver.changeState();
                }
            }
        }
    }
}
