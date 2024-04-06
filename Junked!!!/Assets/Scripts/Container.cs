using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Container : MonoBehaviour
{

    [Tooltip("make sure its lowercase")] public string color;
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(color))
        {
            GameManager.Instance.GivePoint();
            Destroy(other.gameObject);
        }
    }
}
