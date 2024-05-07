using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{
    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    [SerializeField] private Animator clockAnimator;
    public QuotaSystem quotaSystem;

    //Variables
    [SerializeField, Range(0, 24)] private float TimeOfDay;
    [SerializeField] private float timeElapsed = 0;
    public float startTime = 9f;
    public int day = 0;

    [SerializeField] bool bypassReset = false;

    private void Awake()
    {
        TimeOfDay = startTime;
        day = 1;
    }

    private void Update()
    {
        // Checks if preset is available
        if (Preset == null)
            return;


        TimeOfDay += Time.deltaTime / 60;
        TimeOfDay %= 24; // Modulus to ensure time always stays between 0-24. Makes it like the clock

        timeElapsed += Time.deltaTime;
        clockAnimator.SetFloat("minuteHand", ((timeElapsed % 60) / 60));
        clockAnimator.SetFloat("hourHand", ((TimeOfDay - startTime) / 8f));

        if (!bypassReset && TimeOfDay >= 17) // Restarts the day if is has passed more than "17 o'clock"
        {
            print("Day Resetting");
            TimeOfDay = startTime;
            timeElapsed = 0;
            day += 1;
            quotaSystem.UpdateQuota(day);
        }

        // Ups the day by 1 for debugging
        if (Input.GetKeyDown("9"))
        {
            day += 1;
        }

        UpdateLighting(TimeOfDay / 24f);
    }


    private void UpdateLighting(float timePercent)
    {
        // Set ambient and fog based on time ingame. This is a percentage in decimals from 0-1
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        // If the directional light is set then rotate and set it's color
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

    }
}