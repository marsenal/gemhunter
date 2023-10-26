using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    float storedMusicValue;

    void Start()
    {
        storedMusicValue = AudioManager.instance.GetMusicVolume("MainTheme");
        musicSlider.value = storedMusicValue;
    }

    void Update()
    {
        if (GetComponent<Canvas>().enabled)
        {
            Debug.Log("Refreshing music data...");
        }
    }

    public void Cancel()
    {
        musicSlider.value = storedMusicValue;
        PlayMenuSFX();
        GetComponent<Canvas>().enabled = false;
    }

    public void OkSave()
    {
        storedMusicValue = musicSlider.value;
        PlayMenuSFX();
        GetComponent<Canvas>().enabled = false;
    }

    public void PlayMenuSFX()
    {
        AudioManager.instance.PlayClip("Menu");
    }

    public void PlaySliderSFX()
    {
        if (musicSlider.value != storedMusicValue) AudioManager.instance.PlayClip("SetValue");
        AudioManager.instance.SetMusicVolume("MainTheme", musicSlider.value);
        AudioManager.instance.SetMusicVolume("BossTheme", musicSlider.value);
    }
}
