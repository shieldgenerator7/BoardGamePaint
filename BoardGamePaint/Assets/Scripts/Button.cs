using System;

public abstract class Button : TrayComponent
{

    public Button(string imageURL) : base(imageURL)
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
