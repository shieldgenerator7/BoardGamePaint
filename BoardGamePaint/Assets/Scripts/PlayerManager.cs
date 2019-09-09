using System;
using System.Collections.Generic;
using System.Drawing;

public class PlayerManager
{
    private int currentIndex = 0;
    readonly private List<Player> players = new List<Player>();

    public Player Current
    {
        get => (players.Count > 0)
            ? players[currentIndex]
            : null;
        set => currentIndex = Math.Max(0, players.IndexOf(value));
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
    }

    public void makeNewPlayer()
    {
        int colorIndex = players.Count % allowedColors.Count;
        Player newPlayer = new Player(allowedColors[colorIndex]);
        players.Add(newPlayer);
        Managers.Command.addComponent(
            new PlayerButton(newPlayer, Tray.DEFAULT_COMPONENT_SIZE)
            );
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
