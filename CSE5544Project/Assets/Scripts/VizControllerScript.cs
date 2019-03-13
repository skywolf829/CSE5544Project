using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VizControllerScript : MonoBehaviour
{
    public static VizControllerScript instance;
    public float moveSpeed = 1;

    public TextAsset wordEmbeddings, KGEmbeddings, corpus;

    KnowledgeGraphEmbeddingsVisualizer kgvis;
    WordEmbeddingsVisualizer wordvis;
    ParserVisualization parservis;

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

    public void TriggerPressed(object sender, VRTK.ControllerInteractionEventArgs e)
    {

    }

    public void TriggerReleased(object sender, VRTK.ControllerInteractionEventArgs e)
    {

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
