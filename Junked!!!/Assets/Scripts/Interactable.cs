using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractableObserver
{
    void Interact();

}


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



}
