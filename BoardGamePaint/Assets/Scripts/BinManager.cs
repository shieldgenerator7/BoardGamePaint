using System;
using System.Collections.Generic;
using System.Drawing;

public class BinManager : GameObject
{
    readonly List<Bin> bins = new List<Bin>();

    Brush backBrush;
    Rectangle backRect;

    public BinManager() : base(null)
    {
        backBrush = new SolidBrush(Color.FromArgb(206, 117, 57));
        backRect = new Rectangle(0, 0, 0, 100);
    }

    public void makeBin(Image image)
    {
        backRect.Width += 100;
        Bin newBin = new Bin(image);
        newBin.moveTo(
            new Vector(50 + bins.Count * 100, 50),
            false
            );
        bins.Add(newBin);
    }

    public override void draw(Graphics graphics)
    {
        graphics.FillRectangle(backBrush, backRect);
        foreach (Bin bin in bins)
        {
            bin.draw(graphics);
        }
    }
}
