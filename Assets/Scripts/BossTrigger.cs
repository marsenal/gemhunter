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

    bool portalSpawned = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        boss.Activate();
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
