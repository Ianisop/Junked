using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrashBag : MonoBehaviour, IInteractableObserver
{
    Inventory inventory;
    public GameObject[] trashPieces;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }



    public void Cut()
    {
        if(inventory.inventorySlots.Count <= 0) 
        {
            Destroy(this.gameObject);
            
        }
        else
        {
            //spawn item from list
        }
    }

    public void Interact()
    {
        Cut();

    }
    
}
