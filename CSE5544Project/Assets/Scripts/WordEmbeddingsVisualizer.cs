using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordEmbeddingsVisualizer : MonoBehaviour
{
    public static WordEmbeddingsVisualizer instance;

    Dictionary<string, float[]> embeddings;
    bool loadedData = false;
    float[] minValues, maxValues;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);

        }
    }

    public IEnumerator SetData(TextAsset t)
    {
        yield return embeddings = DataImporter.LoadWord2VecEmbeddings(t);
        PreProcessData();
        loadedData = true;
    }
    private void PreProcessData()
    {
        foreach (KeyValuePair<string, float[]> pair in embeddings)
        {
            if (minValues == null)
            {
                minValues = new float[pair.Value.Length];
            }
            if (maxValues == null)
            {
                maxValues = new float[pair.Value.Length];
            }
            for (int i = 0; i < pair.Value.Length; i++)
            {
                minValues[i] = Mathf.Min(minValues[i], pair.Value[i]);
                maxValues[i] = Mathf.Max(maxValues[i], pair.Value[i]);
            }
        }
    }
    public IEnumerator InitVisualization()
    {
        while (!loadedData) yield return null;

        foreach(KeyValuePair<string, float[]> pair in embeddings)
        {
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            g.transform.localScale = Vector3.one * 0.1f;

            if(pair.Value.Length == 2)
            {
                g.transform.position = new Vector2(pair.Value[0], pair.Value[1]);
                g.GetComponent<Renderer>().material.color = Color.HSVToRGB(Mathf.InverseLerp(minValues[0], maxValues[0], pair.Value[0]), 
                    Mathf.InverseLerp(minValues[1], maxValues[1], pair.Value[1]), 1);
            }
            else if(pair.Value.Length == 3)
            {
                g.transform.position = new Vector3(pair.Value[0], pair.Value[1], pair.Value[2]);
                g.GetComponent<Renderer>().material.color = Color.HSVToRGB(Mathf.InverseLerp(minValues[0], maxValues[0], pair.Value[0]),
                    Mathf.InverseLerp(minValues[1], maxValues[1], pair.Value[1]),
                    Mathf.InverseLerp(minValues[2], maxValues[2], pair.Value[2]));
            }


            g.transform.parent = transform;
        }
    }
}
