using UnityEngine;
/// <summary>
/// Serializeable class that contains data for a given sound, like volume, pitch. Serialize this in AudioManager.
/// </summary>
[System.Serializable]

public class Sound
{
    [SerializeField] private string name;
    [SerializeField] private AudioClip clip;
    [SerializeField] [Range(0f, 1f)] private float volume;
    [SerializeField] [Range(0f, 1f)] private float pitch;
    [SerializeField] bool playOnAwake = false;
    public AudioSource source;
    public bool loop;
    public string GetName()
    {
        return name;
    }
    public AudioClip GetClip()
    {
        return clip;
    }

    public float GetVolume()
    {
        return volume;
    }
    public void SetVolume(float volumeNew)
    {
        volume = volumeNew;
    }


    public float GetPitch()
    {
        return pitch;
    }

    public bool IsPlayingOnAwake()
    {
        return playOnAwake;
    }


    public AudioSource GetSource()
    {
        return source;
    }

    public void SetSource(AudioSource newSource)
    {
        source = newSource;
    }


}
