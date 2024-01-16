using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPortal : MonoBehaviour
{
    [SerializeField] int sceneToLoad;
    [SerializeField] bool shouldAudiomanagerReset;

    public int GetSceneIndex()
    {
        if (shouldAudiomanagerReset) AudioManager.instance.StopAllClips();
        return sceneToLoad;
    }
}
