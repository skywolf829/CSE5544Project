using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class DataImporter
{
    public static Dictionary<string, float[]> LoadWord2VecEmbeddings(TextAsset t)
    {
        Dictionary<string, float[]> embeddings2D =
          JsonConvert.DeserializeObject<Dictionary<string, float[]>>(t.text.Substring(1, t.text.Length - 2));
        return embeddings2D;
    }
    public static Dictionary<string, Vector2> LoadWord2VecEmbeddings2D(TextAsset t)
    {
        Dictionary<string, string> embeddings =
           JsonConvert.DeserializeObject<Dictionary<string, string>>(t.text.Substring(1, t.text.Length - 2));

        Dictionary<string, Vector2> embeddings2D =
            new Dictionary<string, Vector2>();
        foreach(KeyValuePair<string, string> pair in embeddings)
        {
            string[] split = pair.Value.Split(new char[] { ',' });
            Vector2 v = new Vector2(float.Parse(split[0]), float.Parse(split[1]));
            embeddings2D.Add(pair.Key, v);
        }
        return embeddings2D;
    }
    public static Dictionary<string, Vector3> LoadWord2VecEmbeddings3D(TextAsset t)
    {
        Dictionary<string, string> embeddings =
           JsonConvert.DeserializeObject<Dictionary<string, string>>(t.text.Substring(1, t.text.Length - 2));

        Dictionary<string, Vector3> embeddings2D =
            new Dictionary<string, Vector3>();
        foreach (KeyValuePair<string, string> pair in embeddings)
        {
            string[] split = pair.Value.Split(new char[] { ',' });
            Vector2 v = new Vector3(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));
            embeddings2D.Add(pair.Key, v);
        }
        return embeddings2D;
    }
    public static Dictionary<string, float[]> LoadKGEmbeddings(TextAsset t)
    {
        Dictionary<string, float[]> embeddings2D =
          JsonConvert.DeserializeObject<Dictionary<string, float[]>>(t.text.Substring(1, t.text.Length - 2));
        return embeddings2D;
    }
    public static Dictionary<string, Vector2> LoadKGEmbeddings2D(TextAsset t)
    {
        Dictionary<string, string> embeddings =
           JsonConvert.DeserializeObject<Dictionary<string, string>>(t.text.Substring(1,t.text.Length - 2));

        Dictionary<string, Vector2> embeddings2D =
            new Dictionary<string, Vector2>();
        foreach (KeyValuePair<string, string> pair in embeddings)
        {
            string[] split = pair.Value.Split(new char[] { ',' });
            Vector2 v = new Vector2(float.Parse(split[0]), float.Parse(split[1]));
            embeddings2D.Add(pair.Key, v);
        }
        return embeddings2D;
    }
    public static Dictionary<string, Vector3> LoadKGEmbeddings3D(TextAsset t)
    {
        Dictionary<string, string> embeddings =
           JsonConvert.DeserializeObject<Dictionary<string, string>>(t.text.Substring(1, t.text.Length - 2));

        Dictionary<string, Vector3> embeddings2D =
            new Dictionary<string, Vector3>();
        foreach (KeyValuePair<string, string> pair in embeddings)
        {
            string[] split = pair.Value.Split(new char[] { ',' });
            Vector2 v = new Vector3(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));
            embeddings2D.Add(pair.Key, v);
        }
        return embeddings2D;
    }
    public static List<List<string>> LoadParserData(TextAsset t)
    {
        List<List<string>> data = new List<List<string>>();

        return data;
    }
}
