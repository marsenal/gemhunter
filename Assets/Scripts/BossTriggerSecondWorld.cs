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

    [SerializeField] Canvas skipButtonCanvas;
    [SerializeField] GameObject speechBubble;

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

    private void Update()
    {

        if (hasCutscenePlayed)
        { 
            FindObjectOfType<ContraptionBoss>().transform.position = new Vector2(100f, 100f);
            return;
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
            if (boss == null)
            {
                boss = FindObjectOfType<MossBoss>(); //on re-load trigger loses the boss serialization (maybe rework the whole trigger)
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

    }

    public void Skip() //used on the skip button on level 2-10
    {
        FindObjectOfType<SceneChanger>().FadeOutThenFadeIn();
        Destroy(speechBubble);
        Destroy(playableDirector);
        skipButtonCanvas.enabled = false;
        FindObjectOfType<Player>().CutsceneMode(false);
        boss.Activate();
    }

    public void PlaySound(string soundName)
    {
        AudioManager.instance.PlayClip(soundName);
    }
}
