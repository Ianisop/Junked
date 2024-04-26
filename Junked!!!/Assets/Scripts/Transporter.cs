using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transporter : MonoBehaviour
{
    public Timer timer;
    public TMP_Text timerText;
    public bool isInJunkyard;
    
    private void Awake()
    {
        
        timer.duration = 5;
    }
    private void Start()
    {
        GameManager.Instance._timerHandler.AddTimer(timer, false);
        
    }

    private void Update()
    {

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
            timerText.text = Mathf.Round(timer.currentTime).ToString();
            if (timer.isDone)
            {
                if(isInJunkyard) SceneManager.LoadScene("Home");
                if(!isInJunkyard) SceneManager.LoadScene("Junkyard");



            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            timer.StopTimer();
    
        }
    }
}
