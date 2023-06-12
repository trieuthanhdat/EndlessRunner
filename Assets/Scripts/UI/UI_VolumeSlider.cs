using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string audioParametr;
    [SerializeField] private float multiplier = 25;

    public void SetupSlider()
    {
        slider.onValueChanged.AddListener(SliderValue);
        slider.minValue = .001f;
        slider.value = PlayerPrefs.GetFloat(audioParametr, slider.value);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(audioParametr, slider.value);
    }

    private void SliderValue(float value)
    {
        audioMixer.SetFloat(audioParametr, Mathf.Log10(value) * multiplier);
    }
}
