using BoardGamePaint;
using System;
using System.Drawing;
using System.IO;

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

    private BinManagerSprite binManager;
    public static BinManagerSprite Bin
    {
        get => instance.binManager;
    }

    private TraySprite commandTray;
    public static TraySprite Command
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
        this.binManager = new BinManagerSprite(new BinManager());
        //Command Tray
        this.commandTray = new TraySprite(new BinManager());
        ((Tray)commandTray.gameObject).addComponent(new ExitButton(ImageUtility.getImageURL("exit")));
        ((Tray)commandTray.gameObject).addComponent(new AddPlayerButton(ImageUtility.getImageURL("newplayer")));
        //Player Manager
        this.playerManager = new PlayerManager();
    }
}
