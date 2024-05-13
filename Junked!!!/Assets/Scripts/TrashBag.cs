using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrashBag : PickUp
{
    public GameObject[] trashPieces;
    UnityEngine.SceneManagement.Scene oldScene;
    public bool isOpen;
    public float totalWeight, maxWeight;
    public List<Trash> inventory = new List<Trash>();
    Color debugColor;
    private Animator animator;
    public ScrapSpawner __ss;
    public static TrashBag Instance;
    public DragRigidbody dr;
    // public string actionKey;
    public void Awake()
    {
        isOpen = false;
        if (Instance != this) Instance = this;
    }

    public void Start()
    {
        if(GetComponent<Rigidbody>() == null) { this.AddComponent<Rigidbody>(); }
        oldScene = SceneManager.GetActiveScene();
        animator = GetComponent<Animator>();
    }


    float timeSinceLastDrop = 0.0f;
    public void Update()
    {
        if (oldScene != SceneManager.GetActiveScene())
        {
            oldScene = SceneManager.GetActiveScene();
            transform.position = new Vector3(0,2,0);
            
        }

        // print(GetComponent<Rigidbody>().velocity.sqrMagnitude);
        timeSinceLastDrop += Time.deltaTime;
        if (isOpen && timeSinceLastDrop > 0.25f && GetComponent<Rigidbody>().velocity.sqrMagnitude >= 40 && inventory.Count > 0)
        {
            timeSinceLastDrop = 0.0f;
            RemoveItem();
        }
        

        animator.SetBool("IsOpen", isOpen);
    }

    void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward, debugColor);
        Debug.DrawLine(transform.position, transform.position + GetComponent<Rigidbody>().velocity);
    }


    public override void Interact()
    {
        isOpen = !isOpen;

    }

    public void AddItem(Trash item)
    {
        if ((item.weight + totalWeight) > maxWeight)
            print("item too fat");
            
        else
        {
            print("added item: " + item.weight + item.name);
            totalWeight += item.weight;
            inventory.Add(item);
            if (inventory.Contains(item)) item.gameObject.SetActive(false);
           
        }

    }

    public void RemoveItem()
    {
        Trash trash = inventory[inventory.Count - 1];
       
        if(trash)
        {
            totalWeight -= trash.weight;
            trash.gameObject.SetActive(true);
            trash.gameObject.transform.position = transform.position;
            inventory.Remove(trash);
            
            
        }

    }

    public void IncreaseTrashBagSize(int extraSize)
    {
        maxWeight += extraSize;
    }

}
