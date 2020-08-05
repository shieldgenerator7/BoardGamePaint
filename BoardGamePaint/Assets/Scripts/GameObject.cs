using System;
using System.Collections.Generic;

public class GameObject : ICloneable
{

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
            if (description.ToLower().Contains("board"))
            {
                return "Board";
            }
            else
            {
                return "Piece";
            }
        }
    }

    public virtual string getFooterNumberString()
    {
        return null;
    }

    private string imageURL = null;
    public virtual string ImageURL
    {
        get => imageURL;
        protected set
        {
            if (imageURL == null || imageURL == "")
            {
                imageURL = value;
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

    /// <summary>
    /// The object this object is anchored to
    /// So that it can move when its anchored object moves
    /// </summary>
    public Transform transform { get; private set; }

    public GameObject(string imageURL, string description = null)
    {
        this.ImageURL = imageURL;
        this.description = description;
        transform = new Transform(this);
    }    

    public virtual bool canChangeState()
        => false;

    public virtual void changeState() { }

    public virtual bool canMakeNewObject()
        => false;

    public virtual GameObject makeNewObject()
    {
        throw new NotImplementedException("Class " + GetType() + " does not implement GameObject.makeNewObject().");
    }

    public void anchorTo(GameObject anchor)
    {
        this.transform.anchorTo(anchor?.transform);
    }

    public void anchorOff()
    {
        this.transform.anchorOff();
    }

    public static implicit operator Boolean(GameObject gameObject)
    {
        return gameObject != null;
    }    

    public virtual object Clone()
    {
        GameObject newGO = new GameObject(this.imageURL, this.description);
        return newGO;
    }
}
