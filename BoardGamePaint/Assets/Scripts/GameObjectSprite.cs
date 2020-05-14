using System;
using System.Collections.Generic;
using System.Drawing;

public class GameObjectSprite: IComparable<GameObjectSprite>
{
	GameObject gameObject;

	public const int SNAP_THRESHOLD = 10;//the max snap distance in pixels

    private static Image hiddenImage = null;
    protected static Image HiddenImage
    {
        get
        {
            if (hiddenImage == null)
            {
                hiddenImage = ImageUtility.getImage("hidden");
            }
            return hiddenImage;
        }
    }

    //Drawing Runtime Vars
    protected Vector position;
    public Vector Position
    {
        get { return position; }
    }
    protected Size size;
    public Size Size
    {
        get { return new Size(size.Width, size.Height); }
    }
    private Vector top_left = Vector.zero;
    public Vector TopLeft
    {
        get
        {
            top_left.x = position.x - size.Width / 2;
            top_left.y = position.y - size.Height / 2;
            return top_left;
        }
    }
    public Vector TopLeftScreen
    {
        get => Managers.Display.convertToScreen(TopLeft);
    }

    public virtual Image image { get; protected set; }

    //Pickup Runtime Vars
    private Vector pickupOffset = new Vector(0, 0);

    /// <summary>
    /// The object this object is anchored to
    /// So that it can move when its anchored object moves
    /// </summary>
    public GameObjectSprite anchorObject { get; private set; }
    readonly private List<GameObjectSprite> anchoredObjects = new List<GameObjectSprite>();

    public GameObjectSprite(GameObject gameObject)
	{
        this.gameObject = gameObject;
        image = Image.FromFile(gameObject.ImageURL);

        position = new Vector(0, 0);
        if (image != null)
        {
            size = image.Size;
            image = image;
        }
    }

    public virtual void draw(Graphics graphics)
    {
        graphics.DrawImage(
            image,
            TopLeftScreen.x,
            TopLeftScreen.y,
            size.Width,
            size.Height
            );
        drawFooterNumber(graphics);
    }
    public void drawFooterNumber(Graphics graphics)
    {
        string footerString = gameObject.getFooterNumberString();
        if (footerString != null && footerString != "")
        {
            Font font = new Font("Ariel", 18);
            Brush brush = new SolidBrush(Color.Black);
            graphics.DrawString(
                footerString,
                font,
                brush,
                TopLeftScreen.x,
                TopLeftScreen.y
                );
        }
    }

    public virtual bool containsPosition(Vector pos)
    {
        float halfWidth = size.Width / 2;
        float halfHeight = size.Height / 2;
        return pos.x >= position.x - halfWidth
            && pos.x <= position.x + halfWidth
            && pos.y >= position.y - halfHeight
            && pos.y <= position.y + halfHeight;
    }

    public void pickup(Vector pickupPos)
    {
        pickupOffset = position - pickupPos;
        foreach (GameObjectSprite anchored in anchoredObjects)
        {
            anchored.pickup(pickupPos);
        }
    }

    public Vector getPickupPosition()
    {
        return position - pickupOffset;
    }

    public virtual void moveTo(Vector pos, bool useOffset = true)
    {
        if (useOffset)
        {
            position = pos + pickupOffset;
        }
        else
        {
            position = pos;
        }
        foreach (GameObjectSprite anchored in anchoredObjects)
        {
            anchored.moveTo(pos, useOffset);
        }
    }

    public void anchorTo(GameObjectSprite anchor)
    {
        if (this.anchorObject)
        {
            anchorOff();
        }
        if (anchor)
        {
            this.anchorObject = anchor;
            this.anchorObject.anchoredObjects.Add(this);
        }
    }

    public void anchorOff()
    {
        if (anchorObject)
        {
            this.anchorObject.anchoredObjects.Remove(this);
            this.anchorObject = null;
        }
    }

    public int SnapThresholdX
    {
        get =>
            (int)Math.Max(
                size.Width * 0.10,
                Math.Min(
                    size.Width * 0.25,
                    SNAP_THRESHOLD
                    )
                );
    }
    public int SnapThresholdY
    {
        get =>
            (int)Math.Max(
                size.Height * 0.10,
                Math.Min(
                    size.Height * 0.25,
                    SNAP_THRESHOLD
                    )
                );
    }

