
using System;

public class BinManager : Tray
{
    public BinManager() : base() { }

    public override string TypeString => "Bin Tray";

    public void makeBin(GameObject gameObject)
    {
        Bin newBin = new Bin(gameObject);
        addComponent(newBin);
    }
}
