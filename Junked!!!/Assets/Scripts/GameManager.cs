using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
public class GameManager : MonoBehaviour
{
    public int score;
    public static GameManager Instance;
    public DragRigidbody dragger;
    public TMP_Text scoreText;
    public TimerHandler _timerHandler;
    public TMP_Text notificationText;
    public CapsuleCollider playerColl;
    public GameObject trashBag;


    private void Awake()
    {
        if(Instance == null) Instance = this;
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        if (trashBag == null) trashBag = GameObject.FindGameObjectWithTag("trashBag");
    }

    public void PopUp(string text)
    {
         notificationText.text = text;  
   
    }

}
