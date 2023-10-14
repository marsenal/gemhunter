using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Sound[] sounds;

    public static AudioManager instance;

    bool isChangingVolume;
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

            s.SetSource(gameObject.AddComponent<AudioSource>()); //add a new audiosource to the audiomanager and set that to be the source of the sound
            AudioSource source = s.GetSource(); //reference a new source object which will be the source of the sound
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

    private void Update()
    {
        
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

    public void StopClip(string name) //stop a clip by fading the volume down - then destroy audiomanager
    {
        Sound s = Array.Find(sounds, sound => sound.GetName() == name);
        if (s == null)
        {
            Debug.LogWarning("The sound: " + name + " cannot be found!");
        }
        else StartCoroutine(FadeOutMusic(name));
    }

    public void StopClipWithoutFade(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.GetName() == name);
        /*if (s == null)
        {
            Debug.LogWarning("The sound: " + name + " cannot be found!");
        }
        else*/ s.GetSource().Stop();

    }
    IEnumerator FadeOutMusic(string name)//fade out the music until volume reaches 0
    {
        Debug.Log("Couroutine started");
        Sound s = Array.Find(sounds, sound => sound.GetName() == name);
        float volume = s.GetSource().volume;
        float time = 0f;
        /*volume = volume - 0.5f * Time.deltaTime;
        s.GetSource().volume = volume;*/
        /*while (time < 1.5f) {
            time += Time.deltaTime;
            Debug.Log("While loop running");
            //s.GetSource().volume = Mathf.Lerp(volume, 0f, 0.2f);
            s.GetSource().volume -= volume * Time.deltaTime;
            yield return null;
        } */
        while (s.GetSource().volume > 0f)
        {
            s.GetSource().volume -= volume * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
    public void StopAllClips() //stop playing all music (destroy itself)
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        Destroy(audioManager.gameObject);
    }

}
