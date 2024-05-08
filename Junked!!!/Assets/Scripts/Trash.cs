using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum TrashType
{
    Metal,
    Glass,
    Wood,
    Cardboard,
    Plastic
}



public class Trash : PickUp
{
    public static Trash Instance;
    public TrashType trashType;
    public float cleanliness;
    public float CO2PrKg;
    public float moneyValue;
    public ParticleSystem particles;
    public int weight;
    public PopUp popUp;
    public string name;
    //public string actionKey;

    private void Awake()
    {
        if(Instance == this)
            Instance = this; 
        particles = GetComponentInChildren<ParticleSystem>();
        popUp = this.AddComponent<PopUp>();
        popUp.Setup();
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

    //called when raycaster looks at it
    public override void UpdateMe()
    {
        popUp.animator.SetBool("hover", true);
        popUp.text.text = popUp.name + "\n" + trashType.ToString() + "\n" + weight + "\n" + cleanliness;
        popUp.canvas.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward, Camera.main.transform.up);
       
       

    }
    
    public override void Interact()
    {

    }



}

