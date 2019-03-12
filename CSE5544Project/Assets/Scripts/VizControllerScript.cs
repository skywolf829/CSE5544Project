using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VizControllerScript : MonoBehaviour
{
    public static VizControllerScript instance;

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
            //StartCoroutine(kgvis.SetData(KGEmbeddings));
            //StartCoroutine(parservis.SetData(corpus));

            StartCoroutine(wordvis.InitVisualization());
        }
        else
        {
            print("Missing TextAssets!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
