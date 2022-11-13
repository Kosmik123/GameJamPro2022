using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        LastLevel.OnLastLevelAchieved += PlayFinalMusic;
    }

    private void PlayFinalMusic(AudioClip music)
    {
        if (TryGetComponent<AudioSource>(out var audioSource))
        {
            audioSource.Stop();
            audioSource.clip = music;
            audioSource.Play();
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }






}
