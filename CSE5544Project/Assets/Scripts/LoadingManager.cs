using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;

    public GameObject UICanvas;
    public GameObject loadingBar;

    float topYPosition;
    float fullSize, startYScale;
    Vector2 startPos;
    Vector2 loadingBarStartPos;

    Dictionary<string, GameObject> loadingBars;

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
    // Start is called before the first frame update
    void Start()
    {
        fullSize = loadingBar.transform.GetChild(0).GetComponent<RectTransform>().rect.width;
        startYScale = loadingBar.transform.GetChild(0).GetComponent<RectTransform>().rect.height;
        startPos = loadingBar.transform.GetChild(0).GetComponent<RectTransform>().localPosition;
        loadingBarStartPos = loadingBar.GetComponent<RectTransform>().localPosition;

        UICanvas.SetActive(false);

        loadingBars = new Dictionary<string, GameObject>();
    }
    
    public void SetProgress(string processName, float percent, string info = null)
    {
        if (!loadingBars.ContainsKey(processName))
        {
            GameObject g = Instantiate(loadingBar);
            g.transform.SetParent(UICanvas.transform, false);
            g.GetComponent<RectTransform>().localPosition = loadingBarStartPos - new Vector2(0, loadingBars.Count * loadingBar.GetComponent<RectTransform>().rect.height);
            loadingBars.Add(processName, g);
        }

        UICanvas.SetActive(true);
        if(info != null)
        {
            loadingBars[processName].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = info;
        }
        loadingBars[processName].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(fullSize * percent, startYScale);
        loadingBars[processName].transform.GetChild(0).GetComponent<RectTransform>().localPosition = startPos + new Vector2((percent - 1) * (fullSize / 2f), 0);
    }
    public void FinishedLoading(string processName)
    {
        if (loadingBars.ContainsKey(processName))
        {
            Destroy(loadingBars[processName]);
            loadingBars.Remove(processName);
        }
        if(loadingBars.Count == 0)
            UICanvas.SetActive(false);
        int i = 0;
        foreach(KeyValuePair<string, GameObject> pair in loadingBars)
        {
            pair.Value.GetComponent<RectTransform>().localPosition = loadingBarStartPos - new Vector2(0, i * loadingBar.GetComponent<RectTransform>().rect.height);
            i++;
        }
    }
}
