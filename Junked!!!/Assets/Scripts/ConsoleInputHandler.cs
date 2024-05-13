using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConsoleInputHandler : MonoBehaviour
{
    public GameObject screen;
    public LayerMask rayMask;
    public bool m_consoleMenuOpen = false;
    [SerializeField] GameObject m_consoleUI;

    void Start()
    {

    }

    public void CloseConsoleUI()
    {
        m_consoleMenuOpen=false;
    }

    void Update()
    {
        //free cursor if console menu is open, if not lock it
        if (m_consoleMenuOpen)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //display console ui if console is open
        m_consoleUI.SetActive(m_consoleMenuOpen);


        //raycast to check if the player clicks on the console screen
        var cam = Camera.main;
        Ray camRay = new Ray(cam.transform.position, cam.transform.forward);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitResult;
            if (Physics.Raycast(camRay, out hitResult, 2, rayMask))
            {
                m_consoleMenuOpen = true;
            }
        }
    }
}
