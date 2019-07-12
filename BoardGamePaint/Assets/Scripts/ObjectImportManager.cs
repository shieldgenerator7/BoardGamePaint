using BoardGamePaint;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

public static class ObjectImportManager
{

    public static bool isFileJSON(string filename)
        => filename.ToLower().EndsWith(".txt")
        || filename.ToLower().EndsWith(".json");

    public static void importObject(MainForm mf, string filename)
    {
        List<Image> imagesToProcess = new List<Image>();
        if (isFileJSON(filename))
        {
            string foldername = filename.Substring(0, filename.LastIndexOf("\\") + 1);
            //parse the JSON file
            JObject jo = JObject.Parse(File.ReadAllText(filename));
            foreach (JToken joImage in jo["images"])
            {
                string imageFilename = "default.png";
                int cardCount = 1;
                foreach (JToken jotkn in joImage.Children())
                {
                    imageFilename = foldername + (string)jotkn.SelectToken("image");
                    try
                    {
                        cardCount = (int)jotkn.SelectToken("quantity");
                    }
                    catch (System.ArgumentNullException)
                    {
                        cardCount = 1;
                    }
                    Trace.WriteLine("joimage name: " + joImage.Path);
                    Trace.WriteLine("joimage chldrn: " + imageFilename);
                    Trace.WriteLine("joimage chldrn: " + cardCount);
                }
                if (!File.Exists(imageFilename))
                {
                    continue;
                }
                Image image = Image.FromFile(imageFilename);
                for (int i = 0; i < cardCount; i++)
                {
                    imagesToProcess.Add(image);
                }
            }
            string backFilename = foldername + (string)jo["back"];
            Trace.WriteLine("jo back: " + backFilename);
            Image backImage = (File.Exists(backFilename))
                ? Image.FromFile(backFilename)
                : null;
            processImages(mf, imagesToProcess, backImage);
        }
    }

    public static void processImages(MainForm mf, List<Image> imagesToProcess, Image backImage = null)
    {
        //If there are no images,
        if (imagesToProcess.Count < 1)
        {
            //Don't process any images
            return;
        }
        Size firstSize = imagesToProcess[0].Size;
        bool allSameSize = true;
        foreach (Image image in imagesToProcess)
        {
            if (image.Size != firstSize)
            {
                allSameSize = false;
                break;
            }
        }
        if (allSameSize && imagesToProcess.Count > 1)
        {
            //make it all one object
            if (backImage != null)
            {
                //make it a deck of cards
                GameObject gameObject = new GameObject(imagesToProcess, backImage);
                gameObject.moveTo(new Vector(100, 100), false);
                mf.addGameObject(gameObject);
            }
            else
            {
                //else make it an object with many states
                GameObject gameObject = new GameObject(imagesToProcess);
                gameObject.moveTo(new Vector(100, 100), false);
                mf.addGameObject(gameObject);
            }
        }
        else
        {
            ////make them separate objects
            //foreach (Image image in imagesToProcess)
            //{
            //    makeBin(image);
            //}
        }
    }
}
