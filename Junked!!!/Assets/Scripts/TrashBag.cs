using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBag : MonoBehaviour, IInteractableObserver
{
    public int size; // the number of times it can give out tras
    public GameObject[] trashPieces;
    

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


    public void Cut()
    {
        if(size <= 0) 
        {
            Destroy(this.gameObject);
            
        }
        else
        {
            int x = Random.Range(0, trashPieces.Length);
            var obj = Instantiate(trashPieces[x],transform.position, transform.rotation);
            obj.GetComponent<Rigidbody>().AddForce(transform.up * 200f);
            size -= 1;
        }
    }

    public void Interact()
    {
        Cut();
    }
    
}
