using System;
using System.Drawing;

public class TrayComponent : GameObject
{
    public TrayComponent(Image image, int componentSize):base(image)
    {
        size = new Size(componentSize, componentSize);
    }
}
