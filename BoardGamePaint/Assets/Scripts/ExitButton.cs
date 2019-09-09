using System;
using System.Drawing;
using System.Windows.Forms;

public class ExitButton : Button
{
    public ExitButton(Image image, int buttonSize) : base(image, buttonSize)
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
