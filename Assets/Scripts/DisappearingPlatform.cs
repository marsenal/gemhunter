using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    Animator myAnimator;
    BoxCollider2D myBodyCollider;
    [SerializeField] LayerMask playerLayer;

    [SerializeField] float respawnTime;
    [SerializeField] float disappearTime;
    float timer1;
    float timer2;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<BoxCollider2D>();
        timer1 = respawnTime;
        timer2 = disappearTime;
    }
    private void Update()
    {
        myAnimator.SetBool("isCrumbling", IsPlayerOnMe());
    }

    bool IsPlayerOnMe()
    {
        float boxWidth = myBodyCollider.bounds.size.x;
        Vector2 boxSize = new Vector2(boxWidth, transform.localScale.y / 2);
        Vector2 boxOrigin = new Vector2(transform.position.x, transform.position.y + transform.localScale.y / 2);
        RaycastHit2D hit = Physics2D.BoxCast(boxOrigin, boxSize, 0f, Vector2.up, 0.1f, playerLayer);
        return hit.collider != null;

      /*  RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1f, playerLayer);
        return hit.collider != null;*/
    }
}
