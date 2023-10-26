using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Passing through this activates the boss.
/// </summary>
public class BossTrigger : MonoBehaviour
{
    [SerializeField] ContraptionBoss boss;
    [SerializeField] GameObject endPortal;
          

    private void Start()
    {
        AudioManager.instance.StopClipWithoutFade("MainTheme"); //if the level is started from level select scene
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        boss.Activate();
        if (!AudioManager.instance.IsMusicPlaying("BossTheme")) AudioManager.instance.PlayClip("BossTheme", true);
    }

}
