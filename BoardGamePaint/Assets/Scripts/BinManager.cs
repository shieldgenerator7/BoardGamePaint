using BoardGamePaint;
using System;
using System.Collections.Generic;
using System.Drawing;

public class BinManager : Tray
{
    public BinManager() : base() { }

    public override string TypeString => "Bin Tray";

    public void makeBin(GameObject gameObject)
    {
        Bin newBin = new Bin(gameObject, componentSize);
        addComponent(newBin);
    }

    public Bin getBin(Vector mousePos)
    {
        return (Bin)getComponent(mousePos);
    }
}
