using System;
using System.Collections.Generic;
using System.Drawing;

public class GameObject : IComparable<GameObject>
{
    //Drawing Runtime Vars
    protected Vector position;
    public Vector Position
    {
        get { return position; }
    }
    protected Size size;
    public Size Size
    {
        get { return new Size(size.Width, size.Height); }
    }
    protected List<Image> images;
    protected int imageIndex = 0;
    protected Image image
    {
        get { return images?[imageIndex]; }
        set
        {
            if (images == null)
            {
                images = new List<Image>();
            }
            if (!images.Contains(value))
            {
                images.Add(value);
            }
            imageIndex = images.IndexOf(value);
        }
    }

    bool isDeckOfCards = false;

    //Pickup Runtime Vars
    private Vector pickupOffset = new Vector(0, 0);

    public GameObject(Image image)
    {
        this.position = new Vector(0, 0);
        if (image != null)
        {
            this.size = image.Size;
            this.image = image;
        }
    }
    public GameObject(List<Image> images)
    {
        this.position = new Vector(0, 0);
        this.images = images;
        this.imageIndex = 0;
        this.size = this.image.Size;
    }
    /// <summary>
    /// Turns this multi-sprite game object into a deck of cards
    /// </summary>
    /// <param name="images"></param>
    /// <param name="backImage"></param>
    public GameObject(List<Image> images, Image backImage) : this(images)
    {
        this.images.Insert(0, backImage);
        this.isDeckOfCards = true;
    }

    public virtual void draw(Graphics graphics)
    {
        graphics.DrawImage(
            image,
            position.x - size.Width / 2,
            position.y - size.Height / 2,
            size.Width,
            size.Height
            );
        if (isDeckOfCards && images.Count > 1)
        {
            int count = 10;
            int spacing = 2;
            int limit = Math.Min(count, images.Count);
            for (int i = 1; i < limit; i++)
            {
                graphics.DrawImage(
                image,
                position.x - size.Width / 2,
                position.y - size.Height / 2 - i * spacing,
                size.Width,
                size.Height
                );
            }
        }
    }

    public virtual bool containsPosition(Vector pos)
    {
        float halfWidth = size.Width / 2;
        float halfHeight = size.Height / 2;
        return pos.x >= position.x - halfWidth && pos.x <= position.x + halfWidth
            && pos.y >= position.y - halfHeight && pos.y <= position.y + halfHeight;
    }

    public void pickup(Vector pickupPos)
    {
        pickupOffset = position - pickupPos;
    }

    public virtual void moveTo(Vector pos, bool useOffset = true)
    {
        if (useOffset)
        {
            position = pos + pickupOffset;
        }
        else
        {
            position = pos;
        }
    }

    public bool canChangeState()
    {
        return images.Count > 1 || isDeckOfCards;
    }

    public GameObject changeState()
    {
        if (images.Count >= 2)
        {
            if (isDeckOfCards)
            {
                //Draw a card
                Random rand = new Random();
                int cardIndex = rand.Next(1, images.Count);
                return drawCard(cardIndex);
            }
            else
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
        return null;
    }

    protected GameObject drawCard(int cardIndex)
    {
        Image cardImage = images[cardIndex];
        GameObject newCard = new GameObject(
            new List<Image>(
                new Image[] { images[0], images[cardIndex] }
                )
            );
        newCard.moveTo(position + new Vector(10, 10), false);
        images.RemoveAt(cardIndex);
        return newCard;
    }

    public virtual Rectangle getRect()
    {
        return new Rectangle((int)position.x - size.Width / 2, (int)position.y - size.Height / 2, size.Width, size.Height);
    }

    public static implicit operator Boolean(GameObject gameObject)
    {
        return gameObject != null;
    }

    public int CompareTo(GameObject go)
    {
        float thisSize = this.size.toVector().Magnitude;
        float goSize = go.size.toVector().Magnitude;
        return (thisSize == goSize)
            ? (this.isDeckOfCards)
                ?1
                :(go.isDeckOfCards)
                    ?-1
                    :0
            :(int)(this.size.toVector().Magnitude - go.size.toVector().Magnitude);
    }

    public static bool operator <(GameObject a, GameObject b)
    {
        float aSize = a.size.toVector().Magnitude;
        float bSize = b.size.toVector().Magnitude;
        return (aSize == bSize)
            ? b.isDeckOfCards
            : aSize < bSize;
    }

    public static bool operator >(GameObject a, GameObject b)
    {
        float aSize = a.size.toVector().Magnitude;
        float bSize = b.size.toVector().Magnitude;
        return (aSize == bSize)
            ? a.isDeckOfCards
            : aSize > bSize;
    }
}
