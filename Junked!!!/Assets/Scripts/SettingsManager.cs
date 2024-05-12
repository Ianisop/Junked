using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    static SettingsManager instance;
    public SettingsMenu menu;
    public bool reloadChanges = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        instance.reloadChanges = true;
        instance.menu = FindFirstObjectByType<SettingsMenu>();

        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (reloadChanges)
        {
            ReloadChanges();
        }
    }
    void ReloadChanges()
    {
        if (menu != null)
        {
            foreach (AudioSlider slider in menu.audioSliders)
            {
                string channel = slider.channel.ToLower();
                if (audioLevels.ContainsKey(channel))
                {
                    slider.GetComponent<Slider>().value = audioLevels[channel];
                }
            }
        }
        reloadChanges = false;
    }

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Dictionary<string, float> audioLevels = new Dictionary<string, float>();
    public static void SetAudioLevel(string channel, float level)
    {
        channel = channel.ToLower();
        if (!instance.audioLevels.ContainsKey(channel))
        {
            print("creating audiolevel entry");
            instance.audioLevels.Add(channel, level);
        }
        else
        {
            instance.audioLevels[channel] = level;
            print("setting audiolevel entry");
        }

        instance.audioMixer.SetFloat(channel, Mathf.Log10(instance.audioLevels[channel]) * 20f);
    }

    public static void applySettings()
    {
        foreach (KeyValuePair<string,float> e in instance.audioLevels)
        {
            instance.audioMixer.SetFloat(e.Key, Mathf.Log10(e.Value) * 20f);
        }
    }
}
