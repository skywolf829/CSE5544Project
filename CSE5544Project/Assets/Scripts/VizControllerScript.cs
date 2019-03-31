using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using TMPro;
using UnityEngine.EventSystems;

public class VizControllerScript : MonoBehaviour
{
    
    public static VizControllerScript instance;
    public float moveSpeed = 1;

    public TextAsset wordEmbeddings, KGEmbeddings, corpus, topics;
    public TextAsset embeddingColors;
    public GameObject TMProPrefab;

    [HideInInspector]
    public List<string[]> entries;
    [HideInInspector]
    public List<string> topicsList;
    [HideInInspector]
    public List<Color> colorsList;
    [HideInInspector]
    public Dictionary<string, Color> embeddingColorsDict;

    KnowledgeGraphEmbeddingsVisualizer kgvis;
    WordEmbeddingsVisualizer wordvis;
    ParserVisualization parservis;

    bool holdingTrigger;
    GameObject selectionBubble;

    [HideInInspector]
    public List<string> currentFilters;
    [HideInInspector]
    public List<string> predicatesSelected;
    [HideInInspector]
    public List<Color> SOPColoring;

    VRTK_ControllerReference controllerRef;
    
    public Material transparentMat;

    bool visScaled = false;

    Vector3 startTriggerPos, startTriggerRot;
    
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
    // Start is called before the first frame update
    void Start()
    {
        predicatesSelected = new List<string>();

        kgvis = KnowledgeGraphEmbeddingsVisualizer.instance;
        wordvis = WordEmbeddingsVisualizer.instance;
        parservis = ParserVisualization.instance;


        entries = DataImporter.LoadParserData(corpus);
        colorsList = DataImporter.LoadColors(embeddingColors);
        topicsList = DataImporter.LoadTopics(topics);
        embeddingColorsDict = DataImporter.LoadEmbeddingColors(embeddingColors);

        if (!kgvis || !wordvis || !parservis)
        {
            print("Missing a visualization instance!");
            gameObject.SetActive(false);
        }
        else if(wordEmbeddings != null && KGEmbeddings != null && corpus != null)
        {
            wordvis.embeddings = DataImporter.LoadWord2VecEmbeddings(wordEmbeddings);
            kgvis.embeddings = DataImporter.LoadKGEmbeddings(KGEmbeddings);

            wordvis.PreProcessData();
            kgvis.PreProcessData();
            parservis.PreProcessData();

            StartCoroutine(wordvis.UpdateVisualization(null, false, 100));
            StartCoroutine(kgvis.UpdateVisualization(null, false, 100));
            StartCoroutine(parservis.CreateUI());

            StartCoroutine(VisibleTextFromHandPositions());
        }
        else
        {
            print("Missing TextAssets!");
        }
    }

