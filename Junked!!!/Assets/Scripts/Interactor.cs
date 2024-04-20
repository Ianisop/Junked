using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public GameObject currentTarget;
    public float range;
    
    
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, range))
        {
            if(!hit.collider.gameObject.CompareTag("Ground"))
            {
                GameManager.Instance.PopUp(hit.collider.gameObject.GetComponent<Interactable>().notification);
            }
            

        }
        else
        {
            GameManager.Instance.PopUp(" ");
        }

    }
}
