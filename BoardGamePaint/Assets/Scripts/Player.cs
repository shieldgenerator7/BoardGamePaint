using System;
using System.Drawing;

public class Player
{
    public Color color { get; private set; }
    public string name { get; private set; }
    public string imageURL { get; set; }

    public Player(Color color, string imageURL)
    {
        this.color = color;
        this.name = "Player " + this.color.Name;
        this.imageURL = imageURL;
    }

    public static implicit operator Boolean(Player player)
        => player != null;
}
