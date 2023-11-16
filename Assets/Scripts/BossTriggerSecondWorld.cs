using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Passing through this activates the second world boss.
/// </summary>
public class BossTriggerSecondWorld : MonoBehaviour
{
 
    [SerializeField] MossBoss boss;

    [SerializeField] bool hasCutscenePlayed = false;

    PlayableDirector playableDirector; //for timeline
    [SerializeField] PlayableAsset bossAlreadyAppearedCutscene;

    private void Awake()
    {
        int numberOfTriggers = FindObjectsOfType<BossTriggerSecondWorld>().Length;
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
        if (hasCutscenePlayed)
        {
            //playableDirector.playableAsset = bossAlreadyAppearedCutscene;
           // playableDirector.Play();
            if (boss == null)
            {
                boss = FindObjectOfType<MossBoss>(); //on re-load trigger loses the boss serialization (maybe rework the whole trigger)
            }
            if (!boss.isActive) boss.Activate();

        }
        else
        {
            collision.GetComponent<Player>().CutsceneMode(true);
            playableDirector.Play();
            hasCutscenePlayed = true;
        }
        //boss.Activate();
        //boss.cutscene = true;
        //boss.CutScene();
        //if (!AudioManager.instance.IsMusicPlaying("BossTheme")) AudioManager.instance.PlayClip("BossTheme", true);

    }
}
