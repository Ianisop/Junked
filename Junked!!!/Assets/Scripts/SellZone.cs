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
    public ConsoleSystem consoleSystem;


    public void SellItemsInSellZone()
    {
        TrashType type = consoleSystem.GetSelectedTrashType();
        //TrashType.TryParse(1, out type);
        if (type  == TrashType.None)
        {
            Debug.LogException(new System.Exception("Invalid TrashType in \"SellItemsInSellZone\""));
        }
        print("Selling: " + type);

        //print("Checking collisions");
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        Collider[] hitColliders = Physics.OverlapBox(transform.position + m_Collider.center, m_Collider.size/2, m_Collider.transform.rotation, 1 << LayerMask.NameToLayer("Pickup"));
        foreach (var a in hitColliders)
        {
            var objectInGame = a.transform.GetComponent<Trash>();
            //Update quota
            if (objectInGame == null) continue;
            if (objectInGame.trashType == type)
            {
                quotaSystem.AddValue(objectInGame.moneyValue, objectInGame.CO2PrKg);
                print(objectInGame.name + " right type");
            }
            else
            {
                quotaSystem.AddValue(-objectInGame.moneyValue, -objectInGame.CO2PrKg);
                print(objectInGame.name + " wrong type");
            }

            Destroy(a.gameObject);
        }
    }

}

