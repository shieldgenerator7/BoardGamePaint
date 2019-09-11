using System;
using System.Drawing;
using System.IO;
using System.Reflection;

public static class ImageUtility
{
    public static string resourcePath = "BoardGamePaint.Assets.Images.";
    public static string fileExtension = ".png";

    public static Image getImage(string imageName)
    {
        //2019-09-10: copied from http://www.csharp411.com/embedded-image-resources/
        Assembly myAssembly = Assembly.GetExecutingAssembly();
        Stream myStream = myAssembly.GetManifestResourceStream(
            resourcePath + imageName + fileExtension
            );
        Image image = Image.FromStream(myStream);
        return image;
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
