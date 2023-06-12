using DigitalRuby.RainMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainController : MonoBehaviour
{
    private RainScript2D rainController => GetComponent<RainScript2D>();

    [Range(0.0f, 1.0f)]
    [SerializeField] private float intensity;
    [SerializeField] private float targetIntensity;

    [SerializeField] private float changeRate = .05f;
    [SerializeField] private float minValue = .2f;
    [SerializeField] private float maxValue = .49f;

    [SerializeField] private float chanceToRain = 40;
    [SerializeField] private float rainCheckCooldown;
    private float rainCheckTimer;

    private bool canChangeIntensity;

    private void Update()
    {
        rainCheckTimer -= Time.deltaTime;
        rainController.RainIntensity = intensity;

        if (Input.GetKeyDown(KeyCode.R))
        {
            canChangeIntensity = true;
        }

        CheckForRain();

        if (canChangeIntensity)
            ChangeIntensity();
    }

    private void CheckForRain()
    {
        if (rainCheckTimer < 0)
        {
            rainCheckTimer = rainCheckCooldown;
            canChangeIntensity = true;


            if (Random.Range(0, 100) < chanceToRain)
                targetIntensity = Random.Range(minValue, maxValue);
            else
                targetIntensity = 0;
        }
    }

    private void ChangeIntensity()
    {
        if (intensity < targetIntensity)
        {
            intensity += changeRate * Time.deltaTime;

            if (intensity >= targetIntensity)
            {
                intensity = targetIntensity;
                canChangeIntensity = false;
            }
        }

        if (intensity > targetIntensity)
        {
            intensity -= changeRate * Time.deltaTime;

            if (intensity <= targetIntensity)
            {
                intensity = targetIntensity;
                canChangeIntensity = false;
            }
        }
    }
}
