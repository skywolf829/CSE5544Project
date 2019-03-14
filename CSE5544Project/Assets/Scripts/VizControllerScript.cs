using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
public class VizControllerScript : MonoBehaviour
{
    public static VizControllerScript instance;
    public float moveSpeed = 1;

    public TextAsset wordEmbeddings, KGEmbeddings, corpus;

    KnowledgeGraphEmbeddingsVisualizer kgvis;
    WordEmbeddingsVisualizer wordvis;
    ParserVisualization parservis;

    bool holdingTrigger;
    GameObject selectionBubble;

    List<string> currentFilters;

    VRTK_ControllerReference controllerRef;
    
    public Material transparentMat;

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
        kgvis = KnowledgeGraphEmbeddingsVisualizer.instance;
        wordvis = WordEmbeddingsVisualizer.instance;
        parservis = ParserVisualization.instance;

        if(!kgvis || !wordvis || !parservis)
        {
            print("Missing a visualization instance!");
            gameObject.SetActive(false);
        }
        else if(wordEmbeddings != null && KGEmbeddings != null && corpus != null)
        {
            StartCoroutine(wordvis.SetData(wordEmbeddings));
            StartCoroutine(kgvis.SetData(KGEmbeddings));
            //StartCoroutine(parservis.SetData(corpus));

            StartCoroutine(wordvis.InitVisualizationParticleSystem());
            StartCoroutine(kgvis.InitVisualizationParticleSystem());
        }
        else
        {
            print("Missing TextAssets!");
        }
    }

    public void TriggerPressed(object o, ControllerInteractionEventArgs e)
    {
        controllerRef = e.controllerReference;
        selectionBubble = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        selectionBubble.transform.position = e.controllerReference.actual.transform.position;
        selectionBubble.transform.localScale = Vector3.one * 0.01f;
        if (transparentMat)
        {
            selectionBubble.GetComponent<Renderer>().material = transparentMat;
            selectionBubble.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.1f);
        }
    }

    public void TriggerReleased(object o, ControllerInteractionEventArgs e)
    {
        controllerRef = null;
        if (WordEmbeddingsVisualizer.instance.gameObject.GetComponent<BoxCollider>().bounds.Contains(selectionBubble.transform.position))
        {
            currentFilters = WordEmbeddingsVisualizer.instance.GetFilterSelection(selectionBubble);
            print(currentFilters.Count);
        }
        else if (KnowledgeGraphEmbeddingsVisualizer.instance.gameObject.GetComponent<BoxCollider>().bounds.Contains(selectionBubble.transform.position))
        {

        }
        if(currentFilters != null) wordvis.SetFilter(currentFilters);
        Destroy(selectionBubble);
        selectionBubble = null;
    }
    private void Update()
    {
        if (controllerRef != null)
        {
            selectionBubble.transform.localScale = Vector3.one * Vector3.Distance(controllerRef.actual.transform.position, selectionBubble.transform.position);
        }
    }

    public void TouchpadInput(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        if (!Camera.main.gameObject) return;
        Transform playerCam = Camera.main.transform;
        Vector3 forwardOnPlane = playerCam.forward - Vector3.Dot(playerCam.forward, Vector3.up) * Vector3.up;
        Vector3 rightOnPlane = playerCam.right - Vector3.Dot(playerCam.right, Vector3.up) * Vector3.up;
        GameObject.FindWithTag("Player").transform.Translate(
            (forwardOnPlane.normalized * e.touchpadAxis.y + 
            rightOnPlane.normalized * e.touchpadAxis.x)
            .normalized 
            * moveSpeed * e.touchpadAxis.magnitude * Time.time, Space.World);
    }
}
