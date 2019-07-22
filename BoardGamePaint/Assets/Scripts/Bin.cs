using System;
using System.Drawing;

public class Bin : GameObject
{
    public Bin(GameObject gameObject, int binSize) : base(gameObject.image)
    {
        size = new Size(binSize, binSize);
    }

    public override bool canMakeNewObject(Vector mousePos)
    {
        return true;
    }

    public override GameObject makeNewObject()
    {
        return new GameObject(image);
    }

    public override string getTypeString()
    {
        return "Piece"+" Bin";
    }
}
