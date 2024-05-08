using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class OverlapBoxExample : MonoBehaviour
{
    public LayerMask m_LayerMask;
    public BoxCollider m_Collider;
    public List<Collider> objectsInside;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown("8"))
        {
            MyCollisions();

        }
    }

    void MyCollisions()
    {
        print("Checking collisions");
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        Collider[] hitColliders = Physics.OverlapBox(transform.position + m_Collider.center, m_Collider.size, m_Collider.transform.rotation, 1 << LayerMask.NameToLayer("Pickup"));//
        //Check when there is a new collider coming into contact with the box
        foreach (var a in hitColliders)
        {
            //Output all of the collider names
            print(a.name);
            //Increase the number of Colliders in the array
            Destroy(a.gameObject);
        }
    }
}
