using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrashBag : PickUp
{
    public GameObject[] trashPieces;
    UnityEngine.SceneManagement.Scene oldScene;
    public bool isOpen;
    public int totalWeight, maxWeight;
    public List<Trash> inventory = new List<Trash>();
    Color debugColor;
    public PopUp popUp;
    private Animator animator;
   // public string actionKey;
    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        isOpen = false;
        popUp = this.AddComponent<PopUp>();
        maxWeight = 15;
    }

    public void Start()
    {
        oldScene = SceneManager.GetActiveScene();
        animator = GetComponent<Animator>();
    
        popUp.Setup();
    }

    public void Update()
    {
        if (oldScene != SceneManager.GetActiveScene())
        {
            oldScene = SceneManager.GetActiveScene();
            transform.position = new Vector3(0,2,0);
            
        }

       // print(GetComponent<Rigidbody>().velocity.sqrMagnitude);

        debugColor = Color.red;

        if(transform.forward.y < -0.8)
        {
            debugColor = Color.green;
            if (isOpen && GetComponent<Rigidbody>().velocity.sqrMagnitude >= 1.2 && inventory.Count > 0)
            {
                RemoveItem();
            }
        }

        animator.SetBool("IsOpen", isOpen);
    }

    void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward, debugColor);
    }

    public override void UpdateMe()
    {
        
        popUp.animator.SetBool("hover", true);
        popUp.text.text = "Black Hole\n" + totalWeight + "/" + maxWeight;
        popUp.canvas.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward, Camera.main.transform.up);


    }

    public override void Interact()
    {
        isOpen = !isOpen;

    }

    public void AddItem(Trash item)
    {
        totalWeight += item.weight;
        inventory.Add(item);
        
        item.gameObject.SetActive(false);
    }

    public void RemoveItem()
    {
        Trash trash = inventory[inventory.Count - 1];
       
        if(trash)
        {
            totalWeight -= trash.GetComponent<Trash>().weight;
            trash.transform.position = transform.position;
            trash.gameObject.SetActive(false);
        }

    }

}
