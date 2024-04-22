using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public enum TrashType
{
    Metal,
    Wood,
    Glass,
    Plastic
}

public class Trash : MonoBehaviour
{
    public static Trash Instance;
    public TrashType trashType;

    private void Awake()
    {
        if(Instance == this)
            Instance = this; 
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }


}

