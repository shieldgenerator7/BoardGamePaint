using System;
using System.Collections.Generic;
using System.Drawing;

public class DieSprite : GameObjectSprite
{
    private Die die { get => (Die)gameObject; }

    private List<Image> images = new List<Image>();

    public override Image image
    {
        get
        {
            try
            {
                return images[die.imageIndex];
            }
            catch (ArgumentOutOfRangeException)
            {
                return defaultImage;
            }
        }
        protected set => defaultImage = value;
    }

    private Image defaultImage;

    public DieSprite(Die die) : base(die)
    {
        foreach (string imageURL in die.imageURLs)
        {
            images.Add(Image.FromFile(imageURL));
        }
        if (die.defaultImageURL != null && die.defaultImageURL != "")
        {
            this.defaultImage = Image.FromFile(die.defaultImageURL);
        }
        else
        {
            this.defaultImage = images[0];
        }
        this.size = this.image.Size;
    }

    public override object Clone()
    {
        return new DieSprite((Die)this.die.Clone());
    }
}
