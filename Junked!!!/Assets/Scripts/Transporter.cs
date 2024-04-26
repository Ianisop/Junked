using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transporter : MonoBehaviour
{
    public Timer timer;
    private void Awake()
    {
        
        timer.duration = 5;
    }
    private void Start()
    {
        GameManager.Instance._timerHandler.AddTimer(timer, false);
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            timer.RestartTimer();

        }
    }
    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.PopUp(Mathf.Round(timer.currentTime).ToString());
            if (timer.isDone)
            {
                SceneManager.LoadScene("Junkyard");
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            timer.StopTimer();
            GameManager.Instance.PopUp(" ");
        }
    }
}