    public void GripPressed(object o, ControllerInteractionEventArgs e)
    {
        if (!WordEmbeddingsVisualizer.instance.gameObject.GetComponent<BoxCollider>().bounds.Contains(e.controllerReference.actual.transform.position)
            && !KnowledgeGraphEmbeddingsVisualizer.instance.gameObject.GetComponent<BoxCollider>().bounds.Contains(e.controllerReference.actual.transform.position))
            return;

        controllerRef = e.controllerReference;
        print(e.controllerReference);
        selectionBubble = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        selectionBubble.transform.position = e.controllerReference.actual.transform.position;
        selectionBubble.transform.localScale = Vector3.one * 0.01f;
        if (transparentMat)
        {
            selectionBubble.GetComponent<Renderer>().material = transparentMat;
            selectionBubble.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.1f);
        }
    }
    public void GripReleased(object o, ControllerInteractionEventArgs e)
    {
        if (!selectionBubble) return;
        controllerRef = null;
        if (WordEmbeddingsVisualizer.instance.gameObject.GetComponent<BoxCollider>().bounds.Contains(selectionBubble.transform.position))
        {
            currentFilters = wordvis.GetFilterSelection(selectionBubble.transform.position, selectionBubble.transform.localScale.x / 2f);
            if (currentFilters != null)
            {
                visScaled = false;
                StartCoroutine(wordvis.UpdateVisualization(currentFilters, visScaled, 100));
                StartCoroutine(kgvis.UpdateVisualization(currentFilters, visScaled, 100));
            }
        }
        else if (KnowledgeGraphEmbeddingsVisualizer.instance.gameObject.GetComponent<BoxCollider>().bounds.Contains(selectionBubble.transform.position))
        {
            currentFilters = KnowledgeGraphEmbeddingsVisualizer.instance.GetFilterSelection(selectionBubble.transform.position, selectionBubble.transform.localScale.x / 2f);
            if (currentFilters != null)
            {
                visScaled = false;
                StartCoroutine(kgvis.UpdateVisualization(currentFilters, visScaled, 100));
                StartCoroutine(wordvis.UpdateVisualization(currentFilters, visScaled, 100));
            }
        }        
        Destroy(selectionBubble);
        selectionBubble = null;
    }
    public void TriggerPressed(object o, ControllerInteractionEventArgs e)
    {
        startTriggerPos = e.controllerReference.actual.transform.position;
        startTriggerRot = e.controllerReference.actual.transform.eulerAngles;
    }
    public void TriggerReleased(object o, ControllerInteractionEventArgs e)
    {
        if (Vector3.Distance(startTriggerPos, e.controllerReference.actual.transform.position) > 0.05f ||
            Vector3.Angle(startTriggerRot, e.controllerReference.actual.transform.eulerAngles) > 5f)
            return;
        RaycastHit[] rchs = Physics.RaycastAll(new Ray(e.controllerReference.actual.transform.position, e.controllerReference.actual.transform.forward), Mathf.Infinity);
        for(int i = 0; i < rchs.Length; i++)
        {
            if(rchs[i].collider.transform.tag == "Predicate")
            {
                string filterTerm = rchs[i].collider.GetComponent<TextMeshProUGUI>().text;
                predicatesSelected = new List<string>();
                predicatesSelected.Add(filterTerm);
                currentFilters = parservis.CreateFilterFromPredicate(filterTerm);
                print(currentFilters.Count);
                StartCoroutine(wordvis.UpdateVisualization(currentFilters, false, 100));
                StartCoroutine(kgvis.UpdateVisualization(currentFilters, false, 100));
            }
        }
    }
    public void ButtonOnePressed(object o, ControllerInteractionEventArgs e)
    {
        currentFilters = new List<string>();
        predicatesSelected = new List<string>();
        visScaled = false;
        StartCoroutine(wordvis.UpdateVisualization(currentFilters, visScaled, 100));
        StartCoroutine(kgvis.UpdateVisualization(currentFilters, visScaled, 100));
    }
    public void ButtonTwoPressed(object o, ControllerInteractionEventArgs e)
    {
        visScaled = !visScaled;
        StartCoroutine(wordvis.UpdateVisualization(currentFilters, visScaled, 100));
        StartCoroutine(kgvis.UpdateVisualization(currentFilters, visScaled, 100));
    }
    private void Update()
    {
        if (controllerRef != null)
        {
            selectionBubble.transform.localScale = Vector3.one * Vector3.Distance(controllerRef.actual.transform.position, selectionBubble.transform.position);
        }
    }

    public void TouchpadInput(object sender, ControllerInteractionEventArgs e)
    {
        if (!Camera.allCameras[0].gameObject) return;
        Transform playerCam = Camera.allCameras[0].transform;
        Vector3 forwardOnPlane = playerCam.forward - Vector3.Dot(playerCam.forward, Vector3.up) * Vector3.up;
        Vector3 rightOnPlane = playerCam.right - Vector3.Dot(playerCam.right, Vector3.up) * Vector3.up;
        GameObject.FindWithTag("Player").transform.Translate(
            (forwardOnPlane.normalized * e.touchpadAxis.y + 
            rightOnPlane.normalized * e.touchpadAxis.x)
            .normalized 
            * moveSpeed * e.touchpadAxis.magnitude * Time.deltaTime, Space.World);
    }
    
    IEnumerator VisibleTextFromHandPositions()
    {
        Dictionary<string, GameObject> currentWEtext = new Dictionary<string, GameObject>();
        Dictionary<string, GameObject> currentKGtext = new Dictionary<string, GameObject>();

        while (true)
        {
            if (!VRTK_DeviceFinder.GetControllerLeftHand()) { print("No left hand gameobject"); yield return new WaitForSeconds(0.1f); continue; }
            if (!VRTK_DeviceFinder.GetControllerRightHand()) { print("No right hand gameobject"); yield return new WaitForSeconds(0.1f); continue; }
            Transform leftController = VRTK_DeviceFinder.GetControllerLeftHand().transform;
            Transform rightController = VRTK_DeviceFinder.GetControllerRightHand().transform;
            float range = 0.15f;
            Dictionary<string, Vector3> KGDisplayedText = new Dictionary<string, Vector3>();
            Dictionary<string, Vector3> WEDisplayedText = new Dictionary<string, Vector3>();
            if (leftController)
            {
                if (WordEmbeddingsVisualizer.instance.gameObject.GetComponent<BoxCollider>().bounds.Contains(leftController.position))
                {
                    WEDisplayedText = wordvis.GetFilterSelectionDict(leftController.position, range);

                }
                else if (KnowledgeGraphEmbeddingsVisualizer.instance.gameObject.GetComponent<BoxCollider>().bounds.Contains(leftController.position))
                {
                    KGDisplayedText = kgvis.GetFilterSelectionDict(leftController.position, range);
                }
            }
            if (rightController)
            {
                if (WordEmbeddingsVisualizer.instance.gameObject.GetComponent<BoxCollider>().bounds.Contains(rightController.position))
                {
                    Dictionary<string, Vector3> moreTextToDisplay = wordvis.GetFilterSelectionDict(rightController.position, range);
                    foreach(KeyValuePair<string, Vector3> pair in moreTextToDisplay)
                    {
                        WEDisplayedText.Add(pair.Key, pair.Value);
                    }
                }
                else if (KnowledgeGraphEmbeddingsVisualizer.instance.gameObject.GetComponent<BoxCollider>().bounds.Contains(rightController.position))
                {
                    KGDisplayedText = kgvis.GetFilterSelectionDict(rightController.position, range);
                    Dictionary<string, Vector3> moreTextToDisplay = wordvis.GetFilterSelectionDict(rightController.position, range);
                    foreach (KeyValuePair<string, Vector3> pair in moreTextToDisplay)
                    {
                        KGDisplayedText.Add(pair.Key, pair.Value);
                    }
                }
            }
            if (WEDisplayedText.Count > 0)
            {
                List<string> itemsToDestroy = new List<string>();
                foreach (KeyValuePair<string, GameObject> pair in currentWEtext)
                {
                    if (!WEDisplayedText.ContainsKey(pair.Key))
                    {
                        itemsToDestroy.Add(pair.Key);
                    }
                    else
                    {
                        pair.Value.transform.LookAt(Camera.allCameras[0].transform.position + Camera.allCameras[0].transform.forward * 1000);
                    }
                }
                foreach (string s in itemsToDestroy)
                {
                    Destroy(currentWEtext[s]);
                    currentWEtext.Remove(s);
                }
                foreach (KeyValuePair<string, Vector3> pair in WEDisplayedText)
                {
                    if (!currentWEtext.ContainsKey(pair.Key) && currentWEtext.Count < 20)
                    {
                        GameObject g = Instantiate(TMProPrefab);
                        g.transform.position = pair.Value;
                        g.transform.LookAt(Camera.allCameras[0].transform.position + Camera.allCameras[0].transform.forward * 1000);
                        g.GetComponent<TextMeshPro>().text = pair.Key;
                        g.name = pair.Key;
                        g.transform.localScale = Vector3.one * 0.02f;
                        g.transform.position += new Vector3(0, 0.02f, 0);
                        currentWEtext.Add(pair.Key, g);
                    }
                }
            }
            else
            {
                foreach(KeyValuePair<string, GameObject> pair in currentWEtext)
                {
                    Destroy(pair.Value);
                }
                currentWEtext = new Dictionary<string, GameObject>();
            }
            if (KGDisplayedText.Count > 0)
            {
                List<string> itemsToDestroy = new List<string>();
                foreach (KeyValuePair<string, GameObject> pair in currentKGtext)
                {
                    if (!KGDisplayedText.ContainsKey(pair.Key))
                    {
                        itemsToDestroy.Add(pair.Key);
                    }
                    else
                    {
                        pair.Value.transform.LookAt(Camera.allCameras[0].transform.position + Camera.allCameras[0].transform.forward * 1000);
                    }
                }
                foreach (string s in itemsToDestroy)
                {
                    Destroy(currentKGtext[s]);
                    currentKGtext.Remove(s);
                }
                foreach (KeyValuePair<string, Vector3> pair in KGDisplayedText)
                {
                    if (!currentKGtext.ContainsKey(pair.Key) && currentKGtext.Count < 20)
                    {
                        GameObject g = Instantiate(TMProPrefab);
                        g.transform.position = pair.Value;
                        g.transform.LookAt(Camera.allCameras[0].transform.position + Camera.allCameras[0].transform.forward * 1000);
                        g.GetComponent<TextMeshPro>().text = pair.Key;
                        g.name = pair.Key;
                        g.transform.localScale = Vector3.one * 0.02f;
                        g.transform.position += new Vector3(0, 0.02f, 0);
                        currentKGtext.Add(pair.Key, g);
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<string, GameObject> pair in currentKGtext)
                {
                    Destroy(pair.Value);
                }
                currentKGtext = new Dictionary<string, GameObject>();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    
}
