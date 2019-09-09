using System;
using System.Drawing;

public class Player
{
    public Color color { get; private set; }

    public Player(Color color)
    {
        this.color = color;
    }

    public static implicit operator Boolean (Player player)
        => player != null;
}
