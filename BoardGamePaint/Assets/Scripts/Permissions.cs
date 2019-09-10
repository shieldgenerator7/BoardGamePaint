using System;

public class Permissions
{
    private GameObject gameObject;

    public enum Permission
    {
        OWNING_PLAYER_ONLY,
        ALL_PLAYERS
    }

    public Permissions(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public Permission viewPermission = Permission.ALL_PLAYERS;
    public Permission movePermission = Permission.ALL_PLAYERS;
    public Permission interactPermission = Permission.ALL_PLAYERS;
    public Permission editPermission = Permission.ALL_PLAYERS;

    private bool ownerIsCurrent
    {
        get => !gameObject.owner
            || gameObject.owner == Managers.Players.Current;
    }

    public bool canView
    {
        get => viewPermission == Permission.ALL_PLAYERS
            || ownerIsCurrent;
    }

    public bool canMove
    {
        get => movePermission == Permission.ALL_PLAYERS
            || ownerIsCurrent;
    }

    public bool canInteract
    {
        get => interactPermission == Permission.ALL_PLAYERS
            || ownerIsCurrent;
    }

    public bool canEdit
    {
        get => editPermission == Permission.ALL_PLAYERS
            || ownerIsCurrent;
    }
}
