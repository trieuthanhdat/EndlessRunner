using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Time = UnityEngine.Time;

public class MonoAudioPlayer : MonoBehaviour
{
    public Sound sound;
    public float fadeInTimer = 2f;
    public float fadeOutTimer = 2f;

    float timeGradient = 0;
    bool startPlaying = false;
    bool stopPlaying = false;
    float originVolume = 1;

    private void Start()
    {
        originVolume = sound.volume;

        if (sound.playOnAwake)
            PlaySound();
    }

    private void Update()
    {
        if (startPlaying && !stopPlaying)
        {
            if (sound.useFadeInEffect)
            {
                PlayFadeInOutEffect(true);
            }
            else
            {
                sound.audioSource.Play();
                startPlaying = false;
            }
        }
        else if (stopPlaying && !startPlaying)
        {
            if (sound.useFadeOutEffect)
            {
                PlayFadeInOutEffect(false);
            }
            else
            {
                sound.audioSource.Stop();
                stopPlaying = false;

            }
        }
        else if (!stopPlaying && !startPlaying)
        {
            if (!sound.audioSource.isPlaying)
                MonoAudioManager.instance.ToggleActivationPlayer(false, this);
        }
    }

    public void PlaySound(float delay = 0)
    {
        ResetCounter();

        startPlaying = true;
        stopPlaying = false;
        MonoAudioManager.instance.ToggleActivationPlayer(true, this);

        if (delay > 0)
            sound.audioSource.PlayDelayed(delay);
        else
            sound.audioSource.Play();
    }
    public void StopSound()
    {
        if (!sound.audioSource.isPlaying) return;
        ResetCounter();
        stopPlaying = true;
        startPlaying = false;
    }
    public void ResetCounter()
    {
        timeGradient = 0;
    }

    private void PlayFadeInOutEffect(bool isStart)
    {
        sound.audioSource.volume = isStart == true ? 0 : sound.audioSource.volume;

        timeGradient += Time.deltaTime;

        if (isStart)
        {
            sound.audioSource.volume += timeGradient / fadeInTimer;
            if (sound.audioSource.volume >= originVolume)
            {
                startPlaying = false;
            }
        }
        else
        {
            sound.audioSource.volume -= timeGradient / fadeOutTimer;
            if (sound.audioSource.volume <= 0)
            {
                stopPlaying = false;
                sound.audioSource.Stop();
            }
        }


    }
}
