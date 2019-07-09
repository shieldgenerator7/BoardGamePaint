using System;
using System.Drawing;

public static class Utility
{
    public static Vector toVector(this Point point)
    {
        return new Vector(point);
    }

    public static Vector toVector(this Size size)
    {
        return new Vector(size);
    }
}
