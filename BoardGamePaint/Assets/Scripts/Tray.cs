using System;
using System.Collections.Generic;
using System.Drawing;

public class Tray : GameObject
{
    readonly List<TrayComponent> trayComponents = new List<TrayComponent>();

    protected int componentSize = 50;

    Brush backBrush;
    Rectangle backRect;

    public Tray() : base((Image)null)
    {
        backBrush = new SolidBrush(Color.FromArgb(206, 117, 57));
        position = new Vector(0, 0);
        backRect = new Rectangle(0, 0, componentSize * 2, componentSize);
    }

    protected void addComponent(TrayComponent tc)
    {
        backRect.Width += componentSize;
        tc.moveTo(
            getComponentPosition(trayComponents.Count),
            false
            );
        trayComponents.Add(tc);
    }

    private Vector getComponentPosition(int index)
        => new Vector(
            position.x + componentSize / 2 + (index + 1) * componentSize,
            position.y + componentSize / 2
            );

    public TrayComponent getComponent(Vector mousePos)
    {
        foreach (TrayComponent tc in trayComponents)
        {
            if (tc.containsPosition(mousePos))
            {
                return tc;
            }
        }
        return null;
    }

    public override void draw(Graphics graphics)
    {
        graphics.FillRectangle(backBrush, backRect);
        foreach (TrayComponent tc in trayComponents)
        {
            tc.draw(graphics);
        }
    }

    public override bool containsPosition(Vector pos)
    {
        return pos.x >= backRect.X && pos.x <= backRect.X + backRect.Width
            && pos.y >= backRect.Y && pos.y <= backRect.Y + backRect.Height;
    }

    public override void moveTo(Vector pos, bool useOffset = true)
    {
        base.moveTo(pos, useOffset);
        backRect.X = (int)position.x;
        backRect.Y = (int)position.y;
        for (int i = 0; i < trayComponents.Count; i++)
        {
            TrayComponent tc = trayComponents[i];
            tc.moveTo(
                getComponentPosition(i),
                false
                );
        }
    }

    public override Rectangle getRect()
        => backRect;
}
