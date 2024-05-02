using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    static SettingsManager instance;
    SettingsMenu menu;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        menu = FindFirstObjectByType<SettingsMenu>();
    }
    private void Start()
    {
        if (menu != null)
        {
            foreach (AudioSlider slider in menu.audioSliders)
            {
                if (audioLevels.ContainsKey(slider.channel))
                {
                    slider.GetComponent<Slider>().value = audioLevels[slider.channel];
                }
            }
        }
    }

    [SerializeField]
    AudioMixer audioMixer;
    Dictionary<string, float> audioLevels = new Dictionary<string, float>();
    public static void SetAudioLevel(string channel, float level)
    {
        if (!instance.audioLevels.ContainsKey(channel))
        {
            instance.audioLevels.Add(channel, level);
        }
        else
            instance.audioLevels[channel] = level;

    }

    public static void applySettings()
    {
        foreach (KeyValuePair<string,float> e in instance.audioLevels)
        {
            instance.audioMixer.SetFloat(e.Key, Mathf.Log10(e.Value) * 20f);
        }
    }
}
