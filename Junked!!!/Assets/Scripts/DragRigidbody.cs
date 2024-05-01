using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRigidbody : MonoBehaviour
{
    public float forceAmount = 500;

    public Rigidbody selectedRigidbody;
    Camera targetCamera;
    Vector3 originalScreenTargetPosition;
    Vector3 originalRigidbodyPos, originalRigidbodyRot;
    public float selectionDistance;
    public float pickUpRadius;
    Vector3 oldMousePos, mousePositionOffset;
    // Material variables
    Material originalMaterial;
    public Material outlineMaterial;
    float rotateSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        targetCamera = Camera.main;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.R))
        {
            Cursor.lockState = CursorLockMode.Confined;
            if (selectedRigidbody) selectedRigidbody.isKinematic = true;
        }
        if(!Input.GetKey(KeyCode.R))
        {
            Cursor.lockState = CursorLockMode.Locked;
            if (selectedRigidbody) selectedRigidbody.isKinematic = false;
        }
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
                    //Debug.Log("Original material set: " + originalMaterial.GetInstanceID());
                   // Debug.Log("Outline material: " + outlineMaterial.GetInstanceID());
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
                    //Debug.Log("Material reverted to: " + originalMaterial.GetInstanceID());
                }
            }
            selectedRigidbody = null;
        }
    }




    void FixedUpdate()
    {
        mousePositionOffset = targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance)) - originalScreenTargetPosition;
        print(mousePositionOffset);
        if (selectedRigidbody && Vector3.Distance(selectedRigidbody.transform.position, transform.position) <= pickUpRadius && !Input.GetKey(KeyCode.R))
        {
            MoveRigidbody();


        }

        if (selectedRigidbody && Vector3.Distance(selectedRigidbody.transform.position, transform.position) <= pickUpRadius && Input.GetKey(KeyCode.R))
        {

            RotateRigidbody();

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
                originalRigidbodyRot = hitInfo.collider.transform.rotation.eulerAngles;
                return hitInfo.collider.gameObject.GetComponent<Rigidbody>();
            }
        }

        return null;
    }

    void RotateRigidbody()
    {
        
        Vector3 mouseDelta = Input.mousePosition - oldMousePos;
        
        selectedRigidbody.gameObject.transform.Rotate(Camera.main.transform.up, mouseDelta.x * rotateSpeed, Space.World);
        selectedRigidbody.gameObject.transform.Rotate(Camera.main.transform.right, mouseDelta.y * rotateSpeed , Space.World);

        
        GameManager.Instance.playerColl.gameObject.GetComponent<PlayerMovement>().enabled = false;

        oldMousePos = Input.mousePosition;
        print(mouseDelta);
    }

    void MoveRigidbody()
    {
        oldMousePos = mousePositionOffset;

        selectedRigidbody.velocity = (originalRigidbodyPos + mousePositionOffset - selectedRigidbody.transform.position) * forceAmount * Time.deltaTime;
        GameManager.Instance.playerColl.gameObject.GetComponent<PlayerMovement>().enabled = true;
    }
}
