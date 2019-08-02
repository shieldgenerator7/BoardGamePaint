using System;
using System.Collections.Generic;
using System.Drawing;

public class GameObject : IComparable<GameObject>, ICloneable
{

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

    //Pickup Runtime Vars
    private Vector pickupOffset = new Vector(0, 0);

    /// <summary>
    /// The object this object is anchored to
    /// So that it can move when its anchored object moves
    /// </summary>
    private GameObject anchorObject;
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
            position.x - size.Width / 2,
            position.y - size.Height / 2,
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
                position.x + size.Width / 2,
                position.y + size.Height / 2
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

    public virtual Rectangle getRect()
    {
        return new Rectangle(
            (int)position.x - size.Width / 2,
            (int)position.y - size.Height / 2,
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
