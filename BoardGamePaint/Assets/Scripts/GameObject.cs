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
    public string ImageURL
    {
        get => imageURL;
        set
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


    public GameObject(string imageURL, string description = null)
    {
        this.ImageURL = imageURL;
        this.description = description;
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
