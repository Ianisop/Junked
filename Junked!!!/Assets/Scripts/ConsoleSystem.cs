using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class ConsoleSystem : MonoBehaviour
{
    [SerializeField] Transform m_player;
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

    [SerializeField] GameObject theLove;
    [SerializeField] GameObject theSmile;
    int cookieClicker = 0;
    bool spreadTheLove = false;
    public void DerLilaDrachen()
    {
        print("lila ser gut: " + cookieClicker);
        if (cookieClicker > 10)
        {
            spreadTheLove = true;
        }
        cookieClicker++;
    }

    float slowDownTheLove = 0f;
    bool spreadItEqually = false;
    public void Update()
    {
        if (spreadTheLove)
        {
            slowDownTheLove += Time.deltaTime;
            if (slowDownTheLove > 0.1f)
            {
                slowDownTheLove = 0f;
                Vector3 dir = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
                var whatToSpread = theLove;
                if (spreadItEqually)
                    whatToSpread = theSmile;
                GameObject love = Instantiate(whatToSpread, m_player.position + new Vector3(0, 3, 0), Quaternion.Euler(dir));
                love.transform.GetComponent<Rigidbody>().AddForce(dir / 30, ForceMode.Impulse);
                spreadItEqually = !spreadItEqually;
            }
        }
    }
}
