using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    [SerializeField] float hearingRange;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float speed;

    Vector2 startPosition;

    Animator myAnimator;

    private bool isAttacking;
        
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        startPosition = transform.position;
    }

    void Update()
    {
        if (CheckForPlayer() && CheckForPlayerDash())
        {
            isAttacking = true;
        }
        if (isAttacking)
        {
            if ( CheckForWall())
            {
                isAttacking = false;
            }
            Fly();
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
            transform.localScale = new Vector2(Mathf.Sign(startPosition.x - transform.position.x), transform.localScale.y);
            if (transform.position == (Vector3)startPosition)
            {
                myAnimator.SetTrigger("notFlying");
            }
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
            Player player = FindObjectOfType<Player>();
            myAnimator.SetTrigger("isFlying");
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            transform.localScale = new Vector2(Mathf.Sign(player.transform.position.x - transform.position.x),transform.localScale.y) ;
        }
    }

    bool CheckForWall()
    {
        Vector2 origin = transform.position;
        Vector2 boxSize = new Vector2(1f,1f);
        //RaycastHit2D hit = Physics2D.BoxCast(origin, boxSize, 0f, Vector2.down, 0.1f, groundLayer);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.5f, groundLayer);
        return hit.collider;
    }
}
