using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interactor : MonoBehaviour
{
    //public GameObject currentTarget;
    public float range;
    bool popUp;
    public Material outlineMat;
    Material oldMat; // Store the original material
    GameObject currentTarget;
    public LayerMask layerMask;
    public GameObject oldTarget;

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, range, layerMask))
        {
            currentTarget = hit.collider.gameObject;
            Trash trash = hit.collider.gameObject.GetComponent<Trash>();
            trash.UpdateUI();
            
                
        }
        else
        {
            if(currentTarget)
            {
                oldTarget = currentTarget;
                currentTarget.GetComponent<Trash>().popUp.animator.SetBool("hover", false);

                currentTarget = null;
            }

        }
    }
}
