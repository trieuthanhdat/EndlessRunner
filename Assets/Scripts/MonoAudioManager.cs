using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Sound
{
    [Header("Basic Settings")]
    public string name;
    public AudioClip clip;
    public bool playOnAwake;
    public bool isBackgroundSound = false;
    [Range(0, 1)]
    public float volume = 1f;
    [Range(0f, 2f)]
    public float pitch = 1f;

    [Header("Fade Settings")]
    [Tooltip("Duration of the fade-in effect in seconds")]
    [Range(0f, 60f)]
    public float fadeInDuration = 1f;
    [Tooltip("Duration of the fade-out effect in seconds")]
    [Range(0f, 60f)]
    public float fadeOutDuration = 1f;

    [HideInInspector]
    public AudioSource audioSource;
    [HideInInspector]
    public bool useFadeInEffect = false;
    [HideInInspector]
    public bool useFadeOutEffect = false;
    [HideInInspector]
    public MonoAudioPlayer player;
}

public class MonoAudioManager : MonoSingleton<MonoAudioManager>
{
    [SerializeField] MonoAudioPlayer audioPlayerPrefabs;
    [SerializeField] Sound[] sounds;

    private Dictionary<string, Sound> soundDictionary;

    private void Awake()
    {
        soundDictionary = new Dictionary<string, Sound>();

        foreach (Sound s in sounds)
        {
            // Set up player
            s.player = Instantiate(audioPlayerPrefabs, transform);
            s.player.sound = s;
            s.player.fadeInTimer = s.fadeInDuration;
            s.player.fadeOutTimer = s.fadeOutDuration;

            // Set up sound
            s.audioSource = s.player.gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;

            s.audioSource.playOnAwake = s.playOnAwake;
            s.audioSource.pitch = s.pitch;
            if (s.isBackgroundSound)
                s.audioSource.volume = 0;
            else
                s.audioSource.volume = s.volume;

            // Add sound to the dictionary using its name as the key
            soundDictionary[s.name] = s;

            // Disable to save performance
            ToggleActivationPlayer(false, s.player);
        }
    }

    private void Start()
    {
        PlaySound("BG" + UnityEngine.Random.Range(1, 3), true, true);
    }

    public void ToggleActivationPlayer(bool isOn, MonoAudioPlayer player)
    {
        player.gameObject.SetActive(isOn);
    }

    public void PlaySound(string name, bool isLoop = false, bool isGradient = false, float delay = 0)
    {
        if (soundDictionary.TryGetValue(name, out Sound s))
        {
            s.audioSource.loop = isLoop;
            s.useFadeInEffect = isGradient;
            s.player.PlaySound(delay);
        }
        else
        {
            Debug.LogWarning("MONOAUDIOMANAGER: Sound name: " + name + " is missing!!!");
        }
    }

    public void StopSound(string name, bool isGradient = false, float modifiedFadeoutTime = -1)
    {
        if (soundDictionary.TryGetValue(name, out Sound s))
        {
            s.useFadeOutEffect = isGradient;

            if (modifiedFadeoutTime > -1)
                s.fadeOutDuration = modifiedFadeoutTime;
            s.player.StopSound();
        }
        else
        {
            Debug.LogWarning("MONOAUDIOMANAGER: Sound name: " + name + " is missing!!!");
        }
    }
}
