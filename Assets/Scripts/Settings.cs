using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] SettingsData settingsData;
    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle soundToggle;

    string settingsJson;

    void Start()
    {
        settingsJson = PlayerPrefs.GetString("Settings"); //get a json string from playerpref
        if (settingsJson != null)
        {
            JsonUtility.FromJsonOverwrite(settingsJson, settingsData); //if it is not empty, read it into the settingsdata
        }

        musicToggle.isOn = settingsData.isMusicEnabled;
        soundToggle.isOn = settingsData.isSoundEnabled;


        EnableMusic(); //I believe this is needed here to set the volume again to the default value after audiomanager is destroyed

        Input.backButtonLeavesApp = true;
    }

    public void EraseDataButton() //erase all data - level and gem progress
    {
        LevelSystem.EraseData();
        SaveSystem.SaveGame();
        //FindObjectOfType<Authentication>().SaveProgressToCloud();
        LevelButton[] levelButtons = FindObjectsOfType<LevelButton>();
        foreach (LevelButton lvlbtn in levelButtons)
        {
            lvlbtn.RefreshData();
        }
    }



    public void OkSave() //set the toggle status into the settingsdata, then parse it into a json string and save the string into the playerpref as Settings
    {
        PlayMenuSFX();
        settingsData.isMusicEnabled = musicToggle.isOn;
        settingsData.isSoundEnabled = soundToggle.isOn;

        settingsJson = JsonUtility.ToJson(settingsData);
        PlayerPrefs.SetString("Settings", settingsJson);
        PlayerPrefs.Save();
        Debug.Log(settingsJson);

    }



    public void PlayMenuSFX()
    {
        AudioManager.instance.PlayClip("Menu");
    }

    public void PlaySliderSFX() //Play the Slider SFX sound and set the music volume to that value
    {
        //if (musicSlider.value != storedMusicValue) AudioManager.instance.PlayClip("SetValue");
        //AudioManager.instance.SetMusicVolume(musicSlider.value);
    }

    public void EnableMusic() //used on the Music Toggle
    {
        AudioManager.instance.SetMusic(musicToggle.isOn, settingsData.musicVolume);
    }

    public void EnableSound() //used on the Sound Toggle
    {
        AudioManager.instance.SetSound(soundToggle.isOn);
    }
}
