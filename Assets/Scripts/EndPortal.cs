using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPortal : MonoBehaviour
{
    [SerializeField] int sceneToLoad;
    [SerializeField] bool shouldAudiomanagerReset;

    [SerializeField] bool isFinalEndPortal;
    private int finalScene = 40;

    [SerializeField] string achievementToUnlock;

    public int GetSceneIndex()
    {
        if (shouldAudiomanagerReset) AudioManager.instance.StopAllClips();
        if (achievementToUnlock != null)
        {
            Social.ReportProgress(achievementToUnlock, 100.0f, (bool success) => { //Find a Gem achievement handling
                                                                                    // handle success or failure
            });
        }
        if (isFinalEndPortal && FindObjectOfType<StopWatch>().IsThisAPartialRun()) return finalScene;
        else return sceneToLoad;
    }
}
