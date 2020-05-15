using System;
using System.Windows.Forms;

public class ExitButton : Button
{
    public ExitButton(string imageURL) : base(imageURL)
    {
    }

    public override void activate()
    {
        Application.Exit();
    }

    public override string TypeString
    {
        get => "Exit Board Game Paint";
    }
}
