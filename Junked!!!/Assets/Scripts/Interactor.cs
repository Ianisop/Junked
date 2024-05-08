using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactor : MonoBehaviour
{
    //public GameObject currentTarget;
    public float range;
    GameObject currentTarget;
    public LayerMask layerMask;
    public GameObject oldTarget;
    PickUp trash;

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, range, layerMask))
        {
            currentTarget = hit.collider.gameObject;
            trash = hit.collider.gameObject.GetComponent<PickUp>();

            trash.UpdateMe();
            if(currentTarget.GetComponent<MeshRenderer>())currentTarget.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Enabled", 1);
            if(!currentTarget.GetComponent<MeshRenderer>())currentTarget.GetComponent<SkinnedMeshRenderer>().sharedMaterial.SetFloat("_Enabled", 1);
                
        }
        else
        {
            if(currentTarget)
            {
                oldTarget = currentTarget;

                if(trash)trash.GetComponent<PopUp>().animator.SetBool("hover", false);

                if (currentTarget.GetComponent<MeshRenderer>()) currentTarget.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_Enabled", 0);
                if (!currentTarget.GetComponent<MeshRenderer>()) currentTarget.GetComponent<SkinnedMeshRenderer>().sharedMaterial.SetFloat("_Enabled", 0);

                trash = null;
                currentTarget = null;
            }

        }

        if(currentTarget)
        {
            if (Input.GetButtonDown(trash.actionKey))
            {
                trash.Interact();
            }
        }

    }
}
