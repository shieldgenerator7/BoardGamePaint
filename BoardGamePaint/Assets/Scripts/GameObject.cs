using System;
using System.Drawing;

public class GameObject
{
    private Rectangle rect;
    private Image image;

    public GameObject(Image image)
	{
        this.rect = new Rectangle(0, 0, 100, 100);
        this.image = image;
	}

    public void draw(Graphics graphics)
    {
        graphics.DrawImage(image, rect);
    }

    public void moveRight()
    {
        rect.X += 10;
    }

    public void moveTo(Point pos)
    {
        rect.X = pos.X;
        rect.Y = pos.Y;
    }
}
