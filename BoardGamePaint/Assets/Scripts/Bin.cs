using System;
using System.Drawing;

public class Bin : GameObject
{
    public Bin(Image image, int binSize) : base(image)
    {
        size = new Size(binSize, binSize);
    }

    public GameObject makeNewObject()
    {
        return new GameObject(image);
    }
}
