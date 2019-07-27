using System;
using System.Drawing;

public class Bin : GameObject
{
    private GameObject template;

    public Bin(GameObject gameObject, int binSize) : base(gameObject.image)
    {
        this.template = gameObject;
        size = new Size(binSize, binSize);
    }

    public override bool canMakeNewObject(Vector mousePos)
    {
        return true;
    }

    public override GameObject makeNewObject()
    {
        return (GameObject)template.Clone();
    }

    public override string getTypeString()
    {
        return template.Description + " Bin";
    }
}
