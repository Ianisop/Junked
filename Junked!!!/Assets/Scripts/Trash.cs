using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public enum TrashType
{
    Metal,
    Cardboard,
    Plastic
}

public class Trash : MonoBehaviour, IInteractableObserver
{
    public static Trash Instance;
    public TrashType trashType;
    public float cleanliness;
    public ParticleSystem particles;
    public int weight;



    private void Awake()
    {
        if(Instance == this)
            Instance = this; 
        particles = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        cleanliness = Random.Range(4, 10);
        weight = Random.Range(3,15);
    }

    void Update()
    {
        if (GameManager.Instance.trashBag.GetComponent<TrashBag>().isOpen && Vector3.Distance(this.transform.position, GameManager.Instance.trashBag.transform.position) <= 1 && GameManager.Instance.dragger.selectedRigidbody != GameManager.Instance.trashBag.GetComponent<Rigidbody>())
        {
            GameManager.Instance.trashBag.GetComponent<TrashBag>().AddItem(this);
            
        }
    }

    public void Interact()
    {

    }



}

