using System;
using System.Drawing;

public class GameObject
{
    //Drawing Runtime Vars
    protected Vector position;
    public Vector Position
    {
        get { return position; }
    }
    protected Size size;
    private Image image;

    //Pickup Runtime Vars
    private Vector pickupOffset;

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

    public void moveTo(Vector pos)
    {
        position = pos + pickupOffset;
    }

    public static implicit operator Boolean(GameObject gameObject)
    {
        return gameObject != null;
    }

    public static bool operator <(GameObject a, GameObject b)
        => a.size.toVector() < b.size.toVector();

    public static bool operator >(GameObject a, GameObject b)
        => a.size.toVector() > b.size.toVector();
}
