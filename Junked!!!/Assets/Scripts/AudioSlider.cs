using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSlider : MonoBehaviour
{
    [SerializeField]
    string channel;
    public void UpdateValue(float value)
    {
        SettingsManager.SetAudioLevel(channel, value);
    }
}
