using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This manages the escape menu which can be triggered during gameplay only.
/// </summary>
public class EscapeMenu : MonoBehaviour
{
    [SerializeField] Canvas escapeMenuCanvas;
    void Start()
    {
        escapeMenuCanvas.enabled = false;
    }

    public void OpenMenu()
    {
        escapeMenuCanvas.enabled = true;
        Time.timeScale = 0;
    }

    public void Continue()
    {
        escapeMenuCanvas.enabled = false;
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        FindObjectOfType<LevelChanger>().LoadScene(0);
        FindObjectOfType<AudioManager>().StopAllClips();
    }
}
