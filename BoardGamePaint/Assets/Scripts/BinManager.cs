﻿using BoardGamePaint;
using System;
using System.Collections.Generic;
using System.Drawing;

public class BinManager : GameObject
{
    readonly List<Bin> bins = new List<Bin>();

    int binSize = 50;

    Brush backBrush;
    Rectangle backRect;

    public BinManager() : base((Image)null)
    {
        backBrush = new SolidBrush(Color.FromArgb(206, 117, 57));
        position = new Vector(0, 0);
        backRect = new Rectangle(0, 0, binSize * 2, binSize);
    }

    public void makeBin(Image image)
    {
        backRect.Width += binSize;
        Bin newBin = new Bin(image, binSize);
        newBin.moveTo(
            getBinPosition(bins.Count),
            false
            );
        bins.Add(newBin);
    }
    private Vector getBinPosition(int index)
    {
        return new Vector(
                position.x + binSize / 2 + (index + 1) * binSize,
                position.y + binSize / 2
                );
    }

    public Bin getBin(Vector mousePos)
    {
        foreach (Bin bin in bins)
        {
            if (bin.containsPosition(mousePos))
            {
                return bin;
            }
        }
        return null;
    }

    public override void draw(Graphics graphics)
    {
        graphics.FillRectangle(backBrush, backRect);
        foreach (Bin bin in bins)
        {
            bin.draw(graphics);
        }
    }

    public override bool containsPosition(Vector pos)
    {
        return pos.x >= backRect.X && pos.x <= backRect.X + backRect.Width
            && pos.y >= backRect.Y && pos.y <= backRect.Y + backRect.Height;
    }

    public override void moveTo(Vector pos, bool useOffset = true)
    {
        base.moveTo(pos, useOffset);
        backRect.X = (int)position.x;
        backRect.Y = (int)position.y;
        for (int i = 0; i < bins.Count; i++)
        {
            Bin bin = bins[i];
            bin.moveTo(
                getBinPosition(i),
                false
                );
        }
    }

    public override Rectangle getRect()
    {
        return backRect;
    }

    public override bool canChangeState()
    {
        return false;
    }
}
