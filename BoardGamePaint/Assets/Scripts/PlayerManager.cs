using System;
using System.Collections.Generic;
using System.Drawing;

public class PlayerManager
{
    private int currentIndex = 0;
    readonly private List<Player> players = new List<Player>();
    readonly private List<PlayerButton> playerButtons = new List<PlayerButton>();

    PlayerButton neutralButton;

    public Player Current
    {
        get => (players.Count > 0 && currentIndex >= 0)
            ? players[currentIndex]
            : null;
        set => currentIndex = players.IndexOf(value);
    }

    public PlayerButton CurrentButton
    {
        get => (playerButtons.Count > 0 && currentIndex >= 0)
            ? playerButtons[currentIndex]
            : neutralButton;
    }

    public static List<Color> allowedColors = new List<Color>()
    {
        Color.Red,
        Color.Blue,
        Color.Yellow,
        Color.Green
    };

    public PlayerManager()
    {
        neutralButton = new PlayerButton(null);
        ((Tray)Managers.Command.gameObject).addComponent(neutralButton);
    }

    public void makeNewPlayer()
    {
        int colorIndex = players.Count % allowedColors.Count;
        Player newPlayer = new Player(allowedColors[colorIndex]);
        players.Add(newPlayer);
        PlayerButton newButton = new PlayerButton(newPlayer);
        playerButtons.Add(newButton);
        ((Tray)Managers.Command.gameObject).addComponent(newButton);
        //Set current player
        Current = newPlayer;
    }

    public void nextTurn()
    {
        currentIndex = (currentIndex + 1) % players.Count;
    }

    public int Count
    {
        get => players.Count;
    }
}
