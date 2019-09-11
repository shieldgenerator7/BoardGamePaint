using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class ControlManager
{
    bool WAYPOINTS_ENABLED = false;

    public bool isMouseDown { get; private set; } = false;
    public GameObject selected { get; private set; } = null;
    public GameObject mousedOver { get; private set; } = null;
    public Vector mousePos { get; private set; } = null;
    public bool isMouseHover { get; private set; } = false;

    public ControlManager()
    {
    }

    public void mouseDown()
    {
        this.isMouseDown = true;
        this.isMouseHover = false;
        selected = mousedOver;
        selected?.pickup(mousePos);
        if (selected && selected.canMakeNewObject(mousePos) && selected.Permissions.canInteract)
        {
            selected = selected.makeNewObject();
            Managers.Object.addGameObject(selected);
            selected.moveTo(mousePos, false);
            selected.pickup(mousePos);
            mousedOver = selected;
        }
        if (selected && selected is Button)
        {
            ((Button)selected).activate();
            selected = null;
            mousedOver = null;
        }
    }

    public bool mouseMove(Vector mousePosWorld)
    {
        this.mousePos = mousePosWorld;
        if (isMouseDown)
        {
            if (selected && selected.Permissions.canMove)
            {
                selected.moveTo(mousePosWorld);
                if (!(selected is Tray)
                    && !(selected is TrayComponent))
                {
                    GameObject go = Managers.Object.getSnapObject(selected);
                    if (go)
                    {
                        selected.snapTo(go);
                    }
                }
            }
            return true;
        }
        else
        {
            GameObject prevMousedOver = mousedOver;
            mousedOver = null;
            mousedOver = checkTrayMouseOver(Managers.Bin, mousedOver, mousePosWorld);
            mousedOver = checkTrayMouseOver(Managers.Command, mousedOver, mousePosWorld);
            if (!mousedOver)
            {
                mousedOver = Managers.Object.getObjectAtPosition(mousePosWorld);
                mousedOver?.pickup(mousePosWorld);
            }
            Cursor neededCursor = Cursor.Current;
            if (mousedOver)
            {
                neededCursor = Cursors.SizeAll;
            }
            else
            {
                neededCursor = Cursors.Default;
            }
            if (Cursor.Current != neededCursor)
            {
                Cursor.Current = neededCursor;
            }
            if (prevMousedOver != mousedOver)
            {
                return true;
            }
        }
        return false;
    }

    public void mouseUp()
    {
        this.isMouseDown = false;
        if (selected)
        {
            if (!(selected is Tray))
            {
                if (Managers.Bin.containsPosition(mousePos) && selected.Permissions.canEdit)
                {
                    Managers.Object.removeGameObject(selected);
                }
                else
                {
                    WayPoint selectedWayPoint = Managers.Object
                        .getAnchorWayPoint(selected, mousePos);
                    if (selectedWayPoint)
                    {
                        selected.moveTo(selectedWayPoint.Position, false);
                    }

                    //Anchoring to other objects
                    GameObject anchorObject = Managers.Object
                        .getAnchorObject(selected, mousePos);
                    if (anchorObject)
                    {
                        if (selected.Permissions.canInteract)
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
                    }
                    else
                    {
                        selected.anchorOff();
                    }
                }
            }
        }
        selected = null;
    }

    public void mouseHover()
    {
        this.isMouseHover = true;
    }

    public void mouseDoubleClick()
    {
        bool changedObjectState = false;
        //checkTrayDoubleClick(Managers.Command, mousePos);

        //Find an object to change its state
        GameObject gameObject = Managers.Object.getObjectAtPosition(mousePos);
        if (gameObject && gameObject.canChangeState() && gameObject.Permissions.canInteract)
        {
            changedObjectState = true;
            gameObject.changeState();
        }

        if (!changedObjectState
            && WAYPOINTS_ENABLED)
        {
            Managers.Object.addWayPoint(new WayPoint(
                ImageUtility.getImage("waypoint"),
                mousePos,
                new Size(100, 100),
                WayPoint.Shape.CIRCLE
                ));
        }
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
