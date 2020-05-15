using System;
using System.Drawing;

public class TraySprite : GameObjectSprite
{
    private Tray tray { get => (Tray)gameObject; }

    public const int DEFAULT_COMPONENT_SIZE = 50;

    protected int componentSize = DEFAULT_COMPONENT_SIZE;

    Brush backBrush;
    Rectangle backRect;

    public TraySprite(Tray tray) : base(tray)
    {
        backBrush = new SolidBrush(Color.FromArgb(206, 117, 57));
        position = new Vector(0, 0);
        backRect = new Rectangle(0, 0, componentSize * 2, componentSize);
    }

    public void placeComponents()
    {
        backRect = new Rectangle(0, 0, componentSize * 2, componentSize);
        foreach (TrayComponent tc in tray.trayComponents)
        {
            backRect.Width += componentSize;
        }
        moveTo(position, false);
    }

    private Vector getComponentPosition(int index)
        => new Vector(
            position.x + componentSize / 2 + (index + 1) * componentSize,
            position.y + componentSize / 2
            );

    public TrayComponentSprite getComponent(Vector mousePos)
    {
        foreach (TrayComponent tc in tray.trayComponents)
        {
            TrayComponentSprite tcs = (TrayComponentSprite)Managers.Object.getSprite(tc);
            if (tcs.containsPosition(mousePos))
            {
                return tcs;
            }
        }
        return null;
    }

    public override void draw(Graphics graphics)
    {
        graphics.FillRectangle(
            backBrush,
            Managers.Display.convertToScreen(backRect)
            );
        foreach (TrayComponent tc in tray.trayComponents)
        {
            TrayComponentSprite tcs = (TrayComponentSprite)Managers.Object.getSprite(tc);
            tcs.draw(graphics);
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
        for (int i = 0; i < tray.trayComponents.Count; i++)
        {
            TrayComponent tc = tray.trayComponents[i];
            TrayComponentSprite tcs = (TrayComponentSprite)Managers.Object.getSprite(tc);
            tcs.moveTo(
                getComponentPosition(i),
                false
                );
        }
    }

    public override Rectangle getRect()
        => backRect;
}
