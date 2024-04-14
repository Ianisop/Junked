using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    static SettingsManager instance;
    private void Awake() { if (instance == null) instance = this; }


    [SerializeField]
    AudioMixer audioMixer;

    public static void SetAudioLevel(string channel, float level)
    {
        instance.audioMixer.SetFloat(channel.ToLower(), Mathf.Log10(level) * 20f);
    }
}
