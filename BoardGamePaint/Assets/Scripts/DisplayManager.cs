using System;
using System.Collections.Generic;
using System.Drawing;

public class DisplayManager
{
    int BRUSH_THICKNESS = 4;

    Pen selectPen;
    Pen deletePen;
    Pen anchorPen;
    Pen changePen;
    Pen createPen;

    Font font;

    public Vector origin = Vector.zero;

    public DisplayManager()
    {
        selectPen = new Pen(Color.FromArgb(155, 155, 155), BRUSH_THICKNESS);
        deletePen = new Pen(Color.FromArgb(247, 70, 70), BRUSH_THICKNESS);
        anchorPen = new Pen(Color.FromArgb(52, 175, 0), BRUSH_THICKNESS);
        changePen = new Pen(Color.FromArgb(137, 206, 255), BRUSH_THICKNESS);
        createPen = new Pen(Color.FromArgb(255, 230, 107), BRUSH_THICKNESS);
        //
        FontFamily fontFamily = new FontFamily("Microsoft Sans Serif");
        font = new Font(
            fontFamily,
            20,
            FontStyle.Regular,
            GraphicsUnit.Pixel
            );
    }

    public void displayObjects(Graphics graphics)
    {
        if (Managers.Object.renderOrder.Count > 0)
        {
            //Make sure smaller objects are drawn on top
            //Draw the objects
            foreach (GameObject gameObject in Managers.Object.renderOrder)
            {
                gameObject.draw(graphics);
            }
        }
        Managers.Bin.draw(graphics);
        Managers.Command.draw(graphics);
    }

    public void displayRectangles(Graphics graphics)
    {
        displayRectangles(
            graphics,
            Managers.Control.selected,
            Managers.Control.mousedOver,
            Managers.Control.mousePos,
            Managers.Control.isMouseDown
            );
    }

    public void displayRectangles(Graphics graphics, GameObject selected, GameObject mousedOver, Vector mousePos, bool mouseDown)
    {
        if (Managers.Players.CurrentButton)
        {
            drawRectangle(graphics, selectPen, Managers.Players.CurrentButton);
        }
        if (!mousedOver)
        {
            mousedOver = selected;
        }
        if (mousedOver)
        {
            if (mouseDown)
            {
                //in the middle of a drag
                if (!(mousedOver is Tray)
                    && !(mousedOver is TrayComponent)
                    && Managers.Bin.containsPosition(mousePos)
                    && mousedOver.Permissions.canEdit)
                {
                    drawRectangle(graphics, deletePen, mousedOver);
                }
                else if (!(mousedOver is Tray)
                    && !(mousedOver is TrayComponent)
                    && Managers.Command.containsPosition(mousePos)
                    && Managers.Command.getComponent(mousePos) is PlayerButton
                    && mousedOver.Permissions.canEdit)
                {
                    drawRectangle(graphics, changePen, mousedOver);
                    drawRectangle(graphics, changePen, Managers.Command.getComponent(mousePos));
                }
                else if (mousedOver is Tray)
                {
                    drawRectangle(graphics, selectPen, mousedOver);
                }
                else
                {
                    //check to see if it's going to anchor on anything
                    GameObject anchorObject = Managers.Object
                        .getAnchorObject(mousedOver, mousePos);
                    if (anchorObject)
                    {
                        if (selected.Permissions.canInteract)
                        {
                            if (anchorObject is CardDeck
                            && ((CardDeck)anchorObject).fitsInDeck(mousedOver))
                            {
                                drawRectangle(graphics, changePen, mousedOver);
                                drawRectangle(graphics, changePen, anchorObject);
                            }
                            else
                            {
                                drawRectangle(graphics, anchorPen, mousedOver);
                                drawRectangle(graphics, anchorPen, anchorObject);
                            }
                        }
                    }
                    else
                    {
                        drawRectangle(graphics, selectPen, mousedOver);
                    }
                }
            }
            else
            {
                //just mousing over things
                if (mousedOver is Bin
                    || (mousedOver.canMakeNewObject(mousePos) && mousedOver.Permissions.canInteract))
                {
                    drawRectangle(graphics, createPen, mousedOver);
                }
                else if (mousedOver.canChangeState() && mousedOver.Permissions.canInteract)
                {
                    drawRectangle(graphics, changePen, mousedOver);
                }
                else
                {
                    drawRectangle(graphics, selectPen, mousedOver);
                }
                //Highlight objects that belong to moused over player button's player
                if (mousedOver is PlayerButton)
                {
                    PlayerButton mousedButton = (PlayerButton)mousedOver;
                    foreach (GameObject go in Managers.Object.renderOrder)
                    {
                        if (go.owner == mousedButton.player)
                        {
                            drawRectangle(graphics, selectPen, go);
                        }
                    }
                }
            }
        }
    }
    private void drawRectangle(Graphics graphics, Pen pen, GameObject gameObject)
    {
        graphics.DrawRectangle(
            pen,
            convertToScreen(gameObject.getRect())
            );
    }

    public void displayDescription(Graphics graphics)
    {
        displayDescription(
            graphics,
            Managers.Control.mousedOver,
            Managers.Control.mousePos
            );
    }

    public void displayDescription(Graphics graphics, GameObject mousedOver, Vector mousePos)
    {
        mousePos = convertToScreen(mousePos);
        //Object Description
        if (mousedOver)
        {
            graphics.DrawString(mousedOver.Description,
                font,
                new SolidBrush(Color.Black),
                mousePos.x - 5,
                mousePos.y - 25
                );
        }
    }

    public Vector convertToScreen(Vector vWorld)
    {
        return vWorld + origin;
    }

    public Vector convertToWorld(Vector vScreen)
    {
        return vScreen - origin;
    }

    public Rectangle convertToScreen(Rectangle rWorld)
        => new Rectangle(
            rWorld.X + (int)origin.x,
            rWorld.Y + (int)origin.y,
            rWorld.Width,
            rWorld.Height
            );

    public Rectangle convertToWorld(Rectangle rScreen)
        => new Rectangle(
            rScreen.X - (int)origin.x,
            rScreen.Y - (int)origin.y,
            rScreen.Width,
            rScreen.Height
            );
}
