using System;
using System.Collections.Generic;
using System.Drawing;

public static class ImageBank
{
    private static readonly Dictionary<string, Image> images = new Dictionary<string, Image>();

    public static Image getImage(string filename)
    {
        if (filename == null || filename == "")
        {
            return null;
        }
        if (!images.ContainsKey(filename))
        {
            images.Add(filename, Image.FromFile(filename));
        }
        return images[filename];
    }

    public static void refreshImages()
    {
        foreach (string filename in images.Keys)
        {
            images[filename] = Image.FromFile(filename);
        }
    }

    public static void preloadImages(params string[] filenames)
        => Array.ForEach(filenames, filename => getImage(filename));

    public static void preloadImages(List<string> filenames)
        => filenames.ForEach(filename => getImage(filename));
}
