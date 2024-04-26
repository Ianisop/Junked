using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactor : MonoBehaviour
{
    //public GameObject currentTarget;
    public float range;
    bool popUp;
    Interactable interactable;
    List<IInteractableObserver> interactableObservers = new List<IInteractableObserver>();
    public Material outlineMat;
    Material oldMat; // Store the original material
    GameObject currentTarget;

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, range))
        {
            if (!hit.collider.gameObject.CompareTag("Ground"))
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


                if(currentTarget.GetComponent<TrashBag>() != null && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Home"))
                {
                    GameManager.Instance.PopUp(interactable.notification);
                }
                

                // Register the interactable object with the Interactor
                if (!interactableObservers.Contains(interactable.gameObject.GetComponent<IInteractableObserver>()))
                {
                    interactableObservers.Add(interactable.gameObject.GetComponent<IInteractableObserver>());
                }



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
            // Notify all registered interactable objects to perform their actions
            foreach (var observer in interactableObservers)
            {
                observer.Interact();
            }
        }
    }
}
