using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class ConsoleSystem : MonoBehaviour
{
    [SerializeField] Transform m_player;
    [SerializeField] ConsoleInputHandler m_inputHandler;
    [SerializeField] Animator m_consoleAnimator;
    [SerializeField] Animator m_consoleSellAnimator;
    [SerializeField] int m_sellPagesCount;
    [SerializeField] int m_currentSellPageIdx = 0;
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

    public void DeltaSellPage(int delta)
    {
        print("dlta " + delta);

        if (m_currentSellPageIdx + delta < 0)
        {
            print("idx+dlta " + m_currentSellPageIdx + delta);
            m_currentSellPageIdx = m_sellPagesCount + m_currentSellPageIdx + delta;
        }
        else
            m_currentSellPageIdx += delta;

        m_currentSellPageIdx = m_currentSellPageIdx % (m_sellPagesCount);

        m_consoleSellAnimator.SetInteger("PageIndex", m_currentSellPageIdx);
        print(m_currentSellPageIdx);
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
