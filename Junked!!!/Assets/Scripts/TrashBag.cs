using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrashBag : MonoBehaviour, IInteractableObserver
{
    public GameObject[] trashPieces;
    UnityEngine.SceneManagement.Scene oldScene;
    public bool isOpen;
    public int totalWeight;
    public List<Trash> inventory = new List<Trash>();
    Color debugColor;
    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        isOpen = false;
    }

    public void Start()
    {
        oldScene = SceneManager.GetActiveScene();
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

    }

    void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward, debugColor);
    }



    public void Interact()
    {
        isOpen = !isOpen;

    }

    public void AddItem(Trash item)
    {
        totalWeight += item.weight;
        inventory.Add(item);
    }

    public void RemoveItem()
    {
        Trash trash = inventory[inventory.Count - 1];
        totalWeight -= trash.weight;
        Instantiate(trash.gameObject, transform);
        trash.gameObject.AddComponent<Rigidbody>();
        inventory.Remove(trash);
    }





}
