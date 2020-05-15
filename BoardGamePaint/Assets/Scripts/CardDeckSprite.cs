using System;
using System.Drawing;

public class CardDeckSprite : GameObjectSprite
{
    private CardDeck cardDeck { get => (CardDeck)gameObject; }

    int MAX_VISIBLE_CARD_COUNT = 100;
    float CARD_SPACING = 0.5f;

    protected Size outerSize;

    public CardDeckSprite(CardDeck cardDeck) : base(cardDeck)
    {
        outerSize = new Size(size.Width + 25, size.Height + 25);
        if (cardDeck.Face != null)
        {
            this.Face = Image.FromFile(cardDeck.Face);
        }
        this.Back = Image.FromFile(cardDeck.Back);
    }

    public Image Face { get; protected set; }

    public Image Back { get; protected set; }

    public override Image image
    {
        get => Back;
        protected set => Back = value;
    }

    public override void draw(Graphics graphics)
    {
        base.draw(graphics);
        int cardCount = (cardDeck.cards != null) ? cardDeck.cards.Count : 1;
        if (cardCount > 1)
        {
            int limit = Math.Min(MAX_VISIBLE_CARD_COUNT, cardCount);
            for (int i = 1; i < limit; i++)
            {
                graphics.DrawImage(
                image,
                TopLeftScreen.x,
                TopLeftScreen.y - i * CARD_SPACING,
                size.Width,
                size.Height
                );
            }
        }
        drawFooterNumber(graphics);
    }

    public override bool containsPosition(Vector pos)
    {
        return containsPositionInner(pos)
            || containsPositionOuter(pos);
    }
    public bool containsPositionInner(Vector pos)
    {
        float halfWidth = size.Width / 2;
        float halfHeight = size.Height / 2;
        float bonusHeight = getBonusHeight();
        return
            pos.x >= position.x - halfWidth
            && pos.x <= position.x + halfWidth
            && pos.y >= position.y - halfHeight - bonusHeight
            && pos.y <= position.y + halfHeight;
    }

    public bool containsPositionOuter(Vector pos)
    {
        float halfWidthOuter = outerSize.Width / 2;
        float halfHeightOuter = outerSize.Height / 2;
        float bonusHeight = getBonusHeight();
        return
            pos.x >= position.x - halfWidthOuter
            && pos.x <= position.x + halfWidthOuter
            && pos.y >= position.y - halfHeightOuter - bonusHeight
            && pos.y <= position.y + halfHeightOuter;
    }

    public float getBonusHeight()
    {
        int cardCount = (cardDeck.cards != null) ? cardDeck.cards.Count : 1;
        int limit = Math.Min(MAX_VISIBLE_CARD_COUNT, cardCount);
        return limit * CARD_SPACING;
    }

    public override Rectangle getRect()
    {
        Vector pickupPos = getPickupPosition();
        int bonusHeight = (int)getBonusHeight();
        bool outsideOnly = containsPositionOuter(pickupPos)
                       && !containsPositionInner(pickupPos);
        if (outsideOnly)
        {
            return new Rectangle(
            (int)position.x - outerSize.Width / 2,
            (int)position.y - outerSize.Height / 2 - bonusHeight,
            outerSize.Width,
            outerSize.Height + bonusHeight);
        }
        else
        {
            return new Rectangle(
                (int)TopLeft.x,
                (int)TopLeft.y - bonusHeight,
                size.Width,
                size.Height + bonusHeight);
        }
    }

    public override bool canMakeNewObject(Vector mousePos)
    {
        return base.canMakeNewObject(mousePos) && containsPositionInner(mousePos);
    }

    public override object Clone()
    {
        return new CardDeckSprite((CardDeck)this.cardDeck.Clone());
    }
}
