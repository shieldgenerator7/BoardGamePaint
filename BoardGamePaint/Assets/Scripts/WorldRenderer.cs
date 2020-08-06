using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

public class WorldRenderer
{
    const int MAX_VISIBLE_CARD_COUNT = 100;
    const float CARD_SPACING = 0.5f;
    const int DEFAULT_COMPONENT_SIZE = 50;

    private Graphics graphics;
    private Font font;
    private Brush brush;

    ColorMap[] colorMap;
    ImageAttributes attr;
    Brush backBrush;
    Rectangle backRect;

    public WorldRenderer()
    {
        //Footer String
        font = new Font("Ariel", 18);
        brush = new SolidBrush(Color.Black);

        //Player Button
        //2019-09-09: ColorMap code copied from https://stackoverflow.com/a/27101587/2336212
        // Set the image attribute's color mappings
        colorMap = new ColorMap[1];
        colorMap[0] = new ColorMap();
        colorMap[0].OldColor = Color.White;
        attr = new ImageAttributes();

        //Tray
        backBrush = new SolidBrush(Color.FromArgb(206, 117, 57));
        backRect = new Rectangle(0, 0, DEFAULT_COMPONENT_SIZE * 2, DEFAULT_COMPONENT_SIZE);
    }

    public void renderWorld(Graphics graphics, List<GameObject> gameObjects)
    {
        this.graphics = graphics;
        //gameObjects.ForEach(gameObject => renderGameObject((dynamic)gameObject));
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject is CardDeck)
            {
                renderGameObject((CardDeck)gameObject);
            }
            else if (gameObject is PlayerButton)
            {
                renderGameObject((PlayerButton)gameObject);
            }
            else if (gameObject is Tray)
            {
                renderGameObject((Tray)gameObject);
            }
            else if (gameObject is TrayComponent)
            {
                renderGameObject((TrayComponent)gameObject);
            }
            else
            {
                renderGameObject((GameObject)gameObject);
            }
        }
    }

    private Vector getTopLeftScreen(Transform t, Size size)
        => Managers.Display.convertToScreen(new Vector(
            t.position.x - size.Width / 2,
            t.position.y - size.Height / 2
            ));

    private void renderGameObject(GameObject gameObject)
    {
        //Draw image
        Image image = ImageBank.getImage(gameObject.ImageURL);
        Vector topLeftScreen = getTopLeftScreen(gameObject.transform, image.Size);
        graphics.DrawImage(
            image,
            topLeftScreen.x,
            topLeftScreen.y,
            image.Size.Width,
            image.Size.Height
            );
        //Draw footer number
        drawFooterString(gameObject, topLeftScreen);
    }

    private void drawFooterString(GameObject gameObject, Vector position)
    {
        string footerString = gameObject.getFooterNumberString();
        if (footerString != null && footerString != "")
        {
            graphics.DrawString(
                footerString,
                font,
                brush,
                position.x,
                position.y
                );
        }
    }

    //private void renderGameObject(Die die)
    //{
    //}

    private void renderGameObject(CardDeck cardDeck)
    {
        renderGameObject((GameObject)cardDeck);
        Image image = ImageBank.getImage(cardDeck.ImageURL);
        Vector topLeftScreen = getTopLeftScreen(cardDeck.transform, image.Size);
        int cardCount = (cardDeck.cards != null) ? cardDeck.cards.Count : 1;
        if (cardCount > 1)
        {
            int limit = Math.Min(MAX_VISIBLE_CARD_COUNT, cardCount);
            for (int i = 1; i < limit; i++)
            {
                graphics.DrawImage(
                image,
                topLeftScreen.x,
                topLeftScreen.y - i * CARD_SPACING,
                image.Size.Width,
                image.Size.Height
                );
            }
        }
        drawFooterString(cardDeck, topLeftScreen);
    }

    private void renderGameObject(PlayerButton playerButton)
    {
        Image image = ImageBank.getImage(playerButton.ImageURL);
        Vector topLeftScreen = getTopLeftScreen(
            playerButton.transform,
            new Size(DEFAULT_COMPONENT_SIZE, DEFAULT_COMPONENT_SIZE)
            );
        // Draw using the color map
        Rectangle rect = new Rectangle(
            (int)topLeftScreen.x,
            (int)topLeftScreen.y,
            DEFAULT_COMPONENT_SIZE,
            DEFAULT_COMPONENT_SIZE
            );
        colorMap[0].NewColor = playerButton.player?.color ?? Color.Gray;
        attr.SetRemapTable(colorMap);
        graphics.DrawImage(
            image,
            rect,
            0,
            0,
            image.Size.Width,
            image.Size.Height,
            GraphicsUnit.Pixel,
            attr
            );
        drawFooterString(playerButton, topLeftScreen);
    }

    private void renderGameObject(Tray tray)
    {
        backRect.X = (int)tray.transform.position.x;
        backRect.Y = (int)tray.transform.position.y;
        backRect.Width = (tray.trayComponents.Count + 2) * DEFAULT_COMPONENT_SIZE;
        graphics.FillRectangle(
            backBrush,
            Managers.Display.convertToScreen(backRect)
            );
        tray.trayComponents.ForEach(tc => renderGameObject(tc));
    }
    private void renderGameObject(TrayComponent trayComponent)
    {
        backRect.X = (int)trayComponent.transform.position.x - DEFAULT_COMPONENT_SIZE / 2;
        backRect.Y = (int)trayComponent.transform.position.y - DEFAULT_COMPONENT_SIZE / 2;
        Image image = ImageBank.getImage(trayComponent.ImageURL);
        Vector topLeftScreen = getTopLeftScreen(
            trayComponent.transform,
            new Size(DEFAULT_COMPONENT_SIZE, DEFAULT_COMPONENT_SIZE)
            );
        graphics.DrawImage(
            image,
            topLeftScreen.x,
            topLeftScreen.y,
            DEFAULT_COMPONENT_SIZE,
            DEFAULT_COMPONENT_SIZE
            );
        //Draw footer number
        drawFooterString(trayComponent, topLeftScreen);
    }
}
