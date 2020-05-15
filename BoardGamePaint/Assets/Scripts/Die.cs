using System;
using System.Collections.Generic;
using System.Windows.Forms;

public class Die : GameObject
{
    public static string JSON_TYPE = "dice";

    public List<string> imageURLs { get; private set; }
    public int imageIndex { get; private set; } = 0;
    public override string ImageURL
    {
        get
        {
            if (imageIndex < 0)
            {
                return defaultImageURL;
            }
            return imageURLs?[imageIndex];
        }
        protected set
        {
            if (imageURLs == null)
            {
                imageURLs = new List<string>();
            }
            if (!imageURLs.Contains(value))
            {
                if (defaultImageURL != null)
                {
                    //do nothing,
                    //images.indexOf() will set imageIndex to -1
                }
                else
                {
                    imageURLs.Add(value);
                }
            }
            imageIndex = imageURLs.IndexOf(value);
        }
    }

    public string defaultImageURL { get; private set; }

    public Die(List<string> imageURLs, string description, string defaultImageURL = null) : base(null, description)
    {
        this.imageURLs = imageURLs;
        if (defaultImageURL != null && defaultImageURL != "")
        {
            this.defaultImageURL = defaultImageURL;
        }
        else
        {
            this.defaultImageURL = imageURLs[0];
        }
        this.imageIndex = -1;
    }

    public override string TypeString
        => "Die";

    private int stateChangeCount = 0;
    public override string getFooterNumberString()
        => "" + stateChangeCount;

    public override bool canChangeState()
        => true;

    public override void changeState()
    {
        stateChangeCount++;
        if (imageURLs.Count >= 2)
        {
            //Change state
            if (imageURLs.Count == 2)
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
                    newIndex = rand.Next(imageURLs.Count);
                }
                imageIndex = newIndex;
            }
        }
    }

    public override object Clone()
    {
        Die newDie = new Die(imageURLs, this.description);
        newDie.imageIndex = this.imageIndex;
        return newDie;
    }
}
