using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Interactable Instance;
    public string notification;
    public KeyCode actionKey;
    
    private void Awake()
    {
        if(Instance != this)
        {
            Instance = this;
        }
    }

    public void Call(string ans)
    {
        
    }



}
