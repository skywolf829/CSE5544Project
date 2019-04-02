﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KnowledgeGraphEmbeddingsVisualizer : MonoBehaviour
{
    public float width = 2f;
    public GameObject TMProPrefab;
    public Material boundsMaterial;
    public Material particleMaterialMobile;
    public static KnowledgeGraphEmbeddingsVisualizer instance;
    public ParticleSystem KnowledgeGraphEmbeddingsParticleSystem;
    public ParticleSystem connectionsParticleSystem;
    public bool showConnections = false;

    [HideInInspector]
    public Dictionary<string, float[]> embeddings;

    Dictionary<ParticleSystem.Particle, string> particleToKey;
    Dictionary<string, ParticleSystem.Particle> keyToParticle;
    bool loadedData = false;
    bool colorsLoaded = false;
    float[] minValues, maxValues, differences;
    ParticleSystem.Particle[] particles;
    List<ParticleSystem.Particle> connectionParticles;

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
#if UNITY_ANDROID
        GetComponent<ParticleSystemRenderer>().material = particleMaterialMobile;
#endif
    }

    public Dictionary<string, Vector3> GetFilterSelectionDict(Vector3 pos, float dist)
    {
        Dictionary<string, Vector3> f = new Dictionary<string, Vector3>();
        foreach (KeyValuePair<ParticleSystem.Particle, string> pair in particleToKey)
        {
            if (Vector3.Distance(pos, pair.Key.position + transform.position) < dist)
            {
                if(!f.ContainsKey(pair.Value)) f.Add(pair.Value, pair.Key.position + transform.position);
            }
        }
        return f;
    }
    public IEnumerator GetFilterSelection(Vector3 pos, float dist, int steps = 250)
    {
        if (particleToKey != null)
        {
            List<string> f = new List<string>();
            int i = 0;
            foreach (KeyValuePair<ParticleSystem.Particle, string> pair in particleToKey)
            {
                if (Vector3.Distance(pos, pair.Key.position + transform.position) < dist)
                {
                    if (!f.Contains(pair.Value)) f.Add(pair.Value);
                }
                if (i % steps == 0 && steps != 0)
                {
                    LoadingManager.instance.SetProgress("GetFilterSelectionKG", 
                        VizControllerScript.instance.selectWithConnections ? (float)i / (particleToKey.Count * 2) : (float)i / particleToKey.Count, 
                        "Determining filter selection");
                    yield return null;
                }
                i++;
            }
            yield return null;
            
            if (VizControllerScript.instance.selectWithConnections)
            {
                List<string> filtersToAdd = new List<string>();
                for(i = 0; i < VizControllerScript.instance.entries.Count; i++)
                {
                    if (f.Contains(VizControllerScript.instance.entries[i][0]))
                    {
                        if (!f.Contains(VizControllerScript.instance.entries[i][1]) && !filtersToAdd.Contains(VizControllerScript.instance.entries[i][1]))
                        {
                            filtersToAdd.Add(VizControllerScript.instance.entries[i][1]);
                        }
                    }
                    if (f.Contains(VizControllerScript.instance.entries[i][1]))
                    {
                        if (!f.Contains(VizControllerScript.instance.entries[i][0]) && !filtersToAdd.Contains(VizControllerScript.instance.entries[i][0]))
                        {
                            filtersToAdd.Add(VizControllerScript.instance.entries[i][0]);
                        }
                    }
                    if (i % (steps * 10) == 0 && steps != 0)
                    {
                        LoadingManager.instance.SetProgress("GetFilterSelectionKG", 0.5f + (float)i / (VizControllerScript.instance.entries.Count * 2),
                            "Determining filter selection");
                        yield return null;
                    }
                }
                f.AddRange(filtersToAdd);
            }
            LoadingManager.instance.FinishedLoading("GetFilterSelectionKG");
            VizControllerScript.instance.UpdateFilters(f);
        }
        yield return null;
    }
    private void CreateBounds()
    {
        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(-transform.localScale.x * width / 2f, -transform.localScale.y * width / 2f, 0);
        g.transform.localScale = new Vector3(width / 50f, width / 50f, width);
        Destroy(g.GetComponent<BoxCollider>());
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(transform.localScale.x * width / 2f, -transform.localScale.y * width / 2f, 0);
        g.transform.localScale = new Vector3(width / 50f, width / 50f, width);
        Destroy(g.GetComponent<BoxCollider>());
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(-transform.localScale.x * width / 2f, transform.localScale.y * width / 2f, 0);
        g.transform.localScale = new Vector3(width / 50f, width / 50f, width);
        Destroy(g.GetComponent<BoxCollider>());
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(transform.localScale.x * width / 2f, transform.localScale.y * width / 2f, 0);
        g.transform.localScale = new Vector3(width / 50f, width / 50f, width);
        Destroy(g.GetComponent<BoxCollider>());
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(-transform.localScale.x * width / 2f, 0, -transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width / 50f, width, width / 50f);
        Destroy(g.GetComponent<BoxCollider>());
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(-transform.localScale.x * width / 2f, 0, transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width / 50f, width, width / 50f);
        Destroy(g.GetComponent<BoxCollider>());
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(transform.localScale.x * width / 2f, 0, -transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width / 50f, width, width / 50f);
        Destroy(g.GetComponent<BoxCollider>());
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(transform.localScale.x * width / 2f, 0, transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width / 50f, width, width / 50f);
        Destroy(g.GetComponent<BoxCollider>());
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(0, -transform.localScale.y * width / 2f, -transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width, width / 50f, width / 50f);
        Destroy(g.GetComponent<BoxCollider>());
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(0, -transform.localScale.y * width / 2f, transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width, width / 50f, width / 50f);
        Destroy(g.GetComponent<BoxCollider>());
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(0, transform.localScale.y * width / 2f, -transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width, width / 50f, width / 50f);
        Destroy(g.GetComponent<BoxCollider>());
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(0, transform.localScale.y * width / 2f, transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width, width / 50f, width / 50f);
        Destroy(g.GetComponent<BoxCollider>());
        g.transform.SetParent(transform, false);

        // "Glass Panes"
        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(0, 0, transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width, width, width / 500f);
        g.GetComponent<Renderer>().material = boundsMaterial;
        g.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.1f);
        Destroy(g.GetComponent<BoxCollider>());
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(0, 0, -transform.localScale.z * width / 2f);
        g.transform.localScale = new Vector3(width, width, width / 500f);
        g.GetComponent<Renderer>().material = boundsMaterial;
        g.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.1f);
        Destroy(g.GetComponent<BoxCollider>());
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(0, transform.localScale.y * width / 2f, 0);
        g.transform.localScale = new Vector3(width, width / 500f, width);
        g.GetComponent<Renderer>().material = boundsMaterial;
        g.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.1f);
        Destroy(g.GetComponent<BoxCollider>());
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(0, -transform.localScale.y * width / 2f, 0);
        g.transform.localScale = new Vector3(width, width / 500f, width);
        g.GetComponent<Renderer>().material = boundsMaterial;
        g.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.1f);
        Destroy(g.GetComponent<BoxCollider>());
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(transform.localScale.x * width / 2f, 0, 0);
        g.transform.localScale = new Vector3(width / 500f, width, width);
        g.GetComponent<Renderer>().material = boundsMaterial;
        g.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.1f);
        Destroy(g.GetComponent<BoxCollider>());
        g.transform.SetParent(transform, false);

        g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "VisBounds";
        g.transform.position = new Vector3(-transform.localScale.x * width / 2f, 0, 0);
        g.transform.localScale = new Vector3(width / 500f, width, width);
        g.GetComponent<Renderer>().material = boundsMaterial;
        g.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.1f);
        Destroy(g.GetComponent<BoxCollider>());
        g.transform.SetParent(transform, false);
    }
    public void PreProcessData()
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
        for (int i = 0; i < minValues.Length; i++)
        {
            differences[i] = maxValues[i] - minValues[i];
        }
        CreateBounds();
        loadedData = true;
        colorsLoaded = true;
    }
    private Color getColorForEmbedding(string s)
    {

        Color c = Color.black;
        if (VizControllerScript.instance.embeddingColorsDict.ContainsKey(s))
            c = VizControllerScript.instance.embeddingColorsDict[s];
        else
        {
            //print("Colors dict missing " + s);
        }
        return c;
    }
    public IEnumerator UpdateVisualization(List<string> filters = null, bool scaled = false, int numPerUpdate = 0, bool selectionWithConnections = false)
    {
        while (!loadedData || !colorsLoaded) yield return null;

        LoadingManager.instance.SetProgress("UpdateVisualizationKG", 0.01f, "Updating KG Visualization");
        ParticleSystem.MainModule mainModule = KnowledgeGraphEmbeddingsParticleSystem.main;
        ParticleSystem.MainModule connectionsMainModule = connectionsParticleSystem.main;

        KnowledgeGraphEmbeddingsParticleSystem.SetParticles(new ParticleSystem.Particle[0], 0);
        connectionsParticleSystem.SetParticles(new ParticleSystem.Particle[0], 0);

        int i = 0;
        if (filters == null || filters.Count == 0)
        {
            filters = new List<string>();
            foreach (string s in embeddings.Keys)
            {
                filters.Add(s);
                if (numPerUpdate != 0 && i % numPerUpdate == 0)
                {
                    LoadingManager.instance.SetProgress("UpdateVisualizationKG", 0.2f * (float)i / embeddings.Count, "Updating KG Visualization");
                    yield return null;
                }
                i++;
            }
        }

        i = 0;
        mainModule.maxParticles = filters.Count;
        particles = new ParticleSystem.Particle[filters.Count];
        connectionParticles = new List<ParticleSystem.Particle>();
        particleToKey = new Dictionary<ParticleSystem.Particle, string>();
        keyToParticle = new Dictionary<string, ParticleSystem.Particle>();

        float[] tempMin = new float[minValues.Length];
        float[] tempMax = new float[maxValues.Length];
        float size = 0.25f * width / Mathf.Min(differences);
        if (scaled)
        {
            for (i = 0; i < filters.Count; i++)
            {
                for (int j = 0; j < tempMax.Length; j++)
                {
                    tempMin[j] = Mathf.Min(tempMin[j], embeddings[filters[i]][j]);
                    tempMax[j] = Mathf.Max(tempMax[j], embeddings[filters[i]][j]);
                }
            }
        }
        else
        {
            tempMin = minValues;
            tempMax = maxValues;
        }
        i = 0;
        
        for (i = 0; i < filters.Count; i++)
        {
            particles[i] = new ParticleSystem.Particle();
            float[] currentValues = new float[] {
                (Mathf.InverseLerp(tempMin[0], tempMax[0], embeddings[filters[i]][0]) - 0.5f) * width,
                    (Mathf.InverseLerp(tempMin[1],tempMax[1], embeddings[filters[i]][1]) - 0.5f) * width,
                    minValues.Length == 3 ? (Mathf.InverseLerp(tempMin[2],tempMax[2], embeddings[filters[i]][2]) - 0.5f) * width : 0
            };
            if (embeddings[filters[i]].Length == 2)
            {
                particles[i].position = new Vector2(currentValues[0], currentValues[1]);

                if (VizControllerScript.instance.predicatesSelected.Count == 0 ||
                    !VizControllerScript.instance.useSOPColoring) particles[i].startColor = getColorForEmbedding(filters[i]);
                else particles[i].startColor = VizControllerScript.instance.SOPColoring[i];
            }
            else if (embeddings[filters[i]].Length == 3)
            {
                particles[i].position = new Vector3(currentValues[0], currentValues[1], currentValues[2]);
                if (VizControllerScript.instance.predicatesSelected.Count == 0 ||
                    !VizControllerScript.instance.useSOPColoring) particles[i].startColor = getColorForEmbedding(filters[i]);
                else particles[i].startColor = VizControllerScript.instance.SOPColoring[i];
            }
            particles[i].rotation3D = Vector3.zero;
            particles[i].startSize = size;
            particles[i].randomSeed = (uint)filters[i].GetHashCode();
            particles[i].axisOfRotation = new Vector3(Random.value, Random.value, Random.value);

            particleToKey.Add(particles[i], filters[i]);
            if(!keyToParticle.ContainsKey(filters[i])) keyToParticle.Add(filters[i], particles[i]);            
            
            if (numPerUpdate != 0 && i % numPerUpdate == 0)
            {
                mainModule.maxParticles = particles.Length;
                KnowledgeGraphEmbeddingsParticleSystem.SetParticles(particles, particles.Length);
                LoadingManager.instance.SetProgress("UpdateVisualizationKG", 0.2f + 0.8f * (float)i / filters.Count, "Updating KG Visualization");
                yield return null;
            }
        }
        mainModule.maxParticles = particles.Length;
        KnowledgeGraphEmbeddingsParticleSystem.SetParticles(particles, particles.Length);
        LoadingManager.instance.FinishedLoading("UpdateVisualizationKG");
        yield return null;

        if (showConnections)
        {
            GameObject empty = new GameObject();
            if (VizControllerScript.instance.predicatesSelected.Count > 0 || selectionWithConnections)
            {
                for (i = 0; i < VizControllerScript.instance.entries.Count; i++)
                {
                    if ((VizControllerScript.instance.predicatesSelected.Contains(VizControllerScript.instance.entries[i][2]) || selectionWithConnections)
                        && keyToParticle.ContainsKey(VizControllerScript.instance.entries[i][0])
                        && keyToParticle.ContainsKey(VizControllerScript.instance.entries[i][1]))
                    {
                        Vector3 pos1 = keyToParticle[VizControllerScript.instance.entries[i][0]].position;
                        Vector3 pos2 = keyToParticle[VizControllerScript.instance.entries[i][1]].position;

                        ParticleSystem.Particle p = new ParticleSystem.Particle();
                        p.position = (pos1 + pos2) / 2f;
                        empty.transform.position = (pos1 + transform.position + pos2 + transform.position) / 2f;
                        empty.transform.LookAt(pos2 + transform.position);
                        p.rotation3D = empty.transform.eulerAngles;
                        p.startSize3D = new Vector3(size / 10f, size / 10f, Vector3.Distance(pos1, pos2));
                        p.startColor = Color.gray;
                        connectionParticles.Add(p);
                    }
                    if (numPerUpdate != 0 && i % (numPerUpdate * 100) == 0)
                    {
                        connectionsMainModule.maxParticles = connectionParticles.Count;
                        connectionsParticleSystem.SetParticles(connectionParticles.ToArray(), connectionParticles.Count);
                        LoadingManager.instance.SetProgress("UpdateVisualizationKGConnections", (float)i / VizControllerScript.instance.entries.Count, "Updating KG Visualization Connections");
                        yield return null;
                    }
                }
            }
            Destroy(empty);
            connectionsMainModule.maxParticles = connectionParticles.Count;
            connectionsParticleSystem.SetParticles(connectionParticles.ToArray(), connectionParticles.Count);
            LoadingManager.instance.FinishedLoading("UpdateVisualizationKGConnections");
        }
        else connectionsMainModule.maxParticles = 0;
        yield return null;
    }
}
