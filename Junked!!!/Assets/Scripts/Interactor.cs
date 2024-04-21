using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public GameObject currentTarget;
    public float range;
    bool popUp;
    Interactable interactable;
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, range))
        {
            if(!hit.collider.gameObject.CompareTag("Ground"))
            {
                interactable = hit.collider.gameObject.GetComponent<Interactable>();
                GameManager.Instance.PopUp(interactable.notification);
                popUp = true;
            }
            

        }
        else
        {
            GameManager.Instance.PopUp(" ");
            popUp = false;
        }

        if(popUp && Input.GetKeyDown(interactable.actionKey))
        {
            interactable.Call();
        }

    }
}
