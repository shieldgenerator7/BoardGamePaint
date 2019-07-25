using System;
using System.Collections.Generic;
using System.Drawing;

public class GameObject : IComparable<GameObject>
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
    protected List<Image> images;
    protected int imageIndex = 0;
    public Image image
    {
        get { return images?[imageIndex]; }
        set
        {
            if (images == null)
            {
                images = new List<Image>();
            }
            if (!images.Contains(value))
            {
                images.Add(value);
            }
            imageIndex = images.IndexOf(value);
        }
    }

    public Image Face
    {
        get
        {
            if (images.Count > 0)
            {
                return images[images.Count - 1];
            }
            return null;
        }

        protected set
        {
            if (images.Count > 2)
            {
                images[images.Count - 1] = value;
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    if (i > images.Count - 1
                        || images[i] == null)
                    {
                        images.Add(value);
                    }
                }
                images[1] = value;
            }
        }
    }

    public Image Back
    {
        get
        {
            if (images.Count > 0)
            {
                return images[0];
            }
            return null;
        }
        set
        {
            if (images.Count < 2)
            {
                images.Insert(0, value);
            }
            else
            {
                images[0] = value;
            }
        }
    }

    private string description = null;
    public string Description
    {
        get
        {
            if (description == null
                //if it's a facedown card
                || images.Count == 2 && imageIndex == 0)
            {
                return getTypeString();
            }
            return description;
        }
    }
    public virtual string getTypeString()
    {
        if (images == null)
        {
            return "Bin Drawer";
        }
        if (images.Count == 1)
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
        else if (images.Count == 2)
        {
            return "Card";
        }
        else if (images.Count > 2)
        {
            return "Die";
        }
        return "Unknown";
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

    public GameObject(List<Image> images)
    {
        this.position = new Vector(0, 0);
        this.images = images;
        this.imageIndex = 0;
        this.size = this.image.Size;
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
    {
        return images.Count > 1;
    }

    public virtual GameObject changeState()
    {
        if (images.Count >= 2)
        {
            //Change state
            if (images.Count == 2)
            {
                //Flip
                imageIndex = (imageIndex + 1) % 2;
            }
            else
            {
                //Randomly roll
                Random rand = new Random();
                int newIndex = imageIndex;
                while (newIndex == imageIndex)
                {
                    newIndex = rand.Next(images.Count);
                }
                imageIndex = newIndex;
            }
        }
        return null;
    }

    public virtual bool canMakeNewObject(Vector mousePos)
    {
        return false;
    }

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
}
