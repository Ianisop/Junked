using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Container : MonoBehaviour
{

    [Tooltip("make sure its lowercase")] public string color;
    
    
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(color))
        {

            Destroy(other.gameObject);
        }
        else
        {

            Destroy(other.gameObject);
        }
    }
}
