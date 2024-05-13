using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class ScrapSpawner : MonoBehaviour
{

    public GameObject[] m_scrapPrefabs;
    [SerializeField] List<GameObject> m_spawnedScrap;
    [SerializeField] List<Vector3> m_spawnLocations;
    [SerializeField] Transform m_scrapParent;

    public int m_seed = 1337;
    [SerializeField] bool m_randomizeSeed = true;
    [SerializeField] Vector2 m_seedOffset = Vector2.zero;
    [SerializeField] GameObject m_scrapSpawnMeshObject;
    Mesh m_scrapSpawnMesh;
    [SerializeField] Material m_heatMaterial;
    [SerializeField] bool m_bypassRandomization = false;
    [SerializeField] bool m_itemhax = false;

    [SerializeField] float m_spawnRarity = 0.5f;
    [SerializeField] int m_slowPopulationIndex = 0;
    [SerializeField] bool m_populateOverTime = true;
    [SerializeField] bool m_hasPopulatedSpawns = false;
    [SerializeField] bool m_manualPopulationStart = false;
    [SerializeField] bool m_debug = false;

    void Start()
    {
        
        if (m_randomizeSeed) // generate new random seed
            m_seed = Random.Range(13371337, 69696969);

        //grab mesh containing scrap spawn points
        m_scrapSpawnMesh = m_scrapSpawnMeshObject.GetComponent<MeshFilter>().sharedMesh;
        
        // initialize randomization with seed
        InitRandom(m_seed);

        // randomize heatmap
        RandomizeHeatmap();

        //generate locations where a random piece of scrap will be spawned
        GenerateScrapSpawns();

        if (!m_manualPopulationStart)
        {
            //spawn a random scrap prefab at each location
            PopulateSpawns();
        }

    }
    private void Update()
    {
        if (!m_hasPopulatedSpawns)
        {
            if (m_manualPopulationStart)
            {
                /*if (Input.GetKeyDown("8"))
                    PopulateAllSpawns();*/
            }
            else
                PopulateSpawns();
        }

        if (m_debug) // you should probably only use this in the debug scene i made - reality
        {
            foreach (var go in m_spawnedScrap)
            {
                Vector2 uv = new Vector2(go.transform.position.x / 100, go.transform.position.z / 100);
                var cur = go.transform.position;
                cur.y = GetHeat(uv, m_heatMaterial.GetVector("_Offset"), m_heatMaterial.GetFloat("_Scale"), m_heatMaterial.GetFloat("_Deadzone"));
                go.transform.position = cur;
            }
        }
    }

    public void InitRandom(int seed = 0)
    {
        // if init is called with a seed set m_seed to that seed
        if (seed != 0)
            m_seed = seed;

        // bitwise operator to grab 2 different integers from a single integer
        m_seedOffset.x = m_seed & 0b0101010101010101;
        m_seedOffset.y = m_seed & 0b1010101010101010;
    }
    public void RandomizeHeatmap()
    {
        if (m_bypassRandomization) return;

        // randomize the offset and keep it between 10-20 so that the heatmap is fairly accurate
        m_seedOffset = m_seedOffset.normalized * ((m_seed % 10) + 10);

        // set the new offset in the heatmap material
        m_heatMaterial.SetVector("_Offset", new Vector4(m_seedOffset.x % 20, m_seedOffset.y % 20, 0, 0));
    }

    public void CleanScrap()
    {
        //destroy all spawned scrap
        foreach (GameObject go in m_spawnedScrap)
        {
            Destroy(go);
        }
        // clean lists
        m_spawnedScrap.Clear();
        m_spawnLocations.Clear();
        m_slowPopulationIndex = 0;
    }

    public void GenerateScrapSpawns()
    {
        //RenderTexture doubleBufferRT = m_heatRenderTexture.GetDoubleBufferRenderTexture();

        //RenderTexture currentActiveRT = CustomRenderTexture.active;
        //CustomRenderTexture.active = m_heatRenderTexture;
        //Texture2D tex = new Texture2D(m_heatRenderTexture.width, m_heatRenderTexture.height, TextureFormat.RGBAFloat, false);
        //tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        //tex.Apply();
        //Color[] heatPixels = tex.GetPixels();
        //CustomRenderTexture.active = currentActiveRT;

        //RenderTexture doubleBufferRT = m_heatRenderTexture.GetDoubleBufferRenderTexture();
        //Texture2D tex = new Texture2D(doubleBufferRT.width, doubleBufferRT.height, TextureFormat.RGBAFloat, false);
        //Graphics.CopyTexture(doubleBufferRT, tex);
        //Color[] heatPixels = tex.GetPixels();

        // ^^^unused (attempt at sampling noise from renter texture ((impossible)))

        // grab vertices from mesh
        Vector3[] vertices = m_scrapSpawnMesh.vertices;
        if (vertices.Length == 0)
        {
            Debug.LogException(new Exception("Scrap spawn mesh has 0 vertices, turn off \"Vertex Order\" optimization in the import settings"));
        }

        //rotate the points so they align with the world
        m_scrapSpawnMeshObject.transform.TransformPoints(vertices);

        // grab center and extents of mesh to use for heatmap calculations
        Vector3 meshCenter = m_scrapSpawnMeshObject.transform.TransformPoint(m_scrapSpawnMesh.bounds.center);
        Vector3 meshExtents = m_scrapSpawnMeshObject.transform.TransformPoint(m_scrapSpawnMesh.bounds.extents);


        // loop over all vertices
        foreach (var vertex in vertices)
        {
            // calculate their normal position relative to the world size
            Vector2 normalPos = new Vector2((vertex.x - meshCenter.x) / Mathf.Abs(meshExtents.x), (vertex.z - meshCenter.z) / Mathf.Abs(meshExtents.z));
            
            // grab the heatmap "heat" at that location
            float heat = GetHeat(normalPos, m_heatMaterial.GetVector("_Offset"), m_heatMaterial.GetFloat("_Scale"), m_heatMaterial.GetFloat("_Deadzone"));
            
            // if that location is hot enough, save that location
            if (m_itemhax || (Random.Range(0.0f, 1.0f) < heat && Random.Range(0.0f, 1.0f) < m_spawnRarity))
            {
                m_spawnLocations.Add(vertex + new Vector3(0, 10 + 5 * heat, 0));
            }
        }
    }

    public void PopulateSpawns()
    {
        // self explanatory
        if (m_populateOverTime)
        {
            PopulateNextSpawn();
        }
        else
        {
            PopulateAllSpawns();
        }
    }
    public void PopulateAllSpawns()
    {

        if (m_scrapPrefabs.Length == 0)
        {
            Debug.LogError("No Scrap Prefabs assigned");
            return;
        }

        // loop over all saved spawn locations
        foreach (Vector3 location in  m_spawnLocations)
        {
            // grab a random scrap prefab
            int TheChosenOne = Random.Range(0, m_scrapPrefabs.Length);
            
            // instantiate the prefab
            GameObject spawnedPrefab = GameObject.Instantiate(m_scrapPrefabs[TheChosenOne], location, Quaternion.identity, m_scrapParent);
            m_spawnedScrap.Add(spawnedPrefab);
        }
        m_hasPopulatedSpawns = true;
    }

    public void PopulateNextSpawn()
    {
        // version of "PopulateAllSpawns" that indexes into spawn locations
        // instead of looping over all of them so we can spawn one prefab per frame

        if (m_slowPopulationIndex >= m_spawnLocations.Count)
        {
            m_hasPopulatedSpawns = true;
            return;
        }
        if (m_scrapPrefabs.Length == 0)
        {
            Debug.LogError("No Scrap Prefabs assigned");
            return;
        }
        Vector3 location = m_spawnLocations[m_slowPopulationIndex];
        
        int TheChosenOne = Random.Range(0, m_scrapPrefabs.Length);

        GameObject spawnedPrefab = GameObject.Instantiate(m_scrapPrefabs[TheChosenOne], location, Quaternion.identity, m_scrapParent);
        m_spawnedScrap.Add(spawnedPrefab);
        m_slowPopulationIndex++;
    }

    float fract(float f)
    {
        return f - Mathf.Floor(f);
    }
    Vector2 fract2(Vector2 v)
    {
        return new Vector2(fract(v.x), fract(v.y));
    }
    Vector2 floor(Vector2 v)
    {
        return new Vector2(Mathf.Floor(v.x), Mathf.Floor(v.y));
    }
    float unity_noise_randomValue(Vector2 uv)
    {
        return fract(Mathf.Sin(Vector2.Dot(uv, new Vector2(12, 78))) * 430);
    }

    // part of shadergraph noise function (unity docs)
    float unity_noise_interpolate(float a, float b, float t)
    {
        return (1.0f - t) * a + (t * b);
    }

    // part of shadergraph noise function (unity docs)
    float unity_valueNoise(Vector2 uv)
    {
        float2 i = floor(uv);
        float2 fractUV = fract2(uv);
        float2 f = fract2(uv);
        f = f * f * new float2(3 - (2 * f).x, 3 - (2 * f).y);

        float2 c0 = i + new float2(0, 0);
        float2 c1 = i + new float2(1, 0);
        float2 c2 = i + new float2(0, 1);
        float2 c3 = i + new float2(1, 1);
        float r0 = unity_noise_randomValue(c0);
        float r1 = unity_noise_randomValue(c1);
        float r2 = unity_noise_randomValue(c2);
        float r3 = unity_noise_randomValue(c3);

        float bottomOfGrid = unity_noise_interpolate(r0, r1, f.x);
        float topOfGrid = unity_noise_interpolate(r2, r3, f.x);
        float t = unity_noise_interpolate(bottomOfGrid, topOfGrid, f.y);
        return t;
    }

    // part of shadergraph noise function (unity docs)
    float Unity_SimpleNoise_float(Vector2 UV, float Scale)
    {
        float t = 0.0f;

        float freq = 1;
        float amp = 0.125f;
        t += unity_valueNoise(new Vector2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

        freq = 2;
        amp = 0.25f;
        t += unity_valueNoise(new Vector2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

        freq = 4;
        amp = 0.5f;
        t += unity_valueNoise(new Vector2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

        float Out = t;
        return Out;
    }

    float GetHeat(Vector2 uv, Vector4 offset, float scale, float deadzoneSize)
    {
        // calculate a deadzone at the center
        float deadzone = Mathf.Clamp01(Vector2.Distance(Vector2.zero, new Vector2(uv.x, uv.y)) - deadzoneSize);
        
        // calculate and scale noise value
        float f = 0f;
        Vector2 off = new Vector2(offset.x, offset.y);
        f = Unity_SimpleNoise_float(new float2(off.x + uv.x, off.y + uv.y), scale);
        f -= 0.5f;
        f *= 10;

        //combine noise and deadzone and return heat
        float heat = (deadzone * f);
        return heat;
    }
}
