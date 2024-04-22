using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRigidbody : MonoBehaviour
{
    public float forceAmount = 500;

    public Rigidbody selectedRigidbody;
    Camera targetCamera;
    Vector3 originalScreenTargetPosition;
    Vector3 originalRigidbodyPos;
    public float selectionDistance;
    public float pickUpRadius;

    // Material variables
    Material originalMaterial;
    public Material outlineMaterial;

    // Start is called before the first frame update
    void Start()
    {
        targetCamera = Camera.main;
    }

    void Update()
    {
        if (!targetCamera)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            // Check if we are hovering over Rigidbody, if so, select it
            selectedRigidbody = GetRigidbodyFromMouseClick();
            if (selectedRigidbody)
            {
                // Store a new instance of the original material of the selected Rigidbody's GameObject
                MeshRenderer meshRenderer = selectedRigidbody.GetComponent<MeshRenderer>();
                if (meshRenderer)
                {
                    originalMaterial = meshRenderer.sharedMaterial;
                    Debug.Log("Original material set: " + originalMaterial.GetInstanceID());
                    Debug.Log("Outline material: " + outlineMaterial.GetInstanceID());
                }
            }
        }
        if (Input.GetMouseButtonUp(0) && selectedRigidbody)
        {
            // Revert the material to the original one
            if (originalMaterial)
            {
                MeshRenderer meshRenderer = selectedRigidbody.GetComponent<MeshRenderer>();
                if (meshRenderer)
                {
                    meshRenderer.sharedMaterial = originalMaterial;
                    Debug.Log("Material reverted to: " + originalMaterial.GetInstanceID());
                }
            }
            selectedRigidbody = null;
        }
    }




    void FixedUpdate()
    {
        if (selectedRigidbody && Vector3.Distance(selectedRigidbody.transform.position, transform.position) <= pickUpRadius)
        {
            Vector3 mousePositionOffset = targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance)) - originalScreenTargetPosition;
            selectedRigidbody.velocity = (originalRigidbodyPos + mousePositionOffset - selectedRigidbody.transform.position) * forceAmount * Time.deltaTime;

        }
    }

    Rigidbody GetRigidbodyFromMouseClick()
    {
        RaycastHit hitInfo = new RaycastHit();
        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(ray, out hitInfo);
        if (hit)
        {
            if (hitInfo.collider.gameObject.GetComponent<Rigidbody>())
            {
                selectionDistance = Vector3.Distance(ray.origin, hitInfo.point);
                originalScreenTargetPosition = targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance));
                originalRigidbodyPos = hitInfo.collider.transform.position;
                return hitInfo.collider.gameObject.GetComponent<Rigidbody>();
            }
        }

        return null;
    }
}
