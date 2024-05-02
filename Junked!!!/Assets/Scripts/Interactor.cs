using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactor : MonoBehaviour
{
    //public GameObject currentTarget;
    public float range;
    bool popUp;
    Interactable interactable;
    public Material outlineMat;
    Material oldMat; // Store the original material
    GameObject currentTarget;

    void Update()
    {
        return;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, range))
        {
            if (!hit.collider.gameObject.CompareTag("Ground")&& interactable == null)
            {
                popUp = true;
                interactable = hit.collider.gameObject.GetComponent<Interactable>();
                MeshRenderer meshRenderer = hit.collider.gameObject.GetComponent<MeshRenderer>();

                currentTarget = hit.collider.gameObject;

                // Store the original material if it's not already set
                if (meshRenderer.sharedMaterial != outlineMat)
                {
                    oldMat = meshRenderer.sharedMaterial;
                    meshRenderer.sharedMaterial = outlineMat; // Apply the outline material
                }

                 GameManager.Instance.PopUp(interactable.notification);
               

            }
        }
        else
        {
            // Revert to the original material if there was a previous interactable object
            if (interactable != null)
            {
                interactable.GetComponent<MeshRenderer>().sharedMaterial = oldMat;
                interactable = null; // Reset interactable reference
            }
            GameManager.Instance.PopUp(" ");
            currentTarget = null;
            popUp = false;
        }

        if (popUp && Input.GetKeyDown(interactable.actionKey))
        {

            interactable.GetComponent<IInteractableObserver>().Interact();

        }
    }
}
