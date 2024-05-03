using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuotaSystem : MonoBehaviour
{
    public float currentQuota = 100f;
    public float day = 0f;
    // Easy = 20, Medium = 16, Hard = 12
    // If slider, between 20 and 10.
    public int difficulty = 16;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateQuota();
    }

    public void UpdateQuota()
    {
        if (Input.GetKeyDown("3"))
        {
            // Day can be changed to each time quota is fulfilled
            day += 1f;
            // Math formula from Lethal Company
            currentQuota = 100 * (1 + Mathf.Pow(day, 2) / difficulty)* (Random.Range(0,1) + 1);
            

        }


        //return (int)Mathf.Round(currentQuota);
    }
}
