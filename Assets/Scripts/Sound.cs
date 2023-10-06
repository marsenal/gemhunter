using UnityEngine;

[System.Serializable]
public class Sound
{
    [SerializeField] private string name;
    [SerializeField] private AudioClip clip;
    [SerializeField] [Range(0f, 1f)] private float volume;
    [SerializeField] [Range(0f, 1f)] private float pitch;
    public AudioSource source;
    public bool loop;
    public AudioClip GetClip()
    {
        return clip;
    }

    public float GetVolume()
    {
        return volume;
    }

    public float GetPitch()
    {
        return pitch;
    }

    public string GetName()
    {
        return name;
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
