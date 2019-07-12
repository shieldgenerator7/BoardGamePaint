using BoardGamePaint;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

public static class ObjectImportManager
{

    public static bool isFileJSON(string filename)
        => filename.ToLower().EndsWith(".txt")
        || filename.ToLower().EndsWith(".json");

    public static void importObject(MainForm mf, string filename)
    {
        List<GameObject> objectsToProcess = new List<GameObject>();
        if (isFileJSON(filename))
        {
            string foldername = filename.Substring(0, filename.LastIndexOf("\\") + 1);
            //parse the JSON file
            JObject jo = JObject.Parse(File.ReadAllText(filename));
            foreach (JToken joImage in jo["images"])
            {
                string imageFilename = "default.png";
                string description = null;
                int cardCount = 1;
                foreach (JToken jotkn in joImage.Children())
                {
                    imageFilename = foldername + (string)jotkn.SelectToken("image");
                    description = (string)jotkn.SelectToken("description");
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
                    GameObject gameObject = new GameObject(image, description);
                    objectsToProcess.Add(gameObject);
                }
            }
            string backFilename = foldername + (string)jo["back"];
            Trace.WriteLine("jo back: " + backFilename);
            Image backImage = (File.Exists(backFilename))
                ? Image.FromFile(backFilename)
                : null;
            string objectDesc = (string)jo["description"];
            processImages(mf, objectsToProcess, backImage, objectDesc);
        }
    }

    public static void processImages(MainForm mf, List<GameObject> objectsToProcess, Image backImage = null, string description = null)
    {
        //If there are no images,
        if (objectsToProcess.Count < 1)
        {
            //Don't process any images
            return;
        }
        Size firstSize = objectsToProcess[0].Size;
        bool allSameSize = true;
        foreach (GameObject go in objectsToProcess)
        {
            if (go.Size != firstSize)
            {
                allSameSize = false;
                break;
            }
        }
        if (allSameSize && objectsToProcess.Count > 1)
        {
            //make it all one object
            if (backImage != null)
            {
                //make it a deck of cards
                foreach(GameObject gameObject in objectsToProcess)
                {
                    gameObject.Back = backImage;
                }
                GameObject cardDeck = new CardDeck(objectsToProcess, backImage, description);
                cardDeck.moveTo(new Vector(100, 100), false);
                mf.addGameObject(cardDeck);
            }
            else
            {
                //else make it an object with many states
                List<Image> images = new List<Image>(
                    from go in objectsToProcess
                    select go.image
                    );

                GameObject gameObject = new GameObject(images);
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
