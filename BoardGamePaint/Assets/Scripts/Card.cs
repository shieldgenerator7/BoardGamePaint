using BoardGamePaint;
using System;
using System.Collections.Generic;
using System.Drawing;

public class Card : CardDeck
{
    public Card(Image frontImage, Image backImage, string description = null)
        : base(null, backImage, description)
    {
        Face = frontImage;
        Back = backImage;
        outerSize = Size;
        Permissions.interactPermission = Permissions.Permission.OWNING_PLAYER_ONLY;
        Permissions.viewPermission = Permissions.Permission.OWNING_PLAYER_ONLY;
    }

    public Card(GameObject cardToBe, Image backImage) :
        this(cardToBe.image, backImage, cardToBe.Description)
    { }

    public override Image image
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

    protected override string getFooterNumberString()
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

    public override bool canMakeNewObject(Vector mousePos)
    {
        return false;
    }

    public override void acceptCard(CardDeck cardDeck)
    {
        if (cardDeck is Card)
        {
            Card card = (Card)cardDeck;
            CardDeck newParent = new CardDeck(
                new List<Card>() { this, card },
                card.Back,
                null
                );
            newParent.moveTo(Position, false);
            card.FaceUp = false;
            this.FaceUp = false;
            //Owning Player
            newParent.owner = (card.owner) ? card.owner : this.owner;
            //Anchoring
            newParent.anchorTo(this.anchorObject);
            this.anchorOff();
            card.anchorOff();
            //Adding to lists
            Managers.Object.addGameObject(newParent);
            Managers.Object.removeGameObject(this);
            Managers.Object.removeGameObject(card);
        }
        else
        {
            //Achoring
            cardDeck.anchorTo(this.anchorObject);
            this.anchorOff();
            //Accepting into deck
            cardDeck.acceptCard(this);
        }
    }

    public override object Clone()
    {
        Card newCard = new Card(this.Face, this.Back, this.description);
        newCard.FileName = (string)this.FileName;
        return newCard;
    }
}
