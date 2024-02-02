using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
/// <summary>
/// Passing through this activates the boss.
/// </summary>
public class BossTrigger : MonoBehaviour
{
    [SerializeField] ContraptionBoss boss;
    [SerializeField] Canvas skipButtonCanvas;


    public bool hasCutscenePlayed = false;

    PlayableDirector playableDirector; //for timeline

    private void Awake()
    {
        int numberOfTriggers = FindObjectsOfType<BossTrigger>().Length;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasCutscenePlayed)
        {
            if (boss == null)
            {
                boss = FindObjectOfType<ContraptionBoss>(); //on re-load trigger loses the boss serialization (maybe rework the whole trigger)
            }
            boss.Activate();
        }
        else
        {
            collision.GetComponent<Player>().CutsceneMode(true);
            playableDirector.Play();
            hasCutscenePlayed = true;
        }
        
    }
    public void PlaySound(string soundName)
    {
        AudioManager.instance.PlayClip(soundName);
    }

    public void Skip() //used on the skip button on level 2-10
    {
        FindObjectOfType<SceneChanger>().FadeOutThenFadeIn();
        Destroy(playableDirector);
        skipButtonCanvas.enabled = false;
        FindObjectOfType<Player>().CutsceneMode(false);
        boss.Activate();
    }
}
