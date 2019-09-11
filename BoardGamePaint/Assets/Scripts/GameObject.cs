using System;
using System.Collections.Generic;
using System.Drawing;

public class GameObject : IComparable<GameObject>, ICloneable
{
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

    protected string description = null;
    public virtual string Description
    {
        get
        {
            if (description == null)
            {
                return TypeString;
            }
            return description;
        }
    }
    public virtual string TypeString
    {
        get
        {
            if (anchoredObjects.Count > 0)
            {
                return "Board";
            }
            else
            {
                return "Piece";
            }
        }
    }

    protected virtual string getFooterNumberString()
    {
        return null;
    }

    private string fileName = null;
    public string FileName
    {
        get => fileName;
        set
        {
            if (fileName == null || fileName == "")
            {
                fileName = value;
            }
        }
    }

    public Player owner { get; set; }
    private Permissions permissions;
    public Permissions Permissions
    {
        get
        {
            if (permissions == null)
            {
                permissions = new Permissions(this);
            }
            return permissions;
        }
    }

    //Pickup Runtime Vars
    private Vector pickupOffset = new Vector(0, 0);

    /// <summary>
    /// The object this object is anchored to
    /// So that it can move when its anchored object moves
    /// </summary>
    public GameObject anchorObject { get; private set; }
    readonly private List<GameObject> anchoredObjects = new List<GameObject>();

    public GameObject(Image image, string description = null)
    {
        this.position = new Vector(0, 0);
        if (image != null)
        {
            this.size = image.Size;
            this.image = image;
        }
        this.description = description;
    }
    public GameObject(string fileName, string description = null)
        : this(Image.FromFile(fileName), description)
    {
        this.FileName = fileName;
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
        string footerString = getFooterNumberString();
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
        foreach (GameObject anchored in anchoredObjects)
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
        foreach (GameObject anchored in anchoredObjects)
        {
            anchored.moveTo(pos, useOffset);
        }
    }

    public void anchorTo(GameObject anchor)
    {
        if (this.anchorObject)
        {
            anchorOff();
        }
        this.anchorObject = anchor;
        this.anchorObject.anchoredObjects.Add(this);
    }

    public void anchorOff()
    {
        if (anchorObject)
        {
            this.anchorObject.anchoredObjects.Remove(this);
            this.anchorObject = null;
        }
    }

    public virtual bool canChangeState()
        => false;

    public virtual void changeState() { }

    public virtual bool canMakeNewObject(Vector mousePos)
        => false;

    public virtual GameObject makeNewObject()
    {
        throw new NotImplementedException("Class " + GetType() + " does not implement GameObject.makeNewObject().");
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

    public bool canSnapTo(GameObject go)
        => canSnapToHorizontal(go)
        || canSnapToVertical(go);

    public bool canSnapToHorizontal(GameObject go)
    {
        Rectangle thisRect = getRect();
        Rectangle goRect = go.getRect();
        int snapThreshold = SnapThresholdX;
        bool horizontal = Math.Abs(position.x - go.position.x) > Math.Abs(position.y - go.position.y);
        //horizontal snapping
        if (horizontal
            && size.Height == go.size.Height
            && Math.Abs(position.y - go.position.y) < snapThreshold
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
    public bool canSnapToVertical(GameObject go)
    {
        Rectangle thisRect = getRect();
        Rectangle goRect = go.getRect();
        int snapThreshold = SnapThresholdY;
        bool vertical = Math.Abs(position.x - go.position.x) < Math.Abs(position.y - go.position.y);
        //vertical snapping
        if (vertical
            && size.Width == go.size.Width
            && Math.Abs(position.x - go.position.x) < snapThreshold
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
    /// <param name="go"></param>
    public void snapTo(GameObject go)
    {
        Rectangle thisRect = getRect();
        Rectangle goRect = go.getRect();
        Vector desiredPosition = position;
        //Snap horizontal
        if (canSnapToHorizontal(go))
        {
            desiredPosition.y = go.position.y;
            //snap on left side
            if (position.x < go.position.x)
            {
                desiredPosition.x = goRect.Left - size.Width / 2;
            }
            //snap on right side
            if (position.x > go.position.x)
            {
                desiredPosition.x = goRect.Right + size.Width / 2;
            }
        }
        //Snap vertical
        if (canSnapToVertical(go))
        {
            desiredPosition.x = go.position.x;
            //snap on top side
            if (position.y < go.position.y)
            {
                desiredPosition.y = goRect.Top - size.Height / 2;
            }
            //snap on bottom side
            if (position.y > go.position.y)
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

    public static implicit operator Boolean(GameObject gameObject)
    {
        return gameObject != null;
    }

    public int CompareTo(GameObject go)
    {
        float thisSize = this.size.toVector().Magnitude;
        float goSize = go.size.toVector().Magnitude;
        bool thisCardDeck = (this is CardDeck);
        bool goCardDeck = (go is CardDeck);
        return (thisSize == goSize)
            ? (thisCardDeck && !goCardDeck)
                ? 1
                : (goCardDeck && !thisCardDeck)
                    ? -1
                    : 0
            : (int)(this.size.toVector().Magnitude - go.size.toVector().Magnitude);
    }

    public static bool operator <(GameObject a, GameObject b)
    {
        float aSize = a.size.toVector().Magnitude;
        float bSize = b.size.toVector().Magnitude;
        return (aSize == bSize)
            ? b is CardDeck
            : aSize < bSize;
    }

    public static bool operator >(GameObject a, GameObject b)
    {
        float aSize = a.size.toVector().Magnitude;
        float bSize = b.size.toVector().Magnitude;
        return (aSize == bSize)
            ? a is CardDeck
            : aSize > bSize;
    }

    public virtual object Clone()
    {
        GameObject newGO = new GameObject(image);
        newGO.description = (string)this.description;
        newGO.fileName = (string)this.fileName;
        return newGO;
    }
}
