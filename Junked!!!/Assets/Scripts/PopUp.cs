using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;   

public class PopUp : MonoBehaviour
{
    public Canvas canvas;
    public TMP_Text text;
    public Animator animator;



    public void Setup()
    {
        this.animator = this.GetComponent<Animator>();
        this.canvas = this.GetComponentInChildren<Canvas>();
        this.text = this.GetComponentInChildren<TextMeshProUGUI>();
        bool printPopup = false;

        if (printPopup) // Added this so it doesn't print every single time a prefab is on scene
        {
            if (animator && canvas && text)
            {
                print("setup done" + gameObject.name);

            }
            else
            {
                print("Something broke: " + animator + canvas + text);

            }
        }
        
    }

}

