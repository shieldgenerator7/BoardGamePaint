using System;

public class AddPlayerButton : Button
{
    public AddPlayerButton(string imageURL) : base(imageURL)
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
