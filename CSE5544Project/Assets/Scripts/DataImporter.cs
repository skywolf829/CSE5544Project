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
   
    public static Dictionary<string, float[]> LoadKGEmbeddings(TextAsset t)
    {
        Dictionary<string, float[]> embeddings2D =
          JsonConvert.DeserializeObject<Dictionary<string, float[]>>(t.text.Substring(1, t.text.Length - 2));
        return embeddings2D;
    }
    public static List<string[]> LoadParserData(TextAsset t)
    {
        List<string[]> data = new List<string[]>();

        string[] lines = t.text.Split(new char[] { '\n' });
        for(int i = 0; i < lines.Length; i++)
        {
            string[] SOPtriple = lines[i].Split(new char[] { '\t' });
            if(SOPtriple.Length != 3)
            {
                Debug.Log("Error on " + SOPtriple);
            }
            else
            {
                if (SOPtriple[0].Contains("'")) SOPtriple[0] = SOPtriple[0].Replace("\'", "");
                if (SOPtriple[1].Contains("'")) SOPtriple[1] = SOPtriple[1].Replace("\'", "");
                if (SOPtriple[2].Contains("'")) SOPtriple[2] = SOPtriple[2].Replace("\'", "");
                data.Add(SOPtriple);
            }
        }
        return data;
    }
    public static Dictionary<string, Color> LoadEmbeddingColors(TextAsset t)
    {
        Dictionary<string, string[]> colors =
          JsonConvert.DeserializeObject<Dictionary<string, string[]>>(t.text.Substring(1, t.text.Length - 2));
        Dictionary<string, Color> embeddingColors = new Dictionary<string, Color>();
        foreach(KeyValuePair<string, string[]> pair in colors)
        {
            Color c = new Color();
            ColorUtility.TryParseHtmlString(pair.Value[0], out c);
            embeddingColors.Add(pair.Key, c);
        }
        return embeddingColors;
    }
    public static List<string> LoadTopics(TextAsset t)
    {
        List<string> topics = new List<string>();
        string[] lines = t.text.Split(new char []{ '\n' });
        for(int i = 0; i < lines.Length; i++)
        {
            topics.Add(lines[i]);
        }
        return topics;
    }
    public static List<Color> LoadColors(TextAsset t)
    {
        List<Color> colors = new List<Color>();
        Dictionary<string, string[]> dict =
            JsonConvert.DeserializeObject<Dictionary<string, string[]>>(t.text.Substring(1, t.text.Length -2));
        foreach (KeyValuePair<string, string[]> pair in dict)
        {
            int index = int.Parse(pair.Value[1]);
            while (colors.Count -1 < index)
            {
                colors.Add(new Color());
            }
            Color c = new Color();
            ColorUtility.TryParseHtmlString(pair.Value[0], out c);
            colors[index] = c;
        }
        return colors;
    }
}
