using BoardGamePaint;
using System;
using System.Drawing;

public class Managers
{
    private static Managers instance;

    private MainForm mainForm;

    private ObjectManager objectManager;
    public static ObjectManager Object
    {
        get => instance.objectManager;
    }

    private ControlManager controlManager;
    public static ControlManager Control
    {
        get => instance.controlManager;
    }

    private DisplayManager displayManager;
    public static DisplayManager Display
    {
        get => instance.displayManager;
    }

    private BinManager binManager;
    public static BinManager Bin
    {
        get => instance.binManager;
    }

    private Tray commandTray;
    public static Tray Command
    {
        get => instance.commandTray;
    }

    private PlayerManager playerManager;
    public static PlayerManager Players
    {
        get => instance.playerManager;
    }

    public static void init(MainForm mf)
    {
        if (instance == null)
        {
            new Managers(mf);
        }
    }

    public Managers(MainForm mf)
    {
        instance = this;
        this.mainForm = mf;
        this.objectManager = new ObjectManager();
        this.controlManager = new ControlManager();
        this.displayManager = new DisplayManager();
        this.binManager = new BinManager();
        //Command Tray
        this.commandTray = new Tray();
        commandTray.addComponent(new ExitButton(Image.FromFile("exit.png"),50));
        commandTray.addComponent(new AddPlayerButton(Image.FromFile("newplayer.png"), 50));
        //Player Manager
        this.playerManager = new PlayerManager();
    }
}
