using System;

public class PlayerButton : Button
{
    public Player player { get; private set; }

    public PlayerButton(Player player) : base(null)
    {
        this.player = player;
        this.ImageURL = player.imageURL;
    }

    public override void activate()
    {
        Managers.Players.Current = this.player;
    }

    public override string TypeString
    {
        get => (this.player) ? this.player.name : "Neutral Player";
    }
}
