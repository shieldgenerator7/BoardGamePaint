using System;
using System.Drawing;

public class TrayComponentSprite : GameObjectSprite
{
    public TrayComponentSprite(TrayComponent trayComponent, int componentSize) : base(trayComponent)
    {
        size = new Size(componentSize, componentSize);
    }
}
