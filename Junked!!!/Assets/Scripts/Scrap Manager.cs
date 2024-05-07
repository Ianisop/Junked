using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
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

    void Awake()
    {
        m_scrapSpawnMesh = m_scrapSpawnMeshObject.GetComponent<MeshFilter>().mesh;
        generateRandom(m_seed);
        generateScrapSpawns();
        populateSpawns();
    }

    public void generateRandom(int seed)
    {
        if (seed == 0) seed = 1337;

        m_seedOffset.x = seed & 0b0101010101010101;
        m_seedOffset.y = seed & 0b1010101010101010;

        m_seedOffset = m_seedOffset.normalized * ((seed % 10) + 10);
    }

    public void cleanScrap()
    {
    }

    public void generateScrapSpawns()
    {
        Vector3[] vertices = m_scrapSpawnMesh.vertices;
        m_scrapSpawnMeshObject.transform.TransformPoints(vertices);

        RenderTexture doubleBufferRT = m_heatRenderTexture.GetDoubleBufferRenderTexture();
        Vector2 bufferDimensions = new Vector2(doubleBufferRT.width, doubleBufferRT.height);

        Texture2D tex = new Texture2D(doubleBufferRT.width, doubleBufferRT.height, TextureFormat.RGBAFloat, false);

        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = doubleBufferRT;
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0, false);
        tex.Apply();
        Color[] heatPixels = tex.GetPixels();
        RenderTexture.active = currentActiveRT;

        foreach (var vertex in vertices)
        {
            Vector2 normalPos = new Vector2((vertex.x + 100) / 200, (vertex.y + 100) / 200);
            int bufferIndex = (int)(normalPos.x * bufferDimensions.x) + (int)((int)(normalPos.y * bufferDimensions.y) * bufferDimensions.x);
            if (bufferIndex >= heatPixels.Length) {
                Debug.LogError("OUT OF BOUNDS HEATMAP INDEX!");
                continue;
            }
            print("color at index{" + bufferIndex + "}: " + heatPixels[bufferIndex]);
            float ran = Random.Range(0.0f, 1.0f);
            print("ran: " + ran);
            if ( ran < heatPixels[bufferIndex].r)
            {
                m_spawnLocations.Add(vertex);
            }
        }


    }

    public void populateSpawns()
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
}
