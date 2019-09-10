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
    public Permission movePermission = Permission.OWNING_PLAYER_ONLY;
    public Permission interactPermission = Permission.OWNING_PLAYER_ONLY;
    public Permission editPermission = Permission.OWNING_PLAYER_ONLY;

    private bool ownerIsCurrent
    {
        get => !gameObject.owner
            || gameObject.owner == Managers.Players.Current;
    }

    private bool anchoredOwnerIsCurrent
    {
        get => gameObject.anchorObject &&
            (!gameObject.anchorObject.owner || gameObject.anchorObject.owner == Managers.Players.Current);
    }

    public bool canView
    {
        get => viewPermission == Permission.ALL_PLAYERS
            || ownerIsCurrent
            || anchoredOwnerIsCurrent;
    }

    public bool canMove
    {
        get => movePermission == Permission.ALL_PLAYERS
            || ownerIsCurrent
            || anchoredOwnerIsCurrent;
    }

    public bool canInteract
    {
        get => interactPermission == Permission.ALL_PLAYERS
            || ownerIsCurrent
            || anchoredOwnerIsCurrent;
    }

    public bool canEdit
    {
        get => editPermission == Permission.ALL_PLAYERS
            || ownerIsCurrent
            || anchoredOwnerIsCurrent;
    }
}
