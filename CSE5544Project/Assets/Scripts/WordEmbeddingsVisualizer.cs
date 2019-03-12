using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordEmbeddingsVisualizer : MonoBehaviour
{
    public static WordEmbeddingsVisualizer instance;

    Dictionary<string, Vector2> embeddings;
    bool loadedData = false;

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
        loadedData = true;
    }

    public IEnumerator InitVisualization()
    {
        while (!loadedData) yield return null;

        foreach(KeyValuePair<string, Vector2> pair in embeddings)
        {
            print(pair.ToString());
        }
    }
}
