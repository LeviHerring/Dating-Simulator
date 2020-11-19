using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [HideInInspector]
    public static List<SONG> allSongs = new List<SONG>();

     void Awake()
    {
     if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
     else
        {
            DestroyImmediate(gameObject);
        }
    }
    public void PlaySFX(AudioClip effect, float volume = 1f, float pitch = 1f)
    {
        AudioSource source = CreateNewSource(string.Format("SFX [{0}]", effect.name));
        source.clip = effect;
        source.volume = volume;
        source.Play();

        Destroy(source.gameObject, effect.length);

    }

    public SONG PlaySong(AudioClip song, float maxVolume = 1f, float pitch = 1f, float startingVolume = 0f, bool playOnStart = true, bool loop = true)
    {
        
    }

    public static AudioSource CreateNewSource(string _name)
    {
        AudioSource newSource = new GameObject(_name).AddComponent<AudioSource>();
        newSource.transform.SetParent(instance.transform);
        return newSource; 
    }

    [System.Serializable]
    public class SONG
    {
        public AudioSource source;
        public float maxVolume = 1f;

        public SONG(AudioClip clip, float maxVolume, float pitch, float startingVolume, bool playOnStart, bool loop)
        {
            source = AudioManager.CreateNewSource(string.Format("SONG [{0}]", clip.name));
            source.clip = clip;
            source.volume = startingVolume;
            maxVolume = _maxVolume;
            source.pitch = pitch;
            source.loop = loop;


            AudioManager.allSongs.Add(this);

            if (playOnStart)
                source.Play();

        }
    }
}
