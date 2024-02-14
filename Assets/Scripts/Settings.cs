using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] SettingsData settingsData;
    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle soundToggle;

    [SerializeField] Image musicToggleIndicator;
    [SerializeField] Image soundToggleIndicator;

    string settingsJson;

    void Start()
    {
        if (GetComponent<Canvas>())
        {
            settingsJson = PlayerPrefs.GetString("Settings"); //get a json string from playerpref
            if (settingsJson != null)
            {
                JsonUtility.FromJsonOverwrite(settingsJson, settingsData); //if it is not empty, read it into the settingsdata
            }

            musicToggle.isOn = settingsData.isMusicEnabled;
            soundToggle.isOn = settingsData.isSoundEnabled;

            musicToggleIndicator.enabled = !musicToggle.isOn;
            soundToggleIndicator.enabled = !soundToggle.isOn;

            EnableMusic(); //I believe this is needed here to set the volume again to the default value after audiomanager is destroyed
        }
        else
        {
            AudioManager.instance.SetMusic(settingsData.isMusicEnabled, settingsData.musicVolume);
            AudioManager.instance.SetSound(settingsData.isSoundEnabled);
        }
        Input.backButtonLeavesApp = true;
    }

    public void EraseDataButton() //erase all data - level and gem progress - currently not used
    {
        LevelSystem.EraseData();
        SaveSystem.SaveGame(); //save locally
        FindObjectOfType<Authentication>().OpenSavedGame(true);   //save to cloud
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

    }



    public void PlayMenuSFX()
    {
        AudioManager.instance.PlayClip("Menu");
    }


    public void EnableMusic() //used on the Music Toggle
    {
        AudioManager.instance.SetMusic(musicToggle.isOn, settingsData.musicVolume);
        musicToggleIndicator.enabled = !musicToggle.isOn;
    }

    public void EnableSound() //used on the Sound Toggle
    {
        AudioManager.instance.SetSound(soundToggle.isOn);
        soundToggleIndicator.enabled = !soundToggle.isOn;
    }

}
