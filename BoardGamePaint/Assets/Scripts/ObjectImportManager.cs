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

    public static void importObjects(IEnumerable<string> filenames)
    {
        List<GameObject> objectsToProcess = new List<GameObject>();
        string backImageFileName = null;
        foreach (string filename in filenames)
        {
            if (isFileJSON(filename))
            {
                importJSONObject(filename);
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
                    GameObject gameObject = new GameObject(image, getReadableFileName(filename));
                    gameObject.FileName = filename;
                    objectsToProcess.Add(gameObject);
                }
            }
        }
        GameObject backImageObject = (backImageFileName != null)
            ? new GameObject(backImageFileName)
            : null;
        processObjects(objectsToProcess, backImageObject, getReadableFileName(backImageFileName));
        if (objectsToProcess.Count > 1 && backImageFileName != null)
        {
            writeJSONDeck(filenames, backImageFileName);
        }
    }

    public static void importObject(string filename)
    {
        if (isFileJSON(filename))
        {
            importJSONObject(filename);
        }
    }

    static void importJSONObject(string filename)
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
                gameObject.FileName = imageFilename;
                objectsToProcess.Add(gameObject);
            }
        }
        //Description
        string objectDesc = (string)jo["description"];
        //Type
        string objectType = ((string)jo["type"])?.ToLower();
        GameObject backImageObject = null;
        string backFilename = foldername + (string)jo["back"];
        Trace.WriteLine("jo back: " + backFilename);
        if (objectType == "deck"
            || objectType == null && File.Exists(backFilename))
        {
            backImageObject = new GameObject(backFilename);
            makeDeck(objectsToProcess, Image.FromFile(backFilename), objectDesc);
        }
        else
        {
            makeDie(objectsToProcess, objectDesc);
        }
    }

    public static void processObjects(List<GameObject> objectsToProcess, GameObject backImageObject = null, string description = null)
    {
        //If there are no objects,
        if (objectsToProcess.Count < 1)
        {
            //Don't process any objects
            return;
        }
        if (allObjectsSameSize(objectsToProcess)
            && objectsToProcess.Count > 1)
        {
            //make it all one object
            if (backImageObject)
            {
                //make it a deck of cards
                makeDeck(objectsToProcess, backImageObject.image, description);
            }
            else
            {
                //else make it an object with many states
                makeDie(objectsToProcess, description);
            }
        }
        else
        {
            //make them separate objects
            foreach (GameObject gameobject in objectsToProcess)
            {
                Managers.Bin.makeBin(gameobject);
            }
        }
    }

    static void makeDeck(List<GameObject> objects, Image backImage, string description)
    {
        List<Card> cards = new List<Card>();
        foreach (GameObject gameObject in objects)
        {
            cards.Add(new Card(gameObject, backImage));
        }
        CardDeck cardDeck = new CardDeck(cards, backImage, description);
        Managers.Bin.makeBin(cardDeck);
    }

    static void makeDie(List<GameObject> objects, string description)
    {
        List<Image> images = new List<Image>(
            from go in objects
            select go.image
            );
        Die die = new Die(images, description);
        Managers.Bin.makeBin(die);
    }

    static bool allObjectsSameSize(List<GameObject> objectsToProcess)
    {
        Size firstSize = objectsToProcess[0].Size;
        foreach (GameObject go in objectsToProcess)
        {
            if (go.Size != firstSize)
            {
                return false;
            }
        }
        return true;
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
