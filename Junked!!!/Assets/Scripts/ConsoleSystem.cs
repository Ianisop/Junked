using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleSystem : MonoBehaviour
{
    [SerializeField] ConsoleInputHandler m_inputHandler;
    [SerializeField] Animator m_consoleAnimator;

    public void GoToMenu(string menuName)
    {
        switch (menuName.ToLower())
        {
            case "main":
                m_consoleAnimator.SetBool("Menu/Map", false);
                m_consoleAnimator.SetBool("Menu/Sell", false);
                break;
            case "map":
                m_consoleAnimator.SetBool("Menu/Map", true);
                break;
            case "sell":
                m_consoleAnimator.SetBool("Menu/Sell", true);
                break;
        }
    }
}
