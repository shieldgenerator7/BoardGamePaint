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

    public static string getRelativeFileName(string filename)
    {
        if (filename == null)
        {
            return null;
        }
        string[] split = filename.Split('\\');
        return split[split.Length - 1];
    }

    public static string getReadableFileName(string filename)
    {
        if (filename == null)
        {
            return null;
        }
        filename = getRelativeFileName(filename).Split('.')[0];
        if (filename.Contains("["))
        {
            filename = filename.Substring(0, filename.LastIndexOf("["));
        }
        return filename;
    }

    public static string getParentFolderName(string filename)
    {
        string[] split = filename.Split('\\');
        return split[split.Length - 2];
    }

    public static string getFolderPathName(string filename)
    {
        int lastSlash = filename.LastIndexOf("\\");
        return filename.Substring(0, lastSlash);
    }

    public static int getQuantityFromFileName(string filename)
    {
        int quantity = 1;
        if (filename.Contains("[") && filename.Contains("]"))
        {
            int iLeft = filename.LastIndexOf("[");
            int iRight = filename.LastIndexOf("]");
            bool parsed = int.TryParse(
                filename.Substring(iLeft + 1, iRight - iLeft - 1),
                out quantity
                );
            if (quantity < 1 && !parsed)
            {
                quantity = 1;
            }
        }
        return quantity;
    }

    public static void importObjects(MainForm mf, IEnumerable<string> filenames)
    {
        List<GameObject> objectsToProcess = new List<GameObject>();
        string backImageFileName = null;
        foreach (string filename in filenames)
        {
            if (isFileJSON(filename))
            {
                importJSONObject(mf, filename);
            }
            else if (filename.ToLower().Contains("[back]"))
            {
                backImageFileName = filename;
            }
            else
            {
                int cardCount = getQuantityFromFileName(filename);
                Image image = Image.FromFile(filename);
                for (int i = 0; i < cardCount; i++)
                {
                    objectsToProcess.Add(new GameObject(image, getReadableFileName(filename)));
                }
            }
        }
        Image backImage = (backImageFileName != null)
            ? Image.FromFile(backImageFileName)
            : null;        
        processObjects(mf, objectsToProcess, backImage, getReadableFileName(backImageFileName));
        if (objectsToProcess.Count > 1 && backImageFileName != null)
        {
            writeJSONDeck(filenames, backImageFileName);
        }
    }

    public static void importObject(MainForm mf, string filename)
    {
        if (isFileJSON(filename))
        {
            importJSONObject(mf, filename);
        }
    }

    static void importJSONObject(MainForm mf, string filename)
    {
        List<GameObject> objectsToProcess = new List<GameObject>();
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
        processObjects(mf, objectsToProcess, backImage, objectDesc);
    }

    public static void processObjects(MainForm mf, List<GameObject> objectsToProcess, Image backImage = null, string description = null)
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
                makeDeck(mf, objectsToProcess, backImage, description);
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
            //make them separate objects
            foreach (GameObject gameobject in objectsToProcess)
            {
                mf.binManager.makeBin(gameobject);
            }
        }
    }

    static void makeDeck(MainForm mf, List<GameObject> objects, Image backImage, string description)
    {
        foreach (GameObject gameObject in objects)
        {
            gameObject.Back = backImage;
        }
        GameObject cardDeck = new CardDeck(objects, backImage, description);
        cardDeck.moveTo(new Vector(100, 100), false);
        mf.addGameObject(cardDeck);
    }

    static void writeJSONDeck(IEnumerable<string> filenames, string backFilename)
    {
        string deckName = getReadableFileName(backFilename);
        string json = "{" + "\n"
            + "\"name\": \"" + deckName + "\"," + "\n"
            + "\"type\": \"deck\"," + "\n"
            + "\"description\": \"" + deckName + "\"," + "\n"
            + "\"back\": \"" + getRelativeFileName(backFilename) + "\"," + "\n"
            + "\"images\": {" + "\n";
        foreach (string filename in filenames)
        {
            if (filename != backFilename)
            {
                string cardName = getReadableFileName(filename);
                json += "\"" + cardName + "\":{" + "\n"
                    + "\"image\": \"" + getRelativeFileName(filename) + "\"," + "\n"
                    + "\"description\": \"" + cardName + "\"," + "\n"
                    + "\"quantity\": " + getQuantityFromFileName(filename) + "\n"
                    + "}," + "\n";
            }
        }
        //TODO: remove ending comma
        json += "}" + "\n"//end "images"
            + "}";//end json

        //Write json to file
        string folder = getFolderPathName(backFilename);
        string jsonFileName = folder + "\\" + deckName + ".json";
        File.WriteAllText(jsonFileName, json);
    }
}
