using System;

public class BinManagerSprite:TraySprite
{
	public BinManagerSprite(BinManager binManager):base(binManager)
	{
    }

    public Bin getBin(Vector mousePos)
    {
        return (Bin)getComponent(mousePos);
    }
}
