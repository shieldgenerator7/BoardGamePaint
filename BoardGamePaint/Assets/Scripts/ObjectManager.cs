using System;
using System.Collections.Generic;

public class ObjectManager
{
    Dictionary<GameObject, GameObjectSprite> gameObjectMap;
    public World world { get; private set; } = new World();
    List<GameObjectSprite> gameObjectSprites;
    //List<WayPoint> wayPoints;

    public ObjectManager()
    {
        gameObjectMap = new Dictionary<GameObject, GameObjectSprite>();
        gameObjectSprites = new List<GameObjectSprite>();
        //wayPoints = new List<WayPoint>();
    }

    public GameObjectSprite addGameObject(GameObject gameObject)
    {
        GameObjectSprite gos = GameObjectSpriteFactory.makeSprite(gameObject);
        gameObjectMap.Add(gameObject, gos);
        gameObjectSprites.Add(gos);
        gameObjectSprites.Sort();
        world.gameObjects.Add(gameObject);
        return gos;
    }
    public void removeGameObject(GameObject gameObject)
    {
        GameObjectSprite gos = gameObjectMap[gameObject];
        gameObjectMap.Remove(gameObject);
        gameObjectSprites.Remove(gos);
        gameObjectSprites.Sort();
    }
    //public void addWayPoint(WayPoint wayPoint)
    //{
    //    wayPoints.Add(wayPoint);
    //    renderOrder.Add(wayPoint);
    //    renderOrder.Sort();
    //    renderOrder.Reverse();
    //}

    public GameObjectSprite getSprite(GameObject go)
    {
        GameObjectSprite gos;
        if (gameObjectMap.ContainsKey(go))
        {
            gos = gameObjectMap[go];
            if (!gos)
            {
                gameObjectMap.Remove(go);
                gos = addGameObject(go);
            }
        }
        else
        {
            gos = addGameObject(go);
        }
        return gos;
    }

    public GameObjectSprite getObjectAtPosition(Vector mousePos)
    {
        GameObjectSprite backupObject = null;
        foreach (GameObjectSprite gameObjectSprite in gameObjectSprites)
        {
            if (gameObjectSprite.containsPosition(mousePos))
            {
                if (gameObjectSprite.gameObject.Permissions.canMove)
                {
                    return gameObjectSprite;
                }
                else
                {
                    backupObject = gameObjectSprite;
                }
            }
        }
        return backupObject;
    }

    public GameObjectSprite getAnchorObject(GameObjectSprite anchoree, Vector mousePos)
    {
        foreach (GameObjectSprite gameObjectSprite in gameObjectSprites)
        {
            if (gameObjectSprite != anchoree
                && gameObjectSprite.containsPosition(mousePos)
                && gameObjectSprite > anchoree)
            {
                return gameObjectSprite;
            }
        }
        return null;
    }

    public GameObjectSprite getSnapObject(GameObjectSprite snapee)
    {
        foreach (GameObjectSprite gameObjectSprite in Managers.Object.gameObjectSprites)
        {
            if (gameObjectSprite != snapee)
            {
                if (snapee.canSnapTo(gameObjectSprite))
                {
                    return gameObjectSprite;
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
