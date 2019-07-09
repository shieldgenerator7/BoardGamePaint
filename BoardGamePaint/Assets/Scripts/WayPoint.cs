using System;
using System.Drawing;

public class WayPoint: GameObject
{
    public enum Shape
    {
        CIRCLE,
        RECTANGLE
    }
    Shape shape;

	public WayPoint(Image image, Vector pos, Size size, Shape shape): base(image)
    {
        this.position = pos;
        this.size = size;
        this.shape = shape;
        //If the shape is a circle,
        if (shape == Shape.CIRCLE)
        {
            //Make both dimensions equal
            size.Width = size.Height = Math.Max(size.Width, size.Height);
        }
    }

    public override bool containsPosition(Vector pos)
    {
        switch (shape)
        {
            case Shape.CIRCLE:
                return (position - pos).Magnitude <= size.Width/2;
            case Shape.RECTANGLE:
                return base.containsPosition(pos);
            default:
                return false;
        }
    }
}
