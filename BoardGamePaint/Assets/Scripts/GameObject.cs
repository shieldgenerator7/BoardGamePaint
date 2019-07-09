using System;
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
    private Image image;

    //Pickup Runtime Vars
    private Vector pickupOffset = new Vector(0, 0);

    public GameObject(Image image)
    {
        this.position = new Vector(0, 0);
        this.size = image.Size;
        this.image = image;
    }

    public void draw(Graphics graphics)
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

    public void moveTo(Vector pos, bool useOffset = true)
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
