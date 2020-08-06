using System;
using System.Collections.Generic;

public class GameObjectComparer:Comparer<GameObject>
{
	public GameObjectComparer()
	{
	}

    public override int Compare(GameObject x, GameObject y)
    {
        float xSize = ImageBank.getImage(x.ImageURL).Size.toVector().Magnitude;
        float ySize = ImageBank.getImage(y.ImageURL).Size.toVector().Magnitude;
        bool xCardDeck = (x is CardDeck);
        bool yCardDeck = (y is CardDeck);
        return (xSize == ySize)
            ? (xCardDeck && !yCardDeck)
                ? 1
                : (yCardDeck && !xCardDeck)
                    ? -1
                    : 0
            : (int)(xSize - ySize);
    }
}
