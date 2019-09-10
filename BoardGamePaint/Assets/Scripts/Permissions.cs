using System;

public class Permissions
{
    public enum Permission
    {
        OWNING_PLAYER_ONLY,
        ALL_PLAYERS
    }

    public Permission viewPermission = Permission.ALL_PLAYERS;
    public Permission movePermission = Permission.ALL_PLAYERS;
    public Permission interactPermission = Permission.ALL_PLAYERS;
    public Permission editPermission = Permission.ALL_PLAYERS;
}
