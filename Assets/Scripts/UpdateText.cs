using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;
using System.Globalization;

public class UpdateText : MonoBehaviour
{
    int localeId;

    void Start()
    {
        StartCoroutine(InitializeLanguage());

    }

    IEnumerator InitializeLanguage()
    {
        yield return LocalizationSettings.InitializationOperation;
        Debug.Log(LevelSystem.setLanguageID);
        if (LevelSystem.setLanguageID == -1)
        {
            SystemLanguage language = Application.systemLanguage;
            switch (language.ToString())
            {
                case "English":
                    localeId = 0;
                    break;
                case "Hungarian":
                    localeId = 1;
                    break;
                default:
                    localeId = 0;
                    break;
            }
        }
        else
        {
            localeId = LevelSystem.setLanguageID;
        }
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];

    }

    public void ChangeLanguage(int localeid) //used on the language canvas buttons
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeid];
        LevelSystem.setLanguageID = localeid; 
        SaveSystem.SaveGame();  //save locally
        FindObjectOfType<Authentication>().OpenSavedGame(true); //save to cloud
    }

}
