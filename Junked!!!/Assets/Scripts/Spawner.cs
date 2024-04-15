using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject trashPrefab;
    public Vector3 spawnPoint;
    public Timer spawnerTimer;

    void Start()
    {
        GameManager.Instance._timerHandler.AddTimer(spawnerTimer, false);
    }

    void Update()
    {

        if(spawnerTimer.isDone)
        {
            int x = Random.Range(5, 10);
            Spawn();    
            
        }
    

    }



    public void Spawn()
    {
        
       
        Instantiate(trashPrefab, spawnPoint, Quaternion.identity);
        //transform.Translate(new Vector3(obj.transform.position.x + 10, obj.transform.position.y, obj.transform.position.z + 10) * 2);
         
        // obj.GetComponent<Rigidbody>().AddForce(transform.up);


    }
}
