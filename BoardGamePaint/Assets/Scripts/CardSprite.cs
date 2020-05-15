using System;
using System.Drawing;

public class CardSprite : CardDeckSprite
{
	private Card card { get => (Card)gameObject; }

	public CardSprite(Card card):base(card)
	{
		outerSize = Size;
	}

	public override Image image
	{
		get => (card.FaceUp && card.Permissions.canView) ? Face : Back;
	}
}
