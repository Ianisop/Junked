using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    public GameObject[] trashPrefabs;
    Vector3 spawnPoint;
    Mesh mesh;
    Vector3[] verts;
    int[] ids;

    void Start()
    {
        NavMeshTriangulation triangles = NavMesh.CalculateTriangulation();
        mesh = new Mesh();
        mesh.vertices = triangles.vertices;
        mesh.triangles = triangles.indices;

        Spawn(10);
    }


    public void Spawn(int amount)
    {
        for(int i = 0; i< amount; i++)
        {
            int x = Random.Range(0, trashPrefabs.Length);
            spawnPoint = GetRandomPointFromNavMesh();
            Instantiate(trashPrefabs[x], spawnPoint, Quaternion.identity);
        }
        

    }

    public Vector3 GetRandomPointFromNavMesh()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        // Step 1: Retrieve NavMesh bounds
        Vector3 navMeshBoundsMin = navMeshData.vertices[0];
        Vector3 navMeshBoundsMax = navMeshData.vertices[0];

        for (int i = 1; i < navMeshData.vertices.Length; i++)
        {
            navMeshBoundsMin = Vector3.Min(navMeshBoundsMin, navMeshData.vertices[i]);
            navMeshBoundsMax = Vector3.Max(navMeshBoundsMax, navMeshData.vertices[i]);
        }

        // Step 2: Generate random point
        Vector3 randomPoint = new Vector3(
            Random.Range(navMeshBoundsMin.x, navMeshBoundsMax.x),
            Random.Range(navMeshBoundsMin.y, navMeshBoundsMax.y),
            Random.Range(navMeshBoundsMin.z, navMeshBoundsMax.z)
        );

        // Step 3: Project point onto NavMesh
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(randomPoint, out navMeshHit, 1.0f, NavMesh.AllAreas))
        {
            return navMeshHit.position;
        }
        else
        {
            // If the random point is not on the NavMesh, try again or handle accordingly
            return Vector3.zero;
        }
    }

}


