using System;
using System.Collections.Generic;

public class Transform
{

    public Vector position;
    //public float rotation;
    //public Vector scale;

    public GameObject gameObject { get; private set; }

    /// <summary>
    /// The transform this transform is anchored to
    /// So that it can move when its anchored transform moves
    /// Scaling the anchored transform does not scale this transform
    /// </summary>
    public Transform anchor { get; private set; }
    public readonly List<Transform> anchorlings = new List<Transform>();
    public Vector anchorOffset { get; private set; } = Vector.zero;

    public Transform(GameObject gameObject)
        : this(gameObject, Vector.zero, Vector.one) { }
    public Transform(GameObject gameObject, Vector position)
        : this(gameObject, position, Vector.one) { }
    public Transform(GameObject gameObject, Vector position, Vector scale)
    {
        this.gameObject = gameObject;
        this.position = position;
        //this.scale = scale;
    }

    public void anchorTo(Transform anchor)
    {
        if (this.anchor)
        {
            anchorOff();
        }
        if (anchor)
        {
            this.anchor = anchor;
            this.anchor.anchorlings.Add(this);
            this.anchorOffset = this.position - this.anchor.position;
        }
    }

    public void anchorOff()
    {
        if (this.anchor)
        {
            this.anchor.anchorlings.Remove(this);
        }
        this.anchor = null;
        this.anchorOffset = Vector.zero;
    }

    public static implicit operator bool(Transform t)
        => t != null;
}
