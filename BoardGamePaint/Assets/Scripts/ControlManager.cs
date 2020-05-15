using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class ControlManager
{
    //bool WAYPOINTS_ENABLED = false;

    public bool isMouseDown { get; private set; } = false;
    public GameObjectSprite selected { get; private set; } = null;
    public GameObjectSprite mousedOver { get; private set; } = null;
    public Vector mousePos { get; private set; } = null;
    public bool isMouseHover { get; private set; } = false;
    public Vector origMousePos { get; private set; } = null;

    public ControlManager()
    {
    }

    public void mouseDown()
    {
        this.isMouseDown = true;
        this.isMouseHover = false;
        origMousePos = mousePos;
        selected = mousedOver;
        selected?.pickup(mousePos);
        if (selected && selected.canMakeNewObject(mousePos) && selected.gameObject.Permissions.canInteract)
        {
            selected = Managers.Object.addGameObject(selected.gameObject.makeNewObject());
            selected.moveTo(mousePos, false);
            selected.pickup(mousePos);
            mousedOver = selected;
        }
        if (selected && selected is ButtonSprite)
        {
            ((Button)selected.gameObject).activate();
            selected = null;
            mousedOver = null;
        }
    }

    public bool mouseMove(Vector mousePosWorld)
    {
        this.mousePos = mousePosWorld;
        if (isMouseDown)
        {
            if (selected && selected.gameObject.Permissions.canMove)
            {
                selected.moveTo(mousePosWorld);
                if (!(selected is TraySprite)
                    && !(selected is TrayComponentSprite))
                {
                    GameObjectSprite gos = Managers.Object.getSnapObject(selected);
                    if (gos)
                    {
                        selected.snapTo(gos);
                    }
                }
            }
            return true;
        }
        else
        {
            GameObjectSprite prevMousedOver = mousedOver;
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
            if (!(selected is TraySprite))
            {
                if (Managers.Bin.containsPosition(mousePos) && selected.gameObject.Permissions.canEdit)
                {
                    Managers.Object.removeGameObject(selected.gameObject);
                }
                else if (Managers.Command.containsPosition(mousePos)
                    && Managers.Command.getComponent(mousePos) is PlayerButtonSprite
                    && selected.gameObject.Permissions.canEdit)
                {
                    selected.gameObject.owner = ((PlayerButton)Managers.Command.getComponent(mousePos).gameObject).player;
                    //Move object back to where it was before
                    selected.moveTo(origMousePos);
                }
                else
                {
                    //WayPoint selectedWayPoint = Managers.Object
                    //    .getAnchorWayPoint(selected, mousePos);
                    //if (selectedWayPoint)
                    //{
                    //    selected.moveTo(selectedWayPoint.Position, false);
                    //}

                    //Anchoring to other objects
                    GameObjectSprite anchorObject = Managers.Object
                        .getAnchorObject(selected, mousePos);
                    if (anchorObject)
                    {
                        if (selected.gameObject.Permissions.canInteract)
                        {
                            if (anchorObject is CardDeckSprite
                                && ((CardDeck)anchorObject.gameObject).fitsInDeck(selected.gameObject))
                            {
                                CardDeckSprite parent = (CardDeckSprite)Managers.Object.getSprite(
                                    ((CardDeck)anchorObject.gameObject).acceptCard((CardDeck)selected.gameObject)
                                    );
                                if (parent != anchorObject)
                                {
                                    parent.moveTo(anchorObject.Position, false);
                                }
                                if (parent != selected)
                                {
                                    selected = parent;
                                }
                            }
                            else
                            {
                                selected.gameObject.anchorTo(anchorObject.gameObject);
                            }
                        }
                    }
                    else
                    {
                        selected.gameObject.anchorOff();
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
        //bool changedObjectState = false;
        //checkTrayDoubleClick(Managers.Command, mousePos);

        //Find an object to change its state
        GameObjectSprite gameObjectSprite = Managers.Object.getObjectAtPosition(mousePos);
        if (gameObjectSprite && gameObjectSprite.gameObject.canChangeState() && gameObjectSprite.gameObject.Permissions.canInteract)
        {
            //changedObjectState = true;
            gameObjectSprite.gameObject.changeState();
        }

        //if (!changedObjectState
        //    && WAYPOINTS_ENABLED)
        //{
        //    Managers.Object.addWayPoint(new WayPoint(
        //        ImageUtility.getImage("waypoint"),
        //        mousePos,
        //        new Size(100, 100),
        //        WayPoint.Shape.CIRCLE
        //        ));
        //}
    }

    GameObjectSprite checkTrayMouseOver(TraySprite traySprite, GameObjectSprite currentMousedOver, Vector mousePos)
    {
        if (currentMousedOver == null)
        {
            if (traySprite.containsPosition(mousePos))
            {
                TrayComponentSprite mousedOverComponent = traySprite.getComponent(mousePos);
                if (mousedOverComponent)
                {
                    return mousedOverComponent;
                }
                else
                {
                    return traySprite;
                }
            }
        }
        return currentMousedOver;
    }

    void checkTrayDoubleClick(TraySprite traySprite, Vector mousePos)
    {
        GameObjectSprite currentMousedOver = null;
        if (currentMousedOver == null)
        {
            if (traySprite.containsPosition(mousePos))
            {
                TrayComponentSprite mousedOverComponent = traySprite.getComponent(mousePos);
                if (mousedOverComponent)
                {
                    currentMousedOver = mousedOverComponent;
                }
                else
                {
                    currentMousedOver = traySprite;
                }
            }
        }
        if (currentMousedOver)
        {
            if (currentMousedOver.gameObject.canChangeState())
            {
                currentMousedOver.gameObject.changeState();
            }
        }
    }
}
