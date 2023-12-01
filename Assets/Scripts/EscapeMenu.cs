using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// This manages the escape menu which can be triggered during gameplay only.
/// </summary>
public class EscapeMenu : MonoBehaviour
{
    [SerializeField] Canvas escapeMenuCanvas;
    Animator canvasAnimator;
    void Start()
    {
        if (escapeMenuCanvas)
        {
            escapeMenuCanvas.enabled = false;
            canvasAnimator = escapeMenuCanvas.GetComponent<Animator>();
        }

        Input.backButtonLeavesApp = true;
    }

    public void OpenMenu()
    {
        escapeMenuCanvas.enabled = true;
        canvasAnimator.SetBool("isOpen", true); //animation for opening the menu
        Time.timeScale = 0;
    }

    public void Continue()
    {
        canvasAnimator.SetBool("isOpen", false);
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        FindObjectOfType<SceneChanger>().LoadScene(0);
        AudioManager.instance.StopAllClips();
        BossTriggerSecondWorld secondlvlbosstrigger = FindObjectOfType<BossTriggerSecondWorld>();
        BossTrigger bosstrigger = FindObjectOfType<BossTrigger>();
        if (bosstrigger) Destroy(bosstrigger.gameObject);
        if (secondlvlbosstrigger) Destroy(secondlvlbosstrigger.gameObject);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        FindObjectOfType<SceneChanger>().LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
