﻿using System;
using System.Collections.Generic;
using System.Drawing;

public class CardDeck : GameObject
{
    int MAX_VISIBLE_CARD_COUNT = 100;
    float CARD_SPACING = 0.5f;

    readonly List<GameObject> cards = new List<GameObject>();

    readonly Random random = new Random();

    public CardDeck(List<GameObject> cards, Image backImage, string description = null) : base(backImage, description)
    {
        this.cards = cards;
    }

    public override string getTypeString()
        => "Deck of Cards"
        + ((cards.Count == 0)
            ? " (Empty)"
            : ""
            );

    public override void draw(Graphics graphics)
    {
        base.draw(graphics);
        if (cards.Count > 1)
        {
            int limit = Math.Min(MAX_VISIBLE_CARD_COUNT, images.Count);
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
    }

    public override bool containsPosition(Vector pos)
    {
        float halfWidth = size.Width / 2;
        float halfHeight = size.Height / 2;
        float bonusHeight = getBonusHeight();
        return pos.x >= position.x - halfWidth
            && pos.x <= position.x + halfWidth
            && pos.y >= position.y - halfHeight - bonusHeight
            && pos.y <= position.y + halfHeight;
    }

    public float getBonusHeight()
    {
        int limit = Math.Min(MAX_VISIBLE_CARD_COUNT, images.Count);
        return limit * CARD_SPACING;
    }

    public override Rectangle getRect()
    {
        int bonusHeight = (int)getBonusHeight();
        return new Rectangle(
            (int)position.x - size.Width / 2,
            (int)position.y - size.Height / 2 - bonusHeight,
            size.Width,
            size.Height + bonusHeight);
    }

    public override bool canChangeState()
    {
        return cards.Count > 0;
    }

    public override GameObject changeState()
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
        return newCard;
    }

    public bool fitsInDeck(GameObject other)
        => other.Back == this.Back
        && other.Size == this.Size;

    public void acceptCard(GameObject card)
    {
        card.image = card.Back;
        cards.Add(card);
    }
}
