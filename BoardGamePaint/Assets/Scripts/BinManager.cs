using BoardGamePaint;
using System;
using System.Collections.Generic;
using System.Drawing;

public class BinManager : GameObject
{
    readonly List<Bin> bins = new List<Bin>();
    List<Image> imagesToProcess = new List<Image>();

    int binSize = 50;

    Brush backBrush;
    Rectangle backRect;

    public BinManager() : base((Image)null)
    {
        backBrush = new SolidBrush(Color.FromArgb(206, 117, 57));
        position = new Vector(0, 0);
        backRect = new Rectangle(0, 0, binSize * 2, binSize);
    }

    public void addImage(Image image, int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            imagesToProcess.Add(image);
        }
    }

    public void processImages(MainForm mf, Image backImage = null)
    {
        //If there are no images,
        if (imagesToProcess.Count < 1)
        {
            //Don't process any images
            imagesToProcess = new List<Image>();
            return;
        }
        Size firstSize = imagesToProcess[0].Size;
        bool allSameSize = true;
        foreach(Image image in imagesToProcess)
        {
            if (image.Size != firstSize)
            {
                allSameSize = false;
                break;
            }
        }
        if (allSameSize && imagesToProcess.Count > 1)
        {
            //make it all one object
            if (backImage != null)
            {
            }
            else
            {
                //else make it an object with many states
                GameObject gameObject = new GameObject(imagesToProcess);
                gameObject.moveTo(new Vector(100, 100), false);
                mf.addGameObject(gameObject);
            }
        }
        else
        {
            //make them separate objects
            foreach (Image image in imagesToProcess)
            {
                makeBin(image);
            }
        }
        imagesToProcess = new List<Image>();
    }

    private void makeBin(Image image)
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
