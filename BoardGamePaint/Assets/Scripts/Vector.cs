using System;
using System.Drawing;

public class Vector
{
    public float x;
    public float y;

	public Vector(float x, float y)
	{
        this.x = x;
        this.y = y;
	}

    public Vector(Point point) : this(point.X, point.Y)
    {
        //call other constructor
        //and do nothing
    }

    public static Vector operator -(Vector a)
        => new Vector(-a.x, -a.y);

    public static Vector operator +(Vector a, Vector b)
        => new Vector(a.x + b.x, a.y + b.y);

    public static Vector operator -(Vector a, Vector b)
        => new Vector(a.x - b.x, a.y - b.y);
}
