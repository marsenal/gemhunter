using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// This manages the fade in - fade out during level changing and disables/enables the escape button.
/// </summary>
public class LevelChanger : MonoBehaviour
{
    [SerializeField] Animator myAnimator;
    [SerializeField] Image escapeButton;
    void Start()
    {
        int currentFps = Screen.currentResolution.refreshRate; //get the refreshRate (Hz) of the screen and use that as target FPS
        Application.targetFrameRate = currentFps;
    }

    public void QuitGame()
    {
        //FadeOut(); TODO somehow implement the fadeout here with quit - coroutine? Invoke doesn't work
        Application.Quit();
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

    public void FadeInSound() //use this on the animation keyframe
    {
        
    }

    public void EnableEscapeButton()
    {
        if (escapeButton) escapeButton.enabled = true;
    } //using these in the animation keyframe - fadout/in animation, to not be able to pause while fading
    public void DisableEscapeButton()
    {
        if (escapeButton) escapeButton.enabled = false;
        //FindObjectOfType<AudioManager>().PlayClip("FadeIn"); - disabled until a better loading sound
    }
    public void LoadScene(int sceneToLoad) //used on the level select buttons and when dying/completing level
    {
        FadeOut();
        StartCoroutine(LoadSceneWithDelay(sceneToLoad));
    }
    IEnumerator LoadSceneWithDelay(int sceneToLoad) //this is needed for delaying scene load by 1 second for the fadeout to finish. - maybe use async operation? - or rework/rename at least
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneToLoad);
    }
}
