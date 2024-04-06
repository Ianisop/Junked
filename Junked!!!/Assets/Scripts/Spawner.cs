using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] trashPrefabs;
    public Vector3 spawnPoint;
    public float spawnTime;
    public float next;

    void Start()
    {
        
    }

    void Update()
    {
        next += Time.deltaTime;
        if(next >= spawnTime)
        {
            int x = Random.Range(0, trashPrefabs.Length);
            Spawn(x);    
            next = 0;
        }
    

    }



    public void Spawn(int cube)
    {

        GameObject obj;
        
        obj = trashPrefabs[cube];
       
        Instantiate(obj, spawnPoint, Quaternion.identity);
        //transform.Translate(new Vector3(obj.transform.position.x + 10, obj.transform.position.y, obj.transform.position.z + 10) * 2);
         
        // obj.GetComponent<Rigidbody>().AddForce(transform.up);


    }
}
