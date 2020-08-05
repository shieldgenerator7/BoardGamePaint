
using System.Collections.Generic;

public class Card : CardDeck
{
    public Card(string frontImageURL, string backImageURL, string description = null)
        : base(null, backImageURL, description)
    {
        Face = frontImageURL;
        Back = backImageURL;
        Permissions.interactPermission = Permissions.Permission.OWNING_PLAYER_ONLY;
        Permissions.viewPermission = Permissions.Permission.OWNING_PLAYER_ONLY;
    }

    public Card(GameObject cardToBe, string backImageURL) :
        this(cardToBe.ImageURL, backImageURL, cardToBe.Description)
    { }

    public override string ImageURL
    {
        get => (FaceUp && Permissions.canView) ? Face : Back;
    }

    public override string Description
    {
        get => (FaceUp && Permissions.canView) ? description : TypeString;
    }

    public override string TypeString
    {
        get => "Card";
    }

    public override string getFooterNumberString()
    {
        return null;
    }

    public override bool canChangeState()
    {
        return true;
    }

    public override void changeState()
    {
        //Flip
        FaceUp = !FaceUp;
    }

    public override bool canMakeNewObject()
    {
        return false;
    }

    public override CardDeck acceptCard(CardDeck cardDeck)
    {
        if (cardDeck is Card)
        {
            Card card = (Card)cardDeck;
            CardDeck newParent = new CardDeck(
                new List<Card>() { this, card },
                card.Back,
                //2019-07-25: TODO: instead of null, pass in something from CardDeckData
                null
                );
            card.FaceUp = false;
            this.FaceUp = false;
            //Owning Player
            newParent.owner = (card.owner) ? card.owner : this.owner;
            //Anchoring
            newParent.anchorTo(this.transform.anchor.gameObject);
            this.anchorOff();
            card.anchorOff();
            //Adding to lists
            Managers.Object.addGameObject(newParent);
            Managers.Object.removeGameObject(this);
            Managers.Object.removeGameObject(card);
            return newParent;
        }
        else
        {
            //Achoring
            cardDeck.anchorTo(this.transform.anchor.gameObject);
            this.anchorOff();
            //Accepting into deck
            cardDeck.acceptCard(this);
            return cardDeck;
        }
    }

    public override object Clone()
    {
        return new Card(this.Face, this.Back, this.description);
    }
}
