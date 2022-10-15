using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class ItemDB
{
    public static void SaveToJSON<CustomTile>(string filename)
    {
        Debug.Log(GetPath(filename));

        List<CustomTile> toSave = Resources.LoadAll("Tiles", typeof(CustomTile)).Cast<CustomTile>().ToList();

        string content = JsonHelper.ToJson<CustomTile>(toSave.ToArray());
        WriteFile(GetPath(filename), content);
    }

    public static void SaveToJSON<CustomTile>(CustomTile toSave, string filename)
    {
        string content = JsonUtility.ToJson(toSave);
        WriteFile(GetPath(filename), content);
    }

    public static List<CustomTile> ReadListFromJSON<CustomTile>(string filename)
    {
        string content = ReadFile(GetPath(filename));

        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            return new List<CustomTile>();
        }

        List<CustomTile> res = JsonHelper.FromJson<CustomTile>(content).ToList();

        return res;

    }

    public static CustomTile ReadFromJSON<CustomTile>(string filename)
    {
        string content = ReadFile(GetPath(filename));

        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            return default(CustomTile);
        }

        CustomTile res = JsonUtility.FromJson<CustomTile>(content);

        return res;

    }

    private static string GetPath(string filename)
    {
        return Application.persistentDataPath + "/" + filename;
    }

    private static void WriteFile(string path, string content)
    {
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(content);
        }
    }

    private static string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }
        return "";
    }
}

public static class JsonHelper
{
    public static CustomTile[] FromJson<CustomTile>(string json)
    {
        Wrapper<CustomTile> wrapper = JsonUtility.FromJson<Wrapper<CustomTile>>(json);
        return wrapper.Items;
    }

    public static string ToJson<CustomTile>(CustomTile[] array)
    {
        Wrapper<CustomTile> wrapper = new Wrapper<CustomTile>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<CustomTile>(CustomTile[] array, bool prettyPrint)
    {
        Wrapper<CustomTile> wrapper = new Wrapper<CustomTile>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<CustomTile>
    {
        public CustomTile[] Items;
    }
}