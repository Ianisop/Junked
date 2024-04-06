using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public int score;
    public static GameManager Instance;
    public DragRigidbody dragger;
    public TMP_Text scoreText;

    private void Awake()
    {
        if(Instance == null) Instance = this;
    }

    void Start()
    {
        
    }

    
    void Update()
    {
            
    }

    public void GivePoint()
    {
        score += 1;
        scoreText.text = "score: " + score.ToString();  

    }
}
