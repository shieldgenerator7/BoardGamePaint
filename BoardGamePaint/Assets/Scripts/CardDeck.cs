
using System;
using System.Collections.Generic;
using System.Linq;

public class CardDeck : GameObject
{
    public static string JSON_TYPE = "deck";

    public readonly List<Card> cards = new List<Card>();

    readonly Random random = new Random();

    public CardDeck(List<Card> cards, string backImageURL, string description = null) : base(backImageURL, description)
    {
        this.cards = cards;
        this.Back = backImageURL;
    }

    public bool FaceUp { get; set; } = false;

    public string Face { get; protected set; }

    public string Back { get; protected set; }

    public override string ImageURL
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

    public override string getFooterNumberString()
        => "" + cards.Count;

    public override bool canMakeNewObject()
    {
        return cards.Count > 0;
    }

    public override GameObject makeNewObject()
    {
        //Draw a card
        int cardIndex = random.Next(0, cards.Count);
        Card drawnCard = drawCard(cardIndex);
        drawnCard.owner = Managers.Players.Current;
        return drawnCard;
    }

    protected Card drawCard(int cardIndex)
    {
        Card newCard = cards[cardIndex];
        cards.RemoveAt(cardIndex);
        if (cards.Count == 0)
        {
            Managers.Object.removeGameObject(this);
        }
        return newCard;
    }

    public virtual bool fitsInDeck(GameObject other)
        => (other is Card || other is CardDeck)
        && ((CardDeck)other).Back == this.Back;

    public virtual CardDeck acceptCard(CardDeck card)
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
        Managers.Object.removeGameObject(card);
        return this;
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
