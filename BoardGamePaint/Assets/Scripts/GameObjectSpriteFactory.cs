using System;
using System.CodeDom;

public static class GameObjectSpriteFactory
{
	public static GameObjectSprite makeSprite(GameObject go)
	{
		switch (go.GetType().Name)
		{
			case "GameObject":
				return new GameObjectSprite(go);
			case "Die":
				return new DieSprite((Die)go);
			case "CardDeck":
				return new CardDeckSprite((CardDeck)go);
			case "Card":
				return new CardSprite((Card)go);
			//case "WayPoint":
			//	return new WayPointSprite((WayPoint)go, Vector.zero, new System.Drawing.Size(10,10), WayPointSprite.Shape.CIRCLE);
			case "Tray":
				return new TraySprite((Tray)go);
			case "TrayComponent":
				return new TrayComponentSprite((TrayComponent)go, TraySprite.DEFAULT_COMPONENT_SIZE);
			case "Bin":
				return new BinSprite((Bin)go, TraySprite.DEFAULT_COMPONENT_SIZE);
			case "BinManager":
				return new BinManagerSprite((BinManager)go);
			case "Button":
				return new ButtonSprite((Button)go, TraySprite.DEFAULT_COMPONENT_SIZE);
			case "PlayerButton":
				return new PlayerButtonSprite((PlayerButton)go, TraySprite.DEFAULT_COMPONENT_SIZE);
			case "AddPlayerButton":
				return new AddPlayerButtonSprite((AddPlayerButton)go, TraySprite.DEFAULT_COMPONENT_SIZE);
			case "ExitButton":
				return new ExitButtonSprite((ExitButton)go, TraySprite.DEFAULT_COMPONENT_SIZE);
			default:
				throw new NotSupportedException("GameObject type " + go.GetType().Name + " is not yet supported or is not a GameObject subtype.");
		}
	}
}
