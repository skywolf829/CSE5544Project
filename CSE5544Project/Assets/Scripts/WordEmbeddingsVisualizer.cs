using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordEmbeddingsVisualizer : MonoBehaviour
{
    public float width = 2f;
    public GameObject TMProPrefab;
    public Material boundsMaterial;
    public Material particleMaterialMobile;
    public static WordEmbeddingsVisualizer instance;

    Dictionary<string, float[]> embeddings;
    Dictionary<ParticleSystem.Particle, string> particleToKey;
    bool loadedData = false;
    float[] minValues, maxValues, differences;
    ParticleSystem.Particle[] particles;
    List<string> filters;
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

    public List<string> GetFilterSelection(GameObject g)
    {
        List<string> f = new List<string>();
        foreach(KeyValuePair<ParticleSystem.Particle, string> pair in particleToKey)
        {
            if(Vector3.Distance(g.transform.position, pair.Key.position) < g.transform.localScale.x)
            {
                f.Add(pair.Value);
            }
        }
        return f;
    }
    public IEnumerator SetFilter(List<string> f)
    {
        filters = f;

        while (!loadedData) yield return null;

        ParticleSystem.MainModule mainModule = GetComponent<ParticleSystem>().main;
        mainModule.maxParticles = filters.Count;
        particles = new ParticleSystem.Particle[filters.Count];
        int i = 0;

        foreach (string s in filters)
        {
            if (TMProPrefab)
            {
                GameObject text = GameObject.Instantiate(TMProPrefab);
                text.GetComponent<TextMeshPro>().text = s;
                text.transform.position = new Vector3(embeddings[s][0], embeddings[s][1] + text.GetComponent<TextMeshPro>().fontSize, embeddings[s].Length > 2 ? embeddings[s][2] : 0);
                text.transform.SetParent(transform, false);
            }

            particles[i] = new ParticleSystem.Particle();
            if (embeddings[s].Length == 2)
            {
                particles[i].position = new Vector2(embeddings[s][0], embeddings[s][1]);
                particles[i].startColor = Color.HSVToRGB(Mathf.InverseLerp(minValues[0], maxValues[0], embeddings[s][0]),
                    Mathf.InverseLerp(minValues[1], maxValues[1], embeddings[s][1]), 1);
            }
            else if (embeddings[s].Length == 3)
            {
                particles[i].position = new Vector3(embeddings[s][0], embeddings[s][1], embeddings[s][2]);
                particles[i].startColor = new Color(Mathf.InverseLerp(minValues[0], maxValues[0], embeddings[s][0]),
                    Mathf.InverseLerp(minValues[1], maxValues[1], embeddings[s][1]),
                    Mathf.InverseLerp(minValues[2], maxValues[2], embeddings[s][2]));
            }
            particles[i].rotation3D = Vector3.zero;
            particles[i].startSize = 0.25f;
            //yield return null;
            i++;
        }
        GetComponent<ParticleSystem>().SetParticles(particles, particles.Length);
    }
    public IEnumerator SetData(TextAsset t)
    {
        particleToKey = new Dictionary<ParticleSystem.Particle, string>();
        yield return embeddings = DataImporter.LoadWord2VecEmbeddings(t);
        PreProcessData();
        CreateBounds();
        loadedData = true;
    }
    private void CreateBounds()
    {
        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(-transform.localScale.x * width / 2f, -transform.localScale.y * width / 2f, 0);
        g.transform.localScale = new Vector3(width / 50f, width / 50f, width);
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(transform.localScale.x * width / 2f, -transform.localScale.y * width / 2f, 0);
        g.transform.localScale = new Vector3(width / 50f, width / 50f, width);
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(-transform.localScale.x * width / 2f, transform.localScale.y * width / 2f, 0);
        g.transform.localScale = new Vector3(width / 50f, width / 50f, width);
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(transform.localScale.x * width / 2f, transform.localScale.y * width / 2f, 0);
        g.transform.localScale = new Vector3(width / 50f, width / 50f, width);
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(-transform.localScale.x * width / 2f, 0, -transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width / 50f, width , width / 50f);
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(-transform.localScale.x * width / 2f, 0, transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width / 50f, width, width / 50f);
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(transform.localScale.x * width / 2f, 0, -transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width / 50f, width, width / 50f);
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(transform.localScale.x * width / 2f, 0, transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width / 50f, width, width / 50f);
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(0, -transform.localScale.y * width / 2f, -transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width, width / 50f, width / 50f);
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(0, -transform.localScale.y * width / 2f, transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width, width / 50f, width / 50f);
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(0, transform.localScale.y * width / 2f, -transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width, width / 50f, width / 50f);
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(0, transform.localScale.y * width / 2f, transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width, width / 50f, width / 50f);
        g.transform.SetParent(transform, false);

        // "Glass Panes"
        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(0, 0, transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width, width, width / 500f);
        g.GetComponent<Renderer>().material = boundsMaterial;
        g.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.1f);
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(0, 0, -transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width, width, width / 500f);
        g.GetComponent<Renderer>().material = boundsMaterial;
        g.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.1f);
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(0, transform.localScale.y * width / 2f, 0);
        g.transform.localScale = new Vector3(width, width / 500f, width);
        g.GetComponent<Renderer>().material = boundsMaterial;
        g.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.1f);
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(0, -transform.localScale.y * width / 2f, 0);
        g.transform.localScale = new Vector3(width, width / 500f, width);
        g.GetComponent<Renderer>().material = boundsMaterial;
        g.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.1f);
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(transform.localScale.x * width / 2f, 0, 0);
        g.transform.localScale = new Vector3(width / 500f, width, width);
        g.GetComponent<Renderer>().material = boundsMaterial;
        g.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.1f);
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(-transform.localScale.x * width / 2f, 0, 0);
        g.transform.localScale = new Vector3(width / 500f, width, width);
        g.GetComponent<Renderer>().material = boundsMaterial;
        g.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.1f);
        g.transform.SetParent(transform, false);

       
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
        differences = new float[minValues.Length];
        for(int i = 0; i < minValues.Length; i++)
        {
            differences[i] = maxValues[i] - minValues[i];
        }        
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
#if UNITY_ANDROID
        GetComponent<ParticleSystemRenderer>().material = particleMaterialMobile;
