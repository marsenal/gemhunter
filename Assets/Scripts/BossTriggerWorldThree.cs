using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Passing through this activates the third world boss.
/// </summary>
public class BossTriggerWorldThree : MonoBehaviour
{

    [SerializeField] WorldThreeBoss boss;

    [SerializeField] bool hasCutscenePlayed = false;

    PlayableDirector playableDirector; //for timeline

    [SerializeField] Canvas skipButtonCanvas;

    [SerializeField] SceneChanger sceneChanger;

    private void Awake() //singleton pattern
    {
        int numberOfTriggers = FindObjectsOfType<BossTriggerWorldThree>().Length;
        if (numberOfTriggers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        AudioManager.instance.StopClipWithoutFade("MainTheme"); //if the level is started from level select scene
        playableDirector = GetComponent<PlayableDirector>();
    }

    public bool HasCutscenePlayed()
    {
        return hasCutscenePlayed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasCutscenePlayed) //skip the cutscene if it's already played
        {
            if (boss == null)
            {
                boss = FindObjectOfType<WorldThreeBoss>(); //on re-load trigger loses the boss serialization (maybe rework the whole trigger)
            }
            if (!boss.isActive) boss.Activate();

        }
        else //is this is the first time of trigger, show the cutscene
        {
            if (skipButtonCanvas != null) skipButtonCanvas.enabled = true;
            collision.GetComponent<Player>().CutsceneMode(true);
            playableDirector.Play();
            hasCutscenePlayed = true;
        }

    }

    public void Skip() //used on the Skip button in the cutscene of world 3 boss start
    {
        Destroy(playableDirector);
        skipButtonCanvas.enabled = false;
        FindObjectOfType<Player>().CutsceneMode(false);
        boss.Activate();
        sceneChanger.FadeOutThenFadeIn();
    }

    public void PlaySound(string soundName) //used on the timeline
    {
        AudioManager.instance.PlayClip(soundName);
    }
}
