using UnityEngine.Audio;
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
            /*AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = s.GetClip();
            source.volume = s.GetVolume();
            source.pitch = s.GetPitch();*/

            s.SetSource(gameObject.AddComponent<AudioSource>());
            AudioSource source = s.GetSource();
            source.clip = s.GetClip();

            source.volume = s.GetVolume();
            source.pitch = s.GetPitch();

            source.loop = s.loop;
        }
    }
    void Start()
    {
        PlayClip("MainTheme");
    }

    public void PlayClip(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.GetName() == name);
        if (s == null)
        {
            Debug.LogWarning("The sound: " + name + " cannot be found!");
        }
        else s.GetSource().Play();
    }

    public void StopClip(string name) //stop playing a clip
    {
        Array.Find(sounds, sound => sound.GetName() == name).GetSource().Stop();
    }
    public void StopAllClips() //stop playing all music (destroy itself)
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        Destroy(audioManager.gameObject);
    }
}
