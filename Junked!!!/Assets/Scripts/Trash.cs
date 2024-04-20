using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public static Trash Instance;
    private Animator _animator;
    private Rigidbody rb;
    public int size;


    public void Awake()
    {
        if(Instance == null) Instance = this;
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

    }




    public void Pulse()
    {
        GetComponent<SphereCollider>().enabled = true;
        rb.AddForce((transform.up + transform.forward) * 300);
        _animator.enabled = false;


    }
}

