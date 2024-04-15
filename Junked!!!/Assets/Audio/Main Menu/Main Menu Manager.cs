using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    static MainMenuManager instance;
    private void Awake() { if (instance == null) instance = this; }

    [SerializeField]
    Animator MainMenuContainerAnimator;

    [SerializeField]
    string gameSceneName;
    public void StartGame()
    {
        MainMenuContainerAnimator.SetBool("start", true);
    }
    public static void LoadGameScene()
    {
        SceneManager.LoadScene(instance.gameSceneName);
    }

    public void RequestMenuSwitch(string menuName)
    {
        switch (menuName.ToLower()) // could just use 'menuName' inside of '.SetBool(...)' but we may want edge cases in the future
        {
            case "main":
                MainMenuContainerAnimator.SetBool("settings", false);
                break;
            case "settings":
                MainMenuContainerAnimator.SetBool("settings", true);
                break;
        }
    }
}
