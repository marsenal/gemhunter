using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] int loadingSeconds;

    int localeId;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(loadingSeconds);
       // FindObjectOfType<Authentication>().OpenSavedGame(false);
       /* yield return LocalizationSettings.InitializationOperation;
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
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];*/
        SceneManager.LoadScene(1);
    }
}
