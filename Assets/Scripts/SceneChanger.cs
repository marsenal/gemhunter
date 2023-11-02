using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// This manages the fades, Escape Button and Settings, Quit Game canvases. Also moving between Scenes - back button and button sounds.
/// </summary>
public class SceneChanger : MonoBehaviour
{
    Animator myAnimator;
    [SerializeField] Image escapeButton;
    [SerializeField] Canvas quitCanvas;
    [SerializeField] Canvas settingsCanvas;
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        int currentFps = Screen.currentResolution.refreshRate; //get the refreshRate (Hz) of the screen and use that as target FPS
        Application.targetFrameRate = currentFps;
        if (quitCanvas != null) quitCanvas.enabled = false;
        if (settingsCanvas != null) settingsCanvas.enabled = false;
    }


    public void FadeOut()
    {
        if (escapeButton)
        {
            escapeButton.enabled = false;
        }
        myAnimator.SetTrigger("FadeOut");
        //FindObjectOfType<AudioManager>().PlayClip("FadeOut"); - disabled until a better loading sound

    }

    public void FadeOutMusic(string name)
    {
        AudioManager.instance.StopClip(name);
    }

    public void FadeInSound() //use this on the animation keyframe
    {
        
    }

    public void CutSceneFade() //this makes a quick fade e.g when skipping cutscene
    {
        myAnimator.SetTrigger("Refresh");
    }

    public void EnableEscapeButton() //enable escape button after fade in
    {
        if (escapeButton) escapeButton.enabled = true;
    } //using these in the animation keyframe - fadout/in animation, to not be able to pause while fading
    public void DisableEscapeButton() //disable escape button while fading in
    {
        if (escapeButton) escapeButton.enabled = false;
        //FindObjectOfType<AudioManager>().PlayClip("FadeIn"); - disabled until a better loading sound
    }
    public void LoadScene(int sceneToLoad) //used on the level select buttons and when dying/completing level
    {
        FadeOut();
        StartCoroutine(LoadSceneWithDelay(sceneToLoad));
    }
    IEnumerator LoadSceneWithDelay(int sceneToLoad) //this is needed for delaying scene load by 1.5 seconds for the fadeout to finish. - maybe use async operation? - or rework/rename at least
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneToLoad);
    }

    
    public void BackButton()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene == 1)
        {
            quitCanvas.enabled = true;
        }
        else LoadScene(currentScene - 1);

        ButtonSound();
    }

    public void YesToQuit() //This is redundant but might make the code more readable
    {
        QuitGame();
    }
    public void QuitGame()
    {
        FadeOut();
        StartCoroutine(QuitWithDelay());
    }

    IEnumerator QuitWithDelay()
    {
        yield return new WaitForSeconds(1.5f);
        Application.Quit();
    }
    public void NoToQuit()
    {
        quitCanvas.enabled = false;
    }

    public void Settings()
    {
        settingsCanvas.enabled = true;
        ButtonSound();
    }

    public void ButtonSound()
    {
        AudioManager.instance.PlayClip("Menu");
    }
}
