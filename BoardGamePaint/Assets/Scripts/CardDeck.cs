using BoardGamePaint;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class CardDeck : GameObject
{
    int MAX_VISIBLE_CARD_COUNT = 100;
    float CARD_SPACING = 0.5f;

    readonly List<Card> cards = new List<Card>();

    readonly Random random = new Random();

    protected Size outerSize;

    public CardDeck(List<Card> cards, Image backImage, string description = null) : base(backImage, description)
    {
        this.cards = cards;
        outerSize = new Size(size.Width + 25, size.Height + 25);
        this.Back = backImage;
    }

    public bool FaceUp { get; set; } = false;

    public Image Face { get; protected set; }

    public Image Back { get; protected set; }

    public override Image image
    {
        get => Back;
        protected set => Back = value;
    }

    public override string TypeString
        => "Deck of Cards"
        + ((cards.Count == 0)
            ? " (Empty)"
            : ""
            );

    protected override string getFooterNumberString()
        => "" + cards.Count;

    public override void draw(Graphics graphics)
    {
        base.draw(graphics);
        int cardCount = (cards != null) ? cards.Count : 1;
        if (cardCount > 1)
        {
            int limit = Math.Min(MAX_VISIBLE_CARD_COUNT, cardCount);
            for (int i = 1; i < limit; i++)
            {
                graphics.DrawImage(
                image,
                position.x - size.Width / 2,
                position.y - size.Height / 2 - i * CARD_SPACING,
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
        int cardCount = (cards != null) ? cards.Count : 1;
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
                (int)position.x - size.Width / 2,
                (int)position.y - size.Height / 2 - bonusHeight,
                size.Width,
                size.Height + bonusHeight);
        }
    }    

    public override bool canMakeNewObject(Vector mousePos)
    {
        return cards.Count > 0 && containsPositionInner(mousePos);
    }

    public override GameObject makeNewObject()
    {
        //Draw a card
        int cardIndex = random.Next(0, cards.Count);
        return drawCard(cardIndex);
    }

    protected GameObject drawCard(int cardIndex)
    {
        GameObject newCard = cards[cardIndex];
        newCard.moveTo(position + new Vector(10, 10), false);
        cards.RemoveAt(cardIndex);
        if (cards.Count == 0)
        {
            Managers.Form.removeGameObject(this);
        }
        return newCard;
    }

    public virtual bool fitsInDeck(GameObject other)
        => (other is Card || other is CardDeck)
        && ((CardDeck)other).Back.Size == this.Back.Size
        && other.Size == this.Size;

    public virtual void acceptCard(CardDeck card)
    {
        if (card is Card)
        {
            card.FaceUp = false;
            cards.Add((Card)card);
        }
        else
        {
            cards.AddRange(card.cards);
        }
        Managers.Form.removeGameObject(card);
    }

    public override object Clone()
    {
        List<Card> newCards = new List<Card>(
            from card in this.cards
            select (Card)card.Clone()
            );
        CardDeck newDeck = new CardDeck(newCards, this.Back, this.description);
        return newDeck;
    }
}
