using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Sound[] sounds;

    public static AudioManager instance;
        
    private void Awake()
    {
        if (instance == null)  //singleton pattern
        {
            instance = this;
        }
        else { Destroy(gameObject); }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds) //go over on the array of sounds and asign a new audio source to them - which is equal to a new audiosource on *this*
        {
            s.SetSource(gameObject.AddComponent<AudioSource>()); //add a new audiosource to the audiomanager and set that to be the source of the sound
            AudioSource source = s.GetSource(); //reference a new source object which will be the source of the sound
            source.clip = s.GetClip();

            source.volume = s.GetVolume();
            source.pitch = s.GetPitch();

            source.loop = s.loop;

            source.playOnAwake = s.IsPlayingOnAwake();
        }
    }
    void Start()
    {
        PlayClip("MainTheme", true);
    }

    /// <summary>
    /// Play the "name" clip, with optional parameter "looped" - by default it's false.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="looped"></param>
    public void PlayClip(string name, bool looped = false)
    {
        Sound s = Array.Find(sounds, sound => sound.GetName() == name);
        if (s == null)
        {
            Debug.LogWarning("The sound: " + name + " cannot be found!");
        }
        else
        {
            s.GetSource().loop = looped;
            s.GetSource().Play();
        }       
    }


    public void StopClip(string name) //stop a clip by fading the volume down - then destroy audiomanager
    {
        Sound s = Array.Find(sounds, sound => sound.GetName() == name);
        if (s == null)
        {
            Debug.LogWarning("The sound: " + name + " cannot be found!");
        }
        else
        {
            StartCoroutine(FadeOutMusic(name));

            //Destroy(gameObject);

        }
    }
    IEnumerator FadeOutMusic(string name)//fade out the music until volume reaches 0
    {
        Sound s = Array.Find(sounds, sound => sound.GetName() == name);
        float volume = s.GetSource().volume;

        while (s.GetSource().volume > 0f)
        {
            s.GetSource().volume -= volume * Time.deltaTime;
            if (s.GetSource().volume == 0f) //this is needed, else the GO is destroyed immediately
            {
                Destroy(gameObject);
            }
            yield return null;
        }
    }


    public void StopClipWithoutFade(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.GetName() == name);
        if (s == null)
        {
            Debug.LogWarning("The sound: " + name + " cannot be found!");
        }
        else s.GetSource().Stop();

    }

    public void StopAllClips() //stop playing all music (destroy itself at the end)
    {
        StartCoroutine(FadeOutAllMusic());
    }

    IEnumerator FadeOutAllMusic()
    {
        foreach (Sound s in sounds)
        {
            float volume = s.GetSource().volume;
            while (s.GetSource().volume > 0f)
            {
                s.GetSource().volume -= volume * Time.deltaTime;
                if (s.GetSource().volume == 0f)//this is needed, else the GO is destroyed immediately
                {
                    Destroy(gameObject);
                }
                yield return null;
            }
        }
    }
    public bool IsMusicPlaying(string name) //check wether the clip is already playing to avoid duplicate play
    {
        Sound s = Array.Find(sounds, sound => sound.GetName() == name);
        return s.GetSource().isPlaying;
    }

    ///ABOVE THESE ARE FOR SETTING MUSIC VALUES///
    ///USED IN SETTINGS.CS

    public float GetMusicVolume(string name) //returns the given music's volume in float
    {
        Sound s = Array.Find(sounds, sound => sound.GetName() == name);
        return s.GetVolume();
    }

    public void SetMusicVolume(string name, float volume) //set the given music's to the given volume
    {
        Sound s = Array.Find(sounds, sound => sound.GetName() == name);
        s.GetSource().volume = volume;
        s.SetVolume(volume); //<- this only sets the Sound's volume - which makes this a permanent change (inside, but not outside sessions)
    }
}
