using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;
using System.Globalization;

public class UpdateText : MonoBehaviour
{
    int localeId;

    IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;
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
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];

    }

    public void UpdateTextField(string stringKey)
    {
        
    }
}
