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
        //int currentFps = Screen.currentResolution.refreshRate; //get the refreshRate (Hz) of the screen and use that as target FPS
        int currentFps = 60;
        Application.targetFrameRate = currentFps;
        if (quitCanvas != null) quitCanvas.enabled = false;
        if (settingsCanvas != null) settingsCanvas.enabled = false;
    }


    public void FadeOut()
    {
      /*  if (escapeButton)
        {
            escapeButton.enabled = false;
        }*/
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



    public void EnableEscapeButton() //enable escape button after fade in - these are not used any more - instead masking is used on the button
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
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneToLoad);
    }

    
    public void BackButton() //used on the back button events
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
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }
    public void NoToQuit()
    {
        
    }

    public void Settings() //used on the settings events
    {
        settingsCanvas.enabled = true;
        ButtonSound();
    }

    public void ButtonSound()
    {
        AudioManager.instance.PlayClip("Menu");
    }

    public void FadeOutThenFadeIn() //for skipping cutscenes for example
    {
        myAnimator.SetTrigger("FadeOutThenFadeIn");
        //FindObjectOfType<AudioManager>().PlayClip("FadeOut"); - disabled until a better loading sound
    }
}
