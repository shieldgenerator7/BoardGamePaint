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
        this.size = new Size(100, 100);
        this.image = image;
    }

    public void draw(Graphics graphics)
    {
        graphics.DrawImage(image, position.x, position.y, size.Width, size.Height);
    }

    public virtual bool containsPosition(Vector pos)
    {
        return pos.x >= position.x && pos.x <= position.x + size.Width
            && pos.y >= position.y && pos.y <= position.y + size.Height;
    }

    public void pickup(Vector pickupPos)
    {
        pickupOffset = position - pickupPos;
    }

    public void moveTo(Vector pos)
    {
        position = pos + pickupOffset;
    }

    public static implicit operator Boolean (GameObject gameObject)
    {
        return gameObject != null;
    }
}
