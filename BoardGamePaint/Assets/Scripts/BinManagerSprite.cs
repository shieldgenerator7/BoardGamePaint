using System;

public class BinManagerSprite:TraySprite
{
	public BinManagerSprite(BinManager binManager):base(binManager)
	{
    }

    public BinSprite getBin(Vector mousePos)
    {
        return (BinSprite)getComponent(mousePos);
    }
}
