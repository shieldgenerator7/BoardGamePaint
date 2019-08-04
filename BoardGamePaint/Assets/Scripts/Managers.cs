using BoardGamePaint;
using System;

public class Managers
{
    private static Managers instance;

    private MainForm mainForm;
    public static MainForm Form
    {
        get => instance.mainForm;
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
        this.binManager = new BinManager();
        this.commandTray = new Tray();
    }
}
