using System;
using UnityEngine;

public class LastLevel : MonoBehaviour
{
    public AudioClip finalMusic;

    public static event Action<AudioClip> OnLastLevelAchieved;

    private void Start()
    {
        OnLastLevelAchieved?.Invoke(finalMusic);
    }
}
