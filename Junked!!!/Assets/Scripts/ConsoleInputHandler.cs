/*

MIT License

Copyright (c) 2021 Iain McManus

https://github.com/GameDevEducation/DevToolkit_InteractiveScreens/

NOTICE:
    OUR IMPLEMENTATION HAS BEEN EDITED
 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConsoleInputHandler : MonoBehaviour
{
    public GameObject screen;
    public LayerMask rayMask;
    [SerializeField] GraphicRaycaster raycaster;
    [SerializeField] RectTransform canvasTransform;
    [SerializeField] CanvasScaler canvasScaler;
    List<GameObject> dragTargets = new List<GameObject>();


    void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        canvasTransform = GetComponent<RectTransform>();
        canvasScaler = GetComponent<CanvasScaler>();
    }



    // Update is called once per frame
    void Update()
    {

        var cam = Camera.main;
        Ray camRay = new Ray(cam.transform.position, cam.transform.forward);

        RaycastHit hitResult;
        if (Physics.Raycast(camRay, out hitResult, 2, rayMask))
        {

            Vector2 uvPosition = hitResult.textureCoord;
            Vector3 mousePosition = new Vector3(canvasTransform.sizeDelta.x * uvPosition.x,
                                            canvasTransform.sizeDelta.y * uvPosition.y,
                                            0f);

            // construct our pointer event
            PointerEventData mouseEvent = new PointerEventData(EventSystem.current);
            mouseEvent.position = mousePosition * canvasScaler.scaleFactor;
            
            // perform a raycast using the graphics raycaster
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(mouseEvent, results);

            bool sendMouseDown = Input.GetMouseButtonDown(0);
            bool sendMouseUp = Input.GetMouseButtonUp(0);
            bool isMouseDown = Input.GetMouseButton(0);

            // send through end drag events as needed
            if (sendMouseUp)
            {
                foreach (var target in dragTargets)
                {
                    if (ExecuteEvents.Execute(target, mouseEvent, ExecuteEvents.endDragHandler))
                        break;
                }
                dragTargets.Clear();
            }

            // process the raycast results
            foreach (var result in results)
            {
                // setup the new event data
                PointerEventData eventData = new PointerEventData(EventSystem.current);
                eventData.position = mousePosition;
                eventData.pointerCurrentRaycast = eventData.pointerPressRaycast = result;

                // is the mouse down?
                if (isMouseDown)
                    eventData.button = PointerEventData.InputButton.Left;

                var slider = result.gameObject.GetComponentInParent<UnityEngine.UI.Slider>();

                // potentially new drag targets?
                if (sendMouseDown)
                {
                    if (ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.beginDragHandler))
                        dragTargets.Add(result.gameObject);

                    if (slider != null)
                    {
                        slider.OnInitializePotentialDrag(eventData);

                        if (!dragTargets.Contains(result.gameObject))
                            dragTargets.Add(result.gameObject);
                    }
                } // need to update drag target
                else if (dragTargets.Contains(result.gameObject))
                {
                    eventData.dragging = true;
                    ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.dragHandler);
                    if (slider != null)
                    {
                        slider.OnDrag(eventData);
                    }
                }

                // send a mouse down event?
                if (sendMouseDown)
                {
                    if (ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.pointerDownHandler))
                        break;
                } // send a mouse up event?
                else if (sendMouseUp)
                {
                    bool didRun = ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.pointerUpHandler);
                    didRun |= ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.pointerClickHandler);

                    if (didRun)
                        break;
                }
            }
        }
        else
        {
        }
    }
}
