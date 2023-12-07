using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    [SerializeField] float hearingRange;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float speed;

    Animator myAnimator;

    private bool isAttacking;
        
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (CheckForPlayer() && CheckForPlayerDash())
        {
            isAttacking = true;
        }
        if (isAttacking)
        {
            Fly();
        }
    }

    bool CheckForPlayer()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - hearingRange/2);
        Vector2 boxSize = new Vector2(hearingRange/2, hearingRange/2);
        RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.down, 0.1f, playerLayer);
        return hit.collider;
    }

    bool CheckForPlayerDash()
    {
        return FindObjectOfType<Player>().isDashing;
    }

    private void Fly()
    {
        if (FindObjectOfType<Player>())
        {
            myAnimator.SetTrigger("isFlying");
            transform.position = Vector2.MoveTowards(transform.position, FindObjectOfType<Player>().transform.position, speed * Time.deltaTime);
        }
    }
}
