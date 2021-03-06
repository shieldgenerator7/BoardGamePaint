﻿using System;
using System.Collections.Generic;
using System.Drawing;

public class Die : GameObject
{
    public static string JSON_TYPE = "dice";

    protected List<Image> images;
    protected int imageIndex = 0;
    public override Image image
    {
        get {
            if (imageIndex < 0)
            {
                return defaultImage;
            }
            return images?[imageIndex];
        }
        protected set
        {
            if (images == null)
            {
                images = new List<Image>();
            }
            if (!images.Contains(value))
            {
                if (defaultImage != null)
                {
                    //do nothing,
                    //images.indexOf() will set imageIndex to -1
                }
                else
                {
                    images.Add(value);
                }
            }
            imageIndex = images.IndexOf(value);
        }
    }

    public Image defaultImage { private get; set; }

    public Die(List<Image> images, string description) : base((Image)null, description)
    {
        this.position = new Vector(0, 0);
        this.images = images;
        this.defaultImage = images[0];
        this.imageIndex = -1;
        this.size = this.image.Size;
    }

    public override string TypeString
        => "Die";

    private int stateChangeCount = 0;
    protected override string getFooterNumberString()
        => "" + stateChangeCount;

    public override bool canChangeState()
        => true;

    public override void changeState()
    {
        stateChangeCount++;
        if (images.Count >= 2)
        {
            //Change state
            if (images.Count == 2)
            {
                //Flip
                imageIndex = (imageIndex + 1) % 2;
            }
            else
            {
                //Randomly roll
                Random rand = new Random();
                int newIndex = imageIndex;
                while (newIndex == imageIndex)
                {
                    newIndex = rand.Next(images.Count);
                }
                imageIndex = newIndex;
            }
        }
    }

    public override object Clone()
    {
        Die newDie = new Die(images, this.description);
        newDie.FileName = (string)this.FileName;
        return newDie;
    }
}
