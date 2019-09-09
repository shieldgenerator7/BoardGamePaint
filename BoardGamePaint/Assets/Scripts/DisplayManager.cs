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
        if (Managers.Players.Current)
        {
            graphics.DrawRectangle(selectPen, Managers.Players.CurrentButton.getRect());
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
                    && Managers.Bin.containsPosition(mousePos))
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
                    GameObject anchorObject = Managers.Object
                        .getAnchorObject(mousedOver, mousePos);
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
                if (mousedOver is Bin || mousedOver.canMakeNewObject(mousePos))
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
                if (mousedOver is PlayerButton)
                {
                    PlayerButton mousedButton = (PlayerButton)mousedOver;
                    foreach (GameObject go in Managers.Object.renderOrder)
                    {
                        if (go.owner == mousedButton.player)
                        {
                            graphics.DrawRectangle(selectPen, go.getRect());
                        }
                    }
                }
            }
        }
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
}
