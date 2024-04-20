using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Interactable Instance;
    public string notification;

    private void Awake()
    {
        if(Instance != this)
        {
            Instance = this;
        }
    }



}
