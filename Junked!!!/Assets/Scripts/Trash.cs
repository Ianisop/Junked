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


    private void Awake()
    {
        if(Instance == this)
            Instance = this; 
        particles = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        cleanliness = Random.Range(4, 10);
    }



    public void Interact()
    {
        if(cleanliness >0)
        {
            cleanliness = -1;
            this.particles.Stop();
            this.particles.Play();
        }

        
    }



}

