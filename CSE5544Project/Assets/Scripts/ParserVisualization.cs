using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParserVisualization : MonoBehaviour
{
    public static ParserVisualization instance;

    List<string> headers;
    List<List<string>> entries;
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
        yield return entries = DataImporter.LoadParserData(t);
    }
}
