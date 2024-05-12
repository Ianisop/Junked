using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuotaSystem : MonoBehaviour
{
    // Variables
    public float moneyQuota = 100;
    public float co2Quota = 1000;
    public float currentMoney = 0;
    public float currentCO2 = 0;
    public TMP_Text co2QuotaDisplay, moneyQuotaDisplay, currentEventText;

    // Easy = 20, Medium = 16, Hard = 12
    // If slider, between 20 and 10.
    public int difficulty = 16;

    private void Start()
    {
        UpdateQuota(0);
        currentEventText.text = "";
    }

    public void UpdateQuota(int day) // Updates the quota and handles random events
    {
        if(day == 0)
        {
            moneyQuota = 100;
            co2Quota = 150;
            UpdateQuotaUI();
            return;
        }

        // Just a random chance from 0-100%
        int chance = Random.Range(0, 100);

        if (chance <= 30)
        {
            // TODO - This print statement needs to be visible ingame
            currentEventText.text = "This trash-patch is from young people, who notoriously don't sort their garbage. Expect a higher quota";

            moneyQuota = 100 * (1 + Mathf.Pow(day, 2) / difficulty) * (Random.Range(0.25f, 0.5f) + 1);
            co2Quota = 150 * (1 + Mathf.Pow(day, 2) / difficulty) * (Random.Range(0.25f, 0.5f) + 1);
            print("Money Quota: " + moneyQuota + " + " + "CO2 Quota " + co2Quota);


            StartCoroutine(eventDisplayTime());

        }
        else
        {
            moneyQuota = 100 * (1 + Mathf.Pow(day, 2) / difficulty) * (Random.Range(0, 0.12f) + 1);
            co2Quota = 150 * (1 + Mathf.Pow(day, 2) / difficulty) * (Random.Range(0, 0.12f) + 1);
            print("Money Quota: " + moneyQuota + " + " + "CO2 Quota " + co2Quota);
        }
        UpdateQuotaUI();
    }

    public void AddValue(float moneyValue, float co2Value) // Function for adding new values in other scripts
    {
        currentMoney += moneyValue;
        currentCO2 += co2Value;
        print("Current Money: " + currentMoney + " + " + "Current CO2 " + currentCO2);
        UpdateQuotaUI();
    }
    
    public bool CheckQuota() // Checks if the current quotas are met and returns bool
    {
        if ((int)currentMoney >= (int)moneyQuota && (int)currentCO2 >= (int)co2Quota)
        {
            return true;
        }
        return false;
    }

    public void UpdateQuotaUI()
    {
        co2QuotaDisplay.text = "<size=2>CO2 Quota:\n" + (int)currentCO2 + "kg/" + (int)co2Quota + "kg";
        moneyQuotaDisplay.text = "<size=2>Money Quota:\n" + (int)currentMoney + "$/" + (int)moneyQuota + "$";
    }

    public IEnumerator eventDisplayTime()
    {
        yield return new WaitForSeconds(10);
        currentEventText.text = "";
        yield return null;
    }
}
