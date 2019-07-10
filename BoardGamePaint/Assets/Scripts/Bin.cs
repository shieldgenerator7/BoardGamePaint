using System;
using System.Drawing;

public class Bin : GameObject
{
    public Bin(Image image) : base(image)
    {
        size = new Size(100, 100);
    }

    public GameObject getNewObject()
    {
        return new GameObject(image);
    }
}
