using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject trashPrefab;
    public Vector3 spawnPoint;


    void Start()
    {

        Spawn(200);
    }


    public void Spawn(int amount)
    {
        Instantiate(trashPrefab, spawnPoint, Quaternion.identity);

    }
}


