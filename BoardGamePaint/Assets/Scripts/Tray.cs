using System;
using System.Collections.Generic;

public class Tray : GameObject
{

    public readonly List<TrayComponent> trayComponents = new List<TrayComponent>();

    public Tray() : base(null)
    {
    }

    public void addComponent(TrayComponent tc)
    {        
        trayComponents.Add(tc);
    }

    public void removeComponent(TrayComponent tc)
    {
        trayComponents.Remove(tc);
    }

}
