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
    public float CO2PrKg;
    public float moneyValue;
    public ParticleSystem particles;
    public float weight;

    public string name;
    //public string actionKey;

    private void Awake()
    {
        if(Instance == null)
            Instance = this; 

        CO2PrKg *= weight;
        moneyValue *= weight/10;
    }

    private void Start()
    {
        
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
            
    }

    public void CopyTo(Trash trash)
    {
        this.weight = trash.weight;
        this.trashType = trash.trashType;
        this.moneyValue = trash.moneyValue;
        this.CO2PrKg = trash.CO2PrKg;

    }



}

