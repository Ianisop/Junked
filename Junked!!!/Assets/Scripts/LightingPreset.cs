using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LightingPreset : ScriptableObject
{
    public Gradient AmbientColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;
}
