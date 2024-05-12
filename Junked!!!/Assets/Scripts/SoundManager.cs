using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource footStepSource, pickUpSource;
    public static SoundManager Instance;

    private void Awake()
    {
        if(Instance!=this) { Instance = this; }
    }


    public void PlayFootStep()
    {
        if(!footStepSource.isPlaying)
        {
            footStepSource.pitch = Random.Range(0.6f,1f);
            footStepSource.Play();
        }
        
    }

    public void PlayPickUpSound()
    {
        if(!pickUpSource.isPlaying) 
        {
            pickUpSource.pitch = Random.Range(0.8f, 1f); ;
            pickUpSource.Play();
        }
    }





}
