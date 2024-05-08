using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class ScrapSpawner : MonoBehaviour
{

    [SerializeField] GameObject[] m_scrapPrefabs;
    [SerializeField] List<GameObject> m_spawnedScrap;
    [SerializeField] List<Vector3> m_spawnLocations;
    [SerializeField] Transform m_scrapParent;

    public int m_seed = 1337;
    [SerializeField] Vector2 m_seedOffset = Vector2.zero;
    [SerializeField] GameObject m_scrapSpawnMeshObject;
    Mesh m_scrapSpawnMesh;
    [SerializeField] CustomRenderTexture m_heatRenderTexture;
    [SerializeField] Material m_heatMaterial;
    [SerializeField] bool m_bypassRandomization = false;

    void Start()
    {
        m_scrapSpawnMesh = m_scrapSpawnMeshObject.GetComponent<MeshFilter>().sharedMesh;
        InitRandom(m_seed);
        RandomizeHeatmap();
        GenerateScrapSpawns();
        PopulateSpawns();
    }

    public void InitRandom(int seed = 0)
    {
        if (seed != 0)
            m_seed = seed;

        m_seedOffset.x = m_seed & 0b0101010101010101;
        m_seedOffset.y = m_seed & 0b1010101010101010;

    }
    public void RandomizeHeatmap()
    {
        if (m_bypassRandomization) return;
        m_seedOffset = m_seedOffset.normalized * ((m_seed % 10) + 10);

        m_heatRenderTexture.material.SetVector("_Offset", new Vector4(m_seedOffset.x, m_seedOffset.y, 0, 0));
    }

    public void CleanScrap()
    {
        foreach (GameObject go in m_spawnedScrap)
        {
            Destroy(go);
        }
        m_spawnedScrap.Clear();
        m_spawnLocations.Clear();
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

        Vector3[] vertices = m_scrapSpawnMesh.vertices;
        m_scrapSpawnMeshObject.transform.TransformPoints(vertices);
        Vector3 meshCenter = m_scrapSpawnMeshObject.transform.TransformPoint(m_scrapSpawnMesh.bounds.center);
        Vector3 meshExtents = m_scrapSpawnMeshObject.transform.TransformPoint(m_scrapSpawnMesh.bounds.extents);

        print("center: " + meshCenter);
        print("extents: " + meshExtents);



        int idx = 0;
        foreach (var vertex in vertices)
        {
            Vector2 normalPos = new Vector2((vertex.x - meshCenter.x) / Mathf.Abs(meshExtents.x), (vertex.z - meshCenter.z) / Mathf.Abs(meshExtents.z));

            float heat = GetHeat(normalPos, m_heatMaterial.GetVector("_Offset"), m_heatMaterial.GetFloat("_Scale"), m_heatMaterial.GetFloat("_Deadzone"));
            if (heat > 0)
                m_spawnLocations.Add(vertex  + new Vector3(0, 5 * heat, 0));
            
            idx++;
        }


    }
    public void PopulateSpawns()
    {
        if (m_scrapPrefabs.Length == 0)
        {
            Debug.LogError("No Scrap Prefabs assigned");
            return;
        }
        foreach (Vector3 location in  m_spawnLocations)
        {
            int TheChosenOne = Random.Range(0, m_scrapPrefabs.Length);

            GameObject spawnedPrefab = GameObject.Instantiate(m_scrapPrefabs[TheChosenOne], location, Quaternion.identity, m_scrapParent);
            m_spawnedScrap.Add(spawnedPrefab);
        }
    }

    float fract(float f)
    {
        return f - Mathf.Floor(f);

    }
    Vector2 fract(Vector2 v)
    {
        return new Vector2(fract(v.x), fract(v.y));
    }
    Vector2 floor(Vector2 v)
    {
        return new Vector2(Mathf.Floor(v.x), Mathf.Floor(v.y));
    }
    Vector2 abs(Vector2 v)
    {
        return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
    }
    Vector2 pow(float p, Vector2 v)
    {
        return new Vector2(Mathf.Pow(p, v.x), Mathf.Pow(p, v.y));
    }

    float unity_noise_randomValue(Vector2 uv)
    {
        return fract(Mathf.Sin(Vector2.Dot(uv, new Vector2(12, 78))) * 43758);
    }

    float unity_noise_interpolate(float a, float b, float t)
    {
        return (1.0f - t) * a + (t * b);
    }

    float unity_valueNoise(Vector2 uv)
    {
        Vector2 i = floor(uv);
        Vector2 fractUV = fract(uv);
        Vector2 f = fract(uv);
        f = f * f * new Vector2(3.0f - (2.0f * f).x, 3.0f - (2.0f * f).y);

        uv = abs(new Vector2(fractUV.x - 0.5f, fractUV.x - 0.5f));
        Vector2 c0 = i + new Vector2(0.0f, 0.0f);
        Vector2 c1 = i + new Vector2(1.0f, 0.0f);
        Vector2 c2 = i + new Vector2(0.0f, 1.0f);
        Vector2 c3 = i + new Vector2(1.0f, 1.0f);
        float r0 = unity_noise_randomValue(c0);
        float r1 = unity_noise_randomValue(c1);
        float r2 = unity_noise_randomValue(c2);
        float r3 = unity_noise_randomValue(c3);

        float bottomOfGrid = unity_noise_interpolate(r0, r1, f.x);
        float topOfGrid = unity_noise_interpolate(r2, r3, f.x);
        float t = unity_noise_interpolate(bottomOfGrid, topOfGrid, f.y);
        return t;
    }

    float Unity_SimpleNoise_float(Vector2 UV, float Scale)
    {
        float t = 0.0f;

        float freq = Mathf.Pow(2.0f, 0);
        float amp = Mathf.Pow(0.5f, 3 - 0);
        t += unity_valueNoise(new Vector2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

        freq = Mathf.Pow(2.0f, 1);
        amp = Mathf.Pow(0.5f, 3 - 1);
        t += unity_valueNoise(new Vector2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

        freq = Mathf.Pow(2.0f, 2);
        amp = Mathf.Pow(0.5f, 3 - 2);
        t += unity_valueNoise(new Vector2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

        float Out = t;
        return Out;
    }

    float GetHeat(Vector2 uv, Vector4 offset, float scale, float deadzoneSize)
    {
        //float deadzoneHeat = Mathf.Clamp01(Vector2.Distance(Vector2.zero, new Vector2((uv * 2f).x - 1f - deadzoneSize, (uv * 2f).y - 1f - deadzoneSize)));
        //float f = 0;
        //Unity_SimpleNoise_float(new Vector2(offset.x + uv.x, offset.y + uv.y), scale, out f);
        //f -= 0.5f;
        //f *= 10;
        //float heat = (deadzoneHeat * f);
        //return heat;


        float deadzone = Mathf.Clamp01(Vector2.Distance(Vector2.zero, new Vector2((uv.x * 2) - 1, (uv.y * 2) - 1)) - deadzoneSize);
        float f = 0f;
        Vector2 off = new Vector2(offset.x, offset.y);
        f = Unity_SimpleNoise_float(uv + off, scale);
        f -= 0.5f;
        f *= 10;
        float heat = (deadzone * f);
        return heat;
    }
}
