using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
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
    public float weight;

    public string name;
    //public string actionKey;

    private void Awake()
    {
        if(Instance == this)
            Instance = this; 

        CO2PrKg *= weight;
        moneyValue *= weight;
    }

    private void Start()
    {
        cleanliness = UnityEngine.Random.Range(4, 10);
        weight = UnityEngine.Random.Range(3,15);
        
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

    
    public override void Interact()
    {
        cleanliness += UnityEngine.Random.Range(1, cleanliness);
        Mathf.Clamp(cleanliness, 1, 10);
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

