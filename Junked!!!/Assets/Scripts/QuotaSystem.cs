using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuotaSystem : MonoBehaviour
{
    // Variables
    public float moneyQuota = 100f;
    public float co2Quota = 1000f;
    public float currentMoney = 0f;
    public float currentCO2 = 0f;

    // Easy = 20, Medium = 16, Hard = 12
    // If slider, between 20 and 10.
    public int difficulty = 16;
    
    

    public void UpdateQuota(int day)
    {
        moneyQuota = 100 * (1 + Mathf.Pow(day, 2) / difficulty) * (Random.Range(0, 0.25f) + 1);
        co2Quota = 1000 * (1 + Mathf.Pow(day, 2) / difficulty) * (Random.Range(0, 0.25f) + 1);
        print(moneyQuota);
    }

    public void AddValue(float moneyValue, float co2Value)
    {
        currentMoney += moneyValue;
        currentCO2 += co2Value;
        print(currentMoney + " + " + currentCO2);
        
    }
    
    public bool CheckQuota()
    {
        if (currentMoney >= moneyQuota && currentCO2 >= co2Quota)
        {
            return true;
        }
        return false;
    }
}
