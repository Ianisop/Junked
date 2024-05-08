using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SellZone : MonoBehaviour
{
    public LayerMask m_LayerMask;
    public BoxCollider m_Collider;
    public List<Collider> objectsInside;
    public QuotaSystem quotaSystem;

    void Update()
    {
        if (Input.GetKeyDown("8")) // TODO - Needs to run when pressing ingame button
        {
            SellItemsInSellZone();
        }
    }

    void SellItemsInSellZone()
    {
        //print("Checking collisions");
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        Collider[] hitColliders = Physics.OverlapBox(transform.position + m_Collider.center, m_Collider.size, m_Collider.transform.rotation, 1 << LayerMask.NameToLayer("Pickup"));//
        foreach (var a in hitColliders)
        {
            //Update quota
            quotaSystem.AddValue(a.transform.GetComponent<Trash>().moneyValue, a.transform.GetComponent<Trash>().CO2PrKg);
            Destroy(a.gameObject);
        }
    }
}
