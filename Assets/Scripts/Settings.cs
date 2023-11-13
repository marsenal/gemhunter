using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle soundToggle;
    float storedMusicValue;
    public bool isMusicOn;
    public bool isSoundOn;

    void Start()
    {
        storedMusicValue = AudioManager.instance.GetMusicVolume("MainTheme");
        musicSlider.value = storedMusicValue;

        isMusicOn = musicToggle.isOn;
        isSoundOn = soundToggle.isOn;
    }

    public void EraseDataButton() //erase all data - level and gem progress
    {
        LevelSystem.EraseData();
        SaveSystem.SaveGame();
        LevelButton[] levelButtons = FindObjectsOfType<LevelButton>();
        foreach (LevelButton lvlbtn in levelButtons)
        {
            lvlbtn.RefreshData();
        }
    }


    public void Cancel()
    {
       // musicSlider.value = storedMusicValue;
        musicToggle.isOn = isMusicOn;
        soundToggle.isOn = isSoundOn;
        PlayMenuSFX();
       // GetComponent<Canvas>().enabled = false;
    }

    public void OkSave()
    {
        //storedMusicValue = musicSlider.value;
        PlayMenuSFX();
    }

    public void PlayMenuSFX()
    {
        AudioManager.instance.PlayClip("Menu");
    }

    public void PlaySliderSFX() //Play the Slider SFX sound and set the music volume to that value
    {
        if (musicSlider.value != storedMusicValue) AudioManager.instance.PlayClip("SetValue");
        AudioManager.instance.SetMusicVolume(musicSlider.value);
    }

    public void EnableMusic() //used on the Music Toggle
    {
        AudioManager.instance.SetMusic(musicToggle.isOn, storedMusicValue);
    }

    public void EnableSound() //used on the Sound Toggle
    {
        AudioManager.instance.SetSound(soundToggle.isOn);
    }
}
