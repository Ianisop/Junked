using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum TrashType
{
    None = 0,
    Wood = 1,
    Plastic = 2,
    Metal = 3,
    Glass = 4,
    Cardboard = 5
} // these need to be set in the order that they are set in the console ui



public class Trash : PickUp
{
    public static Trash Instance;
    public TrashType trashType;
    public float cleanliness;
    public float CO2PrKg;
    public float moneyValue;
    public ParticleSystem particles;
    public float weight;
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

        CO2PrKg *= weight;
        moneyValue *= weight;
    }

    private void Start()
    {
        cleanliness = Random.Range(4, 10);
        weight = Random.Range(3,15);
        
    }

    void Update()   
    {
        if (!TrashBag.Instance.dr.selectedRigidbody == this.GetComponent<Rigidbody>())
        {
            return;
        }
        if (TrashBag.Instance.isOpen && Vector3.Distance(this.transform.position, TrashBag.Instance.transform.position) <= 1 && TrashBag.Instance.dr.selectedRigidbody != TrashBag.Instance.GetComponent<Rigidbody>())
        {
            TrashBag.Instance.AddItem(this);
            
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

    public void CopyTo(Trash trash)
    {
        this.weight = trash.weight;
        this.trashType = trash.trashType;
        this.cleanliness = trash.cleanliness;
        this.moneyValue = trash.moneyValue;
        this.CO2PrKg = trash.CO2PrKg;

    }



}

