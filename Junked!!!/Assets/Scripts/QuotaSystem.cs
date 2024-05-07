using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuotaSystem : MonoBehaviour
{
    public float currentQuota = 100f;
    // Easy = 20, Medium = 16, Hard = 12
    // If slider, between 20 and 10.
    public int difficulty = 16;
    

    public void UpdateQuota(int day)
    {
        currentQuota = 100 * (1 + Mathf.Pow(day, 2) / difficulty) * (Random.Range(0, 0.25f) + 1);
        print(currentQuota);
    }
}
