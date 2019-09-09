using System;
using System.Drawing;

public class AddPlayerButton : Button
{
    public AddPlayerButton(Image image, int buttonSize) : base(image, buttonSize)
    {
    }

    public override void activate()
    {
        Managers.Players.makeNewPlayer();
    }

    public override string TypeString
    {
        get => "Create New Player";
    }
}
