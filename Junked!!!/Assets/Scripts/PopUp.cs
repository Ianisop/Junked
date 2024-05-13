using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;   

public class PopUp : MonoBehaviour
{
    public Canvas obj;
    public TMP_Text text;
    public Animator animator;
    public static PopUp Instance;


    private void Awake()
    {
        if (Instance == null) Instance = this;
    }


    public void Setup()
    {
        this.animator = this.GetComponent<Animator>();

        this.text = this.GetComponentInChildren<TextMeshPro>();
        
        bool printPopup = true;

        if (printPopup) // Added this so it doesn't print every single time a prefab is on scene
        {
            if (animator && obj && text)
            {
                print("setup done" + gameObject.name);

            }
            else
            {
                print("Something broke: " + animator + obj + text);

            }
        }
        
    }

    public void UpdatePopUp(GameObject obj)
    {
        Trash trash = obj.GetComponentInChildren<Trash>();
        if (trash)
        {
            animator.SetBool("hover", true);
            text.text = trash.trashType.ToString() + "\n" + trash.weight + " kg\n" + trash.CO2PrKg/trash.weight + " CO2/Kg\n" + trash.moneyValue + " $";
        }
        else
        {
            TrashBag bag = obj.GetComponentInChildren<TrashBag>();
            if(bag)
            {
                animator.SetBool("hover", true);
                text.text = "Trash Bag\n" + bag.totalWeight + "kg / " + bag.maxWeight + "kg";


            }
        }
       

    }

}

