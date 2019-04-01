using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ParserVisualization : MonoBehaviour
{
    public static ParserVisualization instance;

    public GameObject ContentPane;
    public GameObject TextPrefab, TextPrefabWithImage;
    public GameObject TopicsPane;

    List<string> predicates;
    List<GameObject> predicateObjects;

    bool loadedData = false;
    bool loadedColors = false;
    bool loadedTopics = false;
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

    public void PreProcessData()
    {
        predicates = new List<string>();
        foreach(string[] entry in VizControllerScript.instance.entries)
        {
            if (!predicates.Contains(entry[2]))
            {
                predicates.Add(entry[2]);
            }
        }
        loadedData = true;
        loadedColors = true;
        loadedTopics = true;
    }
    public IEnumerator CreateFilterFromPredicate(string s, int steps = 10000)
    {
        List<string> filters = new List<string>();
        List<Color> colors = new List<Color>();

        int i = 0;
        foreach (string[] entry in VizControllerScript.instance.entries)
        {
            if(entry[2] == s)
            {
                if (!filters.Contains(entry[0]))
                {
                    filters.Add(entry[0]);
                    colors.Add(Color.blue);
                }
                if (!filters.Contains(entry[1]))
                {
                    filters.Add(entry[1]);
                    colors.Add(Color.red);
                }
            }
            if (steps != 0 && i % steps == 0)
            {
                LoadingManager.instance.SetProgress("CreateFilterFromPredicate_"+s, 
                    (float)i / VizControllerScript.instance.entries.Count, "Determining filters based on " + s);
                yield return null;
            }
            i++;
        }
        VizControllerScript.instance.SOPColoring = colors;
        VizControllerScript.instance.UpdateFilters(filters);
        LoadingManager.instance.FinishedLoading("CreateFilterFromPredicate_" + s);
        yield return null;
    }
    public IEnumerator CreateFilterFromPredicate(int spot, int steps = 10000)
    {
        int i = 0;
        List<string> filters = new List<string>();
        List<Color> colors = new List<Color>();
        foreach (string[] entry in VizControllerScript.instance.entries)
        {
            if (entry[2] == VizControllerScript.instance.entries[spot][2])
            {
                if (!filters.Contains(entry[0]))
                {
                    filters.Add(entry[0]);
                    colors.Add(Color.blue);
                }
                if (!filters.Contains(entry[1]))
                {
                    filters.Add(entry[1]);
                    colors.Add(Color.red);
                }
            }
            if (steps != 0 && i % steps == 0)
            {
                LoadingManager.instance.SetProgress("CreateFilterFromPredicate_"+i,
                    (float)i / VizControllerScript.instance.entries.Count, "Determining filters based on index" + i);
                yield return null;
            }
            i++;
        }
        VizControllerScript.instance.SOPColoring = colors;
        VizControllerScript.instance.UpdateFilters(filters);
        LoadingManager.instance.FinishedLoading("CreateFilterFromPredicate_" + i);
        yield return null;
    }
    public IEnumerator CreateUI()
    {
        predicateObjects = new List<GameObject>();
        while (!loadedData || !loadedTopics || !loadedColors) yield return null;

        ContentPane.GetComponent<RectTransform>().sizeDelta = new Vector2(
            ContentPane.GetComponent<RectTransform>().sizeDelta.x,
            predicates.Count * TextPrefab.GetComponent<RectTransform>().rect.height);

        for (int i = 0; i < predicates.Count; i++)
        {
            GameObject t = Instantiate(TextPrefab);
            t.transform.SetParent(ContentPane.transform, false);
            t.GetComponent<TextMeshProUGUI>().text = predicates[i];
            t.GetComponent<RectTransform>().localPosition = new Vector3(ContentPane.GetComponent<RectTransform>().rect.width / 2f, 
                 - i * t.GetComponent<RectTransform>().rect.height - t.GetComponent<RectTransform>().rect.height / 2f, -10);
            predicateObjects.Add(t);
        }

        TopicsPane.GetComponent<RectTransform>().sizeDelta = new Vector2(
            TopicsPane.GetComponent<RectTransform>().sizeDelta.x,
            VizControllerScript.instance.topicsList.Count * TextPrefab.GetComponent<RectTransform>().rect.height);
        for(int i = 0; i < VizControllerScript.instance.topicsList.Count && i < VizControllerScript.instance.colorsList.Count; i++)
        {
            GameObject t = Instantiate(TextPrefabWithImage);
            t.transform.SetParent(TopicsPane.transform, false);
            t.transform.GetChild(0).GetComponent<Image>().color = VizControllerScript.instance.colorsList[i];
            t.GetComponent<TextMeshProUGUI>().text = VizControllerScript.instance.topicsList[i];
            t.GetComponent<RectTransform>().localPosition = new Vector3(TopicsPane.GetComponent<RectTransform>().rect.width / 2f,
                -i * t.GetComponent<RectTransform>().rect.height - t.GetComponent<RectTransform>().rect.height / 2f, 0);

        }
        StartCoroutine(UpdateUI());
    }
    public IEnumerator UpdateUI(float timeStep = 0.1f)
    {
        float lastTime = Time.time;
        Vector3[] BoundingCorners = new Vector3[4];
        ContentPane.transform.parent.parent.GetComponent<RectTransform>().GetWorldCorners(BoundingCorners);

        Vector3 center = new Vector3();
        Vector3 size = new Vector3();
        Vector3 mins = new Vector3();
        Vector3 maxs = new Vector3();
        mins = BoundingCorners[0];
        maxs = BoundingCorners[0];
        for (int i = 0; i < BoundingCorners.Length; i++)
        {
            center += BoundingCorners[i];
            print(BoundingCorners[i].x + " " + BoundingCorners[i].y + " " + BoundingCorners[i].z);
            if (mins.x > BoundingCorners[i].x) mins = new Vector3(BoundingCorners[i].x, mins.y, mins.z);
            else if (maxs.x < BoundingCorners[i].x) maxs = new Vector3(BoundingCorners[i].x, maxs.y, maxs.z);
            if (mins.y > BoundingCorners[i].y) mins = new Vector3(mins.x, BoundingCorners[i].y, mins.z);
            else if (maxs.y < BoundingCorners[i].y) maxs = new Vector3(maxs.x, BoundingCorners[i].y, maxs.z);
            if (mins.z > BoundingCorners[i].z) mins = new Vector3(mins.x, mins.y, BoundingCorners[i].z);
            else if (maxs.z < BoundingCorners[i].z) maxs = new Vector3(maxs.x, maxs.y, BoundingCorners[i].z);
        }
        center /= 4f;
        size = maxs - mins;
        Bounds parentBounds = new Bounds(center, size);
        while (true)
        {
            if (Time.time - lastTime > timeStep)
            {               
                for (int i = 0; i < predicateObjects.Count; i++)
                {
                    Vector3[] corners = new Vector3[4];
                    predicateObjects[i].GetComponent<RectTransform>().GetWorldCorners(corners);
                    if (parentBounds.Contains(corners[0]) || parentBounds.Contains(corners[2]))
                    {
                        predicateObjects[i].SetActive(true);
                    }
                    else
                    {
                        predicateObjects[i].SetActive(false);
                    }
                }
                lastTime = Time.time;
            }
            yield return null;
        }
    }
}
