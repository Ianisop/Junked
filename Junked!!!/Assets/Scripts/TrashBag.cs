using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBag : MonoBehaviour
{
    public int size; // the number of times it can give out trash
    

    private void Start()
    {
        size = Random.Range(1, 5);
    }

    public void Pulse()
    {

        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Animator>().enabled = false;
        GetComponent<Rigidbody>().AddForce(transform.forward * 200f);
        
    }
    
}