    public bool canSnapTo(GameObjectSprite gos)
        => canSnapToHorizontal(gos)
        || canSnapToVertical(gos);

    public bool canSnapToHorizontal(GameObjectSprite gos)
    {
        Rectangle thisRect = getRect();
        Rectangle goRect = gos.getRect();
        int snapThreshold = SnapThresholdX;
        bool horizontal = Math.Abs(position.x - gos.position.x) > Math.Abs(position.y - gos.position.y);
        //horizontal snapping
        if (horizontal
            && size.Height == gos.size.Height
            && Math.Abs(position.y - gos.position.y) < snapThreshold
            && (
                Math.Abs(thisRect.Left - goRect.Right) < snapThreshold
                || Math.Abs(thisRect.Right - goRect.Left) < snapThreshold
                )
            )
        {
            return true;
        }
        return false;
    }
    public bool canSnapToVertical(GameObjectSprite gos)
    {
        Rectangle thisRect = getRect();
        Rectangle goRect = gos.getRect();
        int snapThreshold = SnapThresholdY;
        bool vertical = Math.Abs(position.x - gos.position.x) < Math.Abs(position.y - gos.position.y);
        //vertical snapping
        if (vertical
            && size.Width == gos.size.Width
            && Math.Abs(position.x - gos.position.x) < snapThreshold
            && (
                Math.Abs(thisRect.Top - goRect.Bottom) < snapThreshold
                || Math.Abs(thisRect.Bottom - goRect.Top) < snapThreshold
                )
            )
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Snaps this GameObject to the given GameObject
    /// </summary>
    /// <param name="gos"></param>
    public void snapTo(GameObjectSprite gos)
    {
        Rectangle thisRect = getRect();
        Rectangle goRect = gos.getRect();
        Vector desiredPosition = position;
        //Snap horizontal
        if (canSnapToHorizontal(gos))
        {
            desiredPosition.y = gos.position.y;
            //snap on left side
            if (position.x < gos.position.x)
            {
                desiredPosition.x = goRect.Left - size.Width / 2;
            }
            //snap on right side
            if (position.x > gos.position.x)
            {
                desiredPosition.x = goRect.Right + size.Width / 2;
            }
        }
        //Snap vertical
        if (canSnapToVertical(gos))
        {
            desiredPosition.x = gos.position.x;
            //snap on top side
            if (position.y < gos.position.y)
            {
                desiredPosition.y = goRect.Top - size.Height / 2;
            }
            //snap on bottom side
            if (position.y > gos.position.y)
            {
                position.y = goRect.Bottom + size.Height / 2;
            }
        }
        //Snap
        moveTo(desiredPosition - pickupOffset);
    }

    public virtual Rectangle getRect()
    {
        return new Rectangle(
            (int)TopLeft.x,
            (int)TopLeft.y,
            size.Width,
            size.Height);
    }

    public static implicit operator Boolean(GameObjectSprite gameObjectSprite)
    {
        return gameObjectSprite != null;
    }

    public virtual int CompareTo(GameObjectSprite gos)
    {
        float thisSize = this.size.toVector().Magnitude;
        float goSize = gos.size.toVector().Magnitude;
        bool thisCardDeck = (this is CardDeck);
        bool goCardDeck = (gos is CardDeck);
        return (thisSize == goSize)
            ? (thisCardDeck && !goCardDeck)
                ? 1
                : (goCardDeck && !thisCardDeck)
                    ? -1
                    : 0
            : (int)(this.size.toVector().Magnitude - gos.size.toVector().Magnitude);
    }

    public static bool operator <(GameObjectSprite a, GameObjectSprite b)
    {
        float aSize = a.size.toVector().Magnitude;
        float bSize = b.size.toVector().Magnitude;
        return (aSize == bSize)
            ? b is CardDeck
            : aSize < bSize;
    }

    public static bool operator >(GameObjectSprite a, GameObjectSprite b)
    {
        float aSize = a.size.toVector().Magnitude;
        float bSize = b.size.toVector().Magnitude;
        return (aSize == bSize)
            ? a is CardDeck
            : aSize > bSize;
    }

    public virtual object Clone()
    {
        GameObjectSprite newGOS = new GameObjectSprite((GameObject)this.gameObject.Clone());
        return newGOS;
    }
}
