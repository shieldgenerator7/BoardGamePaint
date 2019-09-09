using System;
using System.Drawing;

public class Bin : TrayComponent
{
    private GameObject template;

    public Bin(GameObject gameObject, int binSize) : base(gameObject.image, binSize)
    {
        this.template = gameObject;
    }

    public override bool canMakeNewObject(Vector mousePos)
    {
        return true;
    }

    public override GameObject makeNewObject()
    {
        GameObject clone = (GameObject)template.Clone();
        clone.owner = Managers.Players.Current;
        return clone;
    }

    public override string TypeString
    {
        get => template.Description + " Bin";
    }
}
