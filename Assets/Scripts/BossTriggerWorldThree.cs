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

    [SerializeField] GameObject smallBoss;

    PlayableDirector playableDirector; //for timeline
    [SerializeField] PlayableAsset bossAlreadyAppearedCutscene;

    [SerializeField] Canvas skipButtonCanvas;

    [SerializeField] SceneChanger sceneChanger;

    private void Awake()
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

    private void Update()
    {
        if (hasCutscenePlayed)
        {
            
        }
    }

    public bool HasCutscenePlayed()
    {
        return hasCutscenePlayed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasCutscenePlayed)
        {
            //playableDirector.playableAsset = bossAlreadyAppearedCutscene;
            // playableDirector.Play();
            if (boss == null)
            {
                boss = FindObjectOfType<WorldThreeBoss>(); //on re-load trigger loses the boss serialization (maybe rework the whole trigger)
            }
            if (!boss.isActive) boss.Activate();

        }
        else
        {
            if (skipButtonCanvas != null) skipButtonCanvas.enabled = true;
            collision.GetComponent<Player>().CutsceneMode(true);
            playableDirector.Play();
            hasCutscenePlayed = true;
        }
        //boss.Activate();
        //boss.cutscene = true;
        //boss.CutScene();
        //if (!AudioManager.instance.IsMusicPlaying("BossTheme")) AudioManager.instance.PlayClip("BossTheme", true);

    }

    public void Skip() //used on the Skip button in the cutscene of world 3 boss start
    {
        
        //playableDirector.Stop();
        Destroy(playableDirector);
        skipButtonCanvas.enabled = false;
        FindObjectOfType<Player>().CutsceneMode(false);
        boss.Activate();
        sceneChanger.FadeOutThenFadeIn();
    }
}
