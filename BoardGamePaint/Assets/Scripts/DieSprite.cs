using System;
using System.Collections.Generic;
using System.Drawing;

public class DieSprite : GameObjectSprite
{
    private Die die { get => (Die)gameObject; }

    public override Image image
    {
        get
        {
            try
            {
                return ImageBank.getImage(die.imageURLs[die.imageIndex]);
            }
            catch (ArgumentOutOfRangeException)
            {
                return defaultImage;
            }
        }
    }

    public Image defaultImage => ImageBank.getImage(this.die.defaultImageURL);

    public DieSprite(Die die) : base(die)
    {
        ImageBank.preloadImages(die.imageURLs);
        ImageBank.preloadImages(die.defaultImageURL);
        this.size = this.image.Size;
    }

    public override object Clone()
    {
        return new DieSprite((Die)this.die.Clone());
    }
}
