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

    public virtual void draw(Graphics graphics)
    {
        graphics.DrawImage(
            image,
            position.x - size.Width / 2,
            position.y - size.Height / 2,
            size.Width,
            size.Height
            );
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
        return images.Count > 1;
    }

    public void changeState()
    {
        if (images.Count >= 2)
        {
            if (images.Count == 2)
            {
                imageIndex = (imageIndex + 1) % 2;
            }
            else
            {
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

    public static implicit operator Boolean(GameObject gameObject)
    {
        return gameObject != null;
    }

    public int CompareTo(GameObject go)
    {
        return (int)(this.size.toVector().Magnitude - go.size.toVector().Magnitude);
    }

    public static bool operator <(GameObject a, GameObject b)
        => a.size.toVector() < b.size.toVector();

    public static bool operator >(GameObject a, GameObject b)
        => a.size.toVector() > b.size.toVector();
}
