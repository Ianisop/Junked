using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] trashPrefabs;
    public Transform spawnPoint;
    public float spawnTime;
    float next;

    void Start()
    {
        
    }

    void Update()
    {
        next -= Time.deltaTime;
        if(next <= spawnTime)
        {
            int x = Random.Range(0, trashPrefabs.Length);
            Spawn(x);    
            next = spawnTime;
        }
    

    }



    public void Spawn(int cube)
    {
        GameObject obj = null;
        
        obj = trashPrefabs[cube];

        Instantiate(obj, spawnPoint);
        obj.GetComponent<Rigidbody>().AddForce(transform.forward*200f);
        

    }
}
