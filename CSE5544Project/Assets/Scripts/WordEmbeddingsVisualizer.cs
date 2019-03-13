using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordEmbeddingsVisualizer : MonoBehaviour
{
    public float width = 2f;
    public GameObject TMProPrefab;

    public static WordEmbeddingsVisualizer instance;

    Dictionary<string, float[]> embeddings;
    bool loadedData = false;
    float[] minValues, maxValues;
    ParticleSystem.Particle[] particles;

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
        PreProcessData();
        loadedData = true;
    }
    private void PreProcessData()
    {
        foreach (KeyValuePair<string, float[]> pair in embeddings)
        {
            if (minValues == null)
            {
                minValues = new float[pair.Value.Length];
            }
            if (maxValues == null)
            {
                maxValues = new float[pair.Value.Length];
            }
            for (int i = 0; i < pair.Value.Length; i++)
            {
                minValues[i] = Mathf.Min(minValues[i], pair.Value[i]);
                maxValues[i] = Mathf.Max(maxValues[i], pair.Value[i]);
            }
        }
        if(maxValues.Length == 2) transform.localScale = new Vector3(width / (maxValues[0] - minValues[0]), width / (maxValues[1] - minValues[1]));
        else if (maxValues.Length == 3) transform.localScale = new Vector3(width / (maxValues[0] - minValues[0]), width / (maxValues[1] - minValues[1]), width / (maxValues[2] - minValues[2]));

    }
    public IEnumerator InitVisualizationGameObjects()
    {
        while (!loadedData) yield return null;

        foreach(KeyValuePair<string, float[]> pair in embeddings)
        {
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            g.transform.localScale = Vector3.one * 0.1f;

            if(pair.Value.Length == 2)
            {
                g.transform.position = new Vector2(pair.Value[0], pair.Value[1]);
                g.GetComponent<Renderer>().material.color = Color.HSVToRGB(Mathf.InverseLerp(minValues[0], maxValues[0], pair.Value[0]), 
                    Mathf.InverseLerp(minValues[1], maxValues[1], pair.Value[1]), 1);
            }
            else if(pair.Value.Length == 3)
            {
                g.transform.position = new Vector3(pair.Value[0], pair.Value[1], pair.Value[2]);
                g.GetComponent<Renderer>().material.color = Color.HSVToRGB(Mathf.InverseLerp(minValues[0], maxValues[0], pair.Value[0]),
                    Mathf.InverseLerp(minValues[1], maxValues[1], pair.Value[1]),
                    Mathf.InverseLerp(minValues[2], maxValues[2], pair.Value[2]));
            }
            g.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            g.GetComponent<Renderer>().receiveShadows = false;
            g.transform.SetParent(transform, false);
        }
    }
    public IEnumerator InitVisualizationParticleSystem()
    {
        while (!loadedData) yield return null;
        ParticleSystem.MainModule mainModule = GetComponent<ParticleSystem>().main;
        mainModule.maxParticles = embeddings.Count;
        particles = new ParticleSystem.Particle[embeddings.Count];
        int i = 0;
        foreach (KeyValuePair<string, float[]> pair in embeddings)
        {
            if (TMProPrefab)
            {
                GameObject text = GameObject.Instantiate(TMProPrefab);
                text.GetComponent<TextMeshPro>().text = pair.Key;
                text.transform.position = new Vector3(pair.Value[0], pair.Value[1] + text.GetComponent<TextMeshPro>().fontSize, pair.Value.Length > 2 ? pair.Value[2] : 0);
                text.transform.SetParent(transform, false);
            }

            particles[i] = new ParticleSystem.Particle();
            if (pair.Value.Length == 2)
            {
                particles[i].position = new Vector2(pair.Value[0], pair.Value[1]);
                particles[i].startColor = Color.HSVToRGB(Mathf.InverseLerp(minValues[0], maxValues[0], pair.Value[0]),
                    Mathf.InverseLerp(minValues[1], maxValues[1], pair.Value[1]), 1);
            }
            else if (pair.Value.Length == 3)
            {
                particles[i].position = new Vector3(pair.Value[0], pair.Value[1], pair.Value[2]);
                particles[i].startColor = new Color(Mathf.InverseLerp(minValues[0], maxValues[0], pair.Value[0]),
                    Mathf.InverseLerp(minValues[1], maxValues[1], pair.Value[1]),
                    Mathf.InverseLerp(minValues[2], maxValues[2], pair.Value[2]));
            }
            particles[i].rotation3D = Vector3.zero;
            particles[i].startSize = 0.25f;
            i++;
            //yield return null;
        }
        GetComponent<ParticleSystem>().SetParticles(particles, particles.Length);
    }
}
