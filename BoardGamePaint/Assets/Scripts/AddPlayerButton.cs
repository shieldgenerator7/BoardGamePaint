using System;
using System.Drawing;

public class AddPlayerButton: Button
{
	public AddPlayerButton(Image image, int buttonSize) :base(image, buttonSize)
	{
	}

    public override void activate()
    {
        Managers.Players.makeNewPlayer();
        System.Windows.Forms.MessageBox.Show("Player Added! Players: " + Managers.Players.Count);
    }
}
