﻿using BoardGamePaint;
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
    }

    public Card(GameObject cardToBe, Image backImage) :
        this(cardToBe.Face, backImage, cardToBe.Description)
    { }

    public override string getTypeString()
    {
        return "Card";
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
        //Change state
        if (images.Count == 2)
        {
            //Flip
            imageIndex = (imageIndex + 1) % 2;
        }
    }

    public override bool canMakeNewObject(Vector mousePos)
    {
        return false;
    }

    public override bool fitsInDeck(GameObject other)
    {
        return other is Card && base.fitsInDeck(other);
    }

    public override void acceptCard(CardDeck cardDeck)
    {
        if (!(cardDeck is Card))
        {
            return;
        }
        Card card = (Card)cardDeck;
        CardDeck newParent = new CardDeck(
            new List<Card>() { this, card },
            card.Back,
            null
            );
        newParent.moveTo(Position, false);
        card.image = card.Back;
        Managers.Form.addGameObject(newParent);
        Managers.Form.removeGameObject(this);
        Managers.Form.removeGameObject(card);
    }

    public override object Clone()
    {
        Card newCard = new Card(this.Face, this.Back, this.description);
        newCard.FileName = (string)this.FileName;
        return newCard;
    }
}
