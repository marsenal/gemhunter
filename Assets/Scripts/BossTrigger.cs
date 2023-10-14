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
    [SerializeField] AudioManager audioManager;

    bool portalSpawned = false;

    private void Awake()
    {
        
    }

    private void Start()
    {
        audioManager.StopClipWithoutFade("MainTheme");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        boss.Activate();
        //audioManager.PlayClip("BossIntro");
        audioManager.PlayClip("MainTheme");
    }

    private void Update()
    {
        if (FindObjectOfType<ContraptionBoss>() == null && !portalSpawned)
        {
            Instantiate(endPortal, new Vector2(13.5f, -20f), Quaternion.identity);
            portalSpawned = true;
        }
    }
}