#endif
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
                particles[i].position = new Vector2(Mathf.InverseLerp(minValues[0], maxValues[0], pair.Value[0]) * width - width / 2f,
                    Mathf.InverseLerp(minValues[1], maxValues[1], pair.Value[1]) * width - width / 2f);
                particles[i].startColor = Color.HSVToRGB(Mathf.InverseLerp(minValues[0], maxValues[0], pair.Value[0]),
                    Mathf.InverseLerp(minValues[1], maxValues[1], pair.Value[1]), 1);
            }
            else if (pair.Value.Length == 3)
            {                
                particles[i].position = new Vector3(Mathf.InverseLerp(minValues[0], maxValues[0], pair.Value[0]) * width - width / 2f,
                    Mathf.InverseLerp(minValues[1], maxValues[1], pair.Value[1]) * width - width / 2f,
                    Mathf.InverseLerp(minValues[2], maxValues[2], pair.Value[2]) * width - width / 2f);
                particles[i].startColor = new Color(Mathf.InverseLerp(minValues[0], maxValues[0], pair.Value[0]),
                    Mathf.InverseLerp(minValues[1], maxValues[1], pair.Value[1]),
                    Mathf.InverseLerp(minValues[2], maxValues[2], pair.Value[2]));
            }
            particles[i].rotation3D = Vector3.zero;
            particles[i].startSize = 0.25f * width / Mathf.Min(differences);
            if (!particleToKey.ContainsValue(pair.Key) && !particleToKey.ContainsKey(particles[i]))
            {
                particleToKey.Add(particles[i], pair.Key);
            }
            i++;
            //yield return null;
        }
        GetComponent<ParticleSystem>().SetParticles(particles, particles.Length);
    }
}
