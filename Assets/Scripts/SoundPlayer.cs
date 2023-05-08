using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private bool playOnStart;
    [SerializeField] private bool isMusic;
    [SerializeField] private string clipName;

    private void Start()
    {
        if (playOnStart)
        {
            if (isMusic)
                PlayMusic(clipName);
            else
                PlaySound(clipName);
        }
    }

    public void PlaySound(string clipName)
    {
        SoundManager.PlayMisc(clipName);
    }

    [Tooltip("Only one music clip can be playing at a time")]
    public void PlayMusic(string clipName)
    {
        SoundManager.PlayMusic(clipName);
    }
}
