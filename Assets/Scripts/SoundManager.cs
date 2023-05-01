using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;

        public bool stack;
        public bool loop;

        [HideInInspector] public AudioSource source;
    }

    public static SoundManager instance;

    [SerializeField] private Sound[] sounds;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds)
            SetupSound(sound);
    }

    private void SetupSound(Sound sound)
    {
        sound.source = gameObject.AddComponent<AudioSource>();
        sound.source.clip = sound.clip;
        sound.source.volume = sound.volume;
        sound.source.pitch = sound.pitch;
        sound.source.loop = sound.loop;
    }

    public static void PlayMisc(string name)
    {
        foreach (Sound s in instance.sounds)
        {
            if (s.name == name)
            {
                if (s.stack)
                    s.source.PlayOneShot(s.clip);
                else
                    s.source.Play();

                return;
            }
        }
        Debug.LogWarning("Sound " + name + " not found");
    }

    public static void StopMisc(string name)
    {
        foreach (Sound s in instance.sounds)
        {
            if (s.name == name)
            {
                s.source.Stop();
                return;
            }
        }
        Debug.LogWarning("Sound " + name + " not found");
    }
}
