using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ParserVisualization : MonoBehaviour
{
    public static ParserVisualization instance;

    public GameObject ContentPane;
    public GameObject TextPrefab;

    List<string> predicates;
    List<string[]> entries;

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

    private void PreProcessData()
    {
        predicates = new List<string>();
        foreach(string[] entry in entries)
        {
            if (!predicates.Contains(entry[2]))
            {
                predicates.Add(entry[2]);
            }
        }
    }
    public List<string> CreateFilterFromPredicate(string s)
    {
        List<string> filters = new List<string>();
        foreach(string[] entry in entries)
        {
            if(entry[2] == s)
            {
                if(!filters.Contains(entry[0])) filters.Add(entry[0]);
                if(!filters.Contains(entry[1])) filters.Add(entry[1]);
            }
        }
        return filters;
    }
    public List<string> CreateFilterFromPredicate(int i)
    {
        List<string> filters = new List<string>();
        foreach (string[] entry in entries)
        {
            if (entry[2] == entries[i][2])
            {
                if(!filters.Contains(entry[0])) filters.Add(entry[0]);
                if (!filters.Contains(entry[1])) filters.Add(entry[1]);
            }
        }
        return filters;
    }
    void CreateUI()
    {

        ContentPane.GetComponent<RectTransform>().sizeDelta = new Vector2(
            ContentPane.GetComponent<RectTransform>().sizeDelta.x,
            predicates.Count * TextPrefab.GetComponent<RectTransform>().rect.height);

        for (int i = 0; i < predicates.Count; i++)
        {
            GameObject t = Instantiate(TextPrefab);
            t.transform.SetParent(ContentPane.transform, false);
            t.GetComponent<TextMeshProUGUI>().text = predicates[i];
            t.GetComponent<RectTransform>().localPosition = new Vector3(ContentPane.GetComponent<RectTransform>().rect.width / 2f, 
                 - i * t.GetComponent<RectTransform>().rect.height + t.GetComponent<RectTransform>().rect.height / 2f, -10);
        }
    }
    public IEnumerator SetData(TextAsset t)
    {
        yield return entries = DataImporter.LoadParserData(t);
        PreProcessData();
        CreateUI();
        loadedData = true;
    }

}
