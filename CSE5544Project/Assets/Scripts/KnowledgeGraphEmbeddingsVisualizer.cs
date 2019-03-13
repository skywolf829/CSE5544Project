using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeGraphEmbeddingsVisualizer : MonoBehaviour
{
    public static KnowledgeGraphEmbeddingsVisualizer instance;

    Dictionary<string, Vector2> embeddings;

    private void Awake()
    {
        if(instance == null)
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
        yield return embeddings = DataImporter.LoadKGEmbeddings2D(t);
    }
}
