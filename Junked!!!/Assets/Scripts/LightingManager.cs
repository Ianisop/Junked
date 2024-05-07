using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    //Variables
    [SerializeField, Range(0, 24)] private float TimeOfDay;
    [SerializeField] private Animator clockAnimator;
    [SerializeField] private float timeElapsed = 0;
    public float startTime = 9f;

    private void Awake()
    {
        TimeOfDay = 9;
    }

    private void Update()
    {
        // Checks if preset is available
        if (Preset == null)
            return;

        // When game plays time will run, otherwise time will stand still
        // but still update if variable is changed
        TimeOfDay += Time.deltaTime / 60;
        TimeOfDay %= 24; // Modulus to ensure time always stays between 0-24. Makes it like the clock

        timeElapsed += Time.deltaTime;
        clockAnimator.SetFloat("minuteHand", ((timeElapsed % 60) / 60));
        clockAnimator.SetFloat("hourHand", ((TimeOfDay - startTime) / 8f));

        if (TimeOfDay >= 17) // Restarts the day if is has passed more than "17 o'clock"
        {
            print("Day Resetting");
            TimeOfDay = startTime;
            timeElapsed = 0;
        }

        UpdateLighting(TimeOfDay / 24f);
    }

    private void UpdateLighting(float timePercent)
    {
        //Set ambient and fog based on time ingame. This is a percentage in decimals from 0-1
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        //If the directional light is set then rotate and set it's color
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

    }
}