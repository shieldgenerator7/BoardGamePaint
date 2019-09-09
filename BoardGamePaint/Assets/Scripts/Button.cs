using System;
using System.Drawing;

public abstract class Button : TrayComponent
{

    public Button(Image image, int buttonSize) : base(image, buttonSize)
    {
    }

    public override bool canChangeState()
        => true;

    public override void changeState()
    {
        activate();
    }

    public abstract void activate();
}
