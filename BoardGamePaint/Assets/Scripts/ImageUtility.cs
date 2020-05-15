using System;
using System.Drawing;
using System.IO;
using System.Reflection;

public static class ImageUtility
{
    public static string resourcePath = "Assets/Images/";
    public static string fileExtension = ".png";

    public static string getImageURL (string imageName)
    => resourcePath + imageName + fileExtension;

    public static Image getImage(string imageName)
    {
        string path = getImageURL(imageName);
        try
        {
            return Image.FromFile(path);
        }
        catch (Exception e)
        {
            System.Windows.Forms.MessageBox.Show(
                "Resource error: can't find: " + path + ", message: " + e.Message
                );
            File.Create("RESOURCE_ERROR.txt");
        }
        return null;
    }

    public static void showValidResourcePaths()
    {
        System.Windows.Forms.MessageBox.Show("Valid resource paths: ");
        //2019-09-10: copied from http://www.csharp411.com/embedded-image-resources/
        Assembly myAssembly = Assembly.GetExecutingAssembly();
        string[] names = myAssembly.GetManifestResourceNames();
        foreach (string name in names)
        {
            System.Windows.Forms.MessageBox.Show(name);
        }
    }
}
