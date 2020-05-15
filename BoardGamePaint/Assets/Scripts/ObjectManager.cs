using System;
using System.Collections.Generic;

public class ObjectManager
{
    List<GameObject> gameObjects;
    public List<GameObject> renderOrder { get; private set; }
    //List<WayPoint> wayPoints;

    public ObjectManager()
    {
        gameObjects = new List<GameObject>();
        renderOrder = new List<GameObject>();
        //wayPoints = new List<WayPoint>();
    }

    public void addGameObject(GameObject gameObject)
    {
        gameObjects.Add(gameObject);
        gameObjects.Sort();
        renderOrder.Add(gameObject);
        renderOrder.Sort();
        renderOrder.Reverse();
    }
    public void removeGameObject(GameObject gameObject)
    {
        gameObjects.Remove(gameObject);
        gameObjects.Sort();
        renderOrder.Remove(gameObject);
        renderOrder.Sort();
        renderOrder.Reverse();
    }
        renderOrder.Sort();
        renderOrder.Reverse();
    }
    //public void addWayPoint(WayPoint wayPoint)
    //{
    //    wayPoints.Add(wayPoint);
    //    renderOrder.Add(wayPoint);
    //    renderOrder.Sort();
    //    renderOrder.Reverse();
    //}

    public GameObject getObjectAtPosition(Vector mousePos)
    {
        GameObject backupObject = null;
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.containsPosition(mousePos))
            {
                if (gameObject.Permissions.canMove)
                {
                    return gameObject;
                }
                else
                {
                    backupObject = gameObject;
                }
            }
        }
        return backupObject;
    }

    public GameObject getAnchorObject(GameObject anchoree, Vector mousePos)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject != anchoree
                && gameObject.containsPosition(mousePos)
                && gameObject > anchoree)
            {
                return gameObject;
            }
        }
        return null;
    }

    public GameObject getSnapObject(GameObject snapee)
    {
        foreach (GameObject gameObject in Managers.Object.gameObjects)
        {
            if (gameObject != snapee)
            {
                if (snapee.canSnapTo(gameObject))
                {
                    return gameObject;
                }
            }
        }
        return null;
    }

    //public WayPoint getAnchorWayPoint(GameObject anchoree, Vector mousePos)
    //{
    //    foreach (WayPoint wayPoint in wayPoints)
    //    {
    //        if (wayPoint.containsPosition(mousePos))
    //        {
    //            //If twice the waypoint size is bigger than the selected
    //            if (wayPoint.Size.toVector() * 2 > anchoree.Size.toVector())
    //            {
    //                return wayPoint;
    //            }
    //        }
    //    }
    //    return null;
    //}
}
