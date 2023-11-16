using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpDistance;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    Vector2 jumpVector;
    bool isCharging;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float timer;
    bool isAlive = true;

    enum State
    {
        Idle,
        Jumping,
        Falling
    }

    State myState;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        IsPlayerNear();
        if (IsWallHit())
        {
            isCharging = false;
        }
        EnumMachine();
        StateMachine();
    }
    private void StateMachine()
    {
        if (!isAlive) return;
        if (isCharging)
        {
            myState = State.Jumping;
            if (timer < 0f)
            {
                Jump();
                timer = 0.7f;
            }
            else
            {
                timer -= Time.deltaTime;

            }
            if (myRigidbody.velocity.y < 0f)
            {
                myState = State.Falling;
            }
            if (IsGrounded())
            {
                myState = State.Idle;
            }
        }
        else
        {
            myState = State.Idle;
        }
    }

    private void EnumMachine()
    {
        switch (myState)
        {
            case State.Idle:
                myAnimator.SetBool("isIdle", true);
                myAnimator.SetBool("isJumping", false);
                myAnimator.SetBool("isFalling", false);
                break;
            case State.Jumping:
                myAnimator.SetBool("isJumping", true);
                myAnimator.SetBool("isIdle", false);
                myAnimator.SetBool("isFalling", false);
                break;
            case State.Falling:
                myAnimator.SetBool("isFalling", true);
                myAnimator.SetBool("isJumping", false);
                myAnimator.SetBool("isIdle", false);
                break;
        }
    }

    private void Jump()
    {
        jumpVector = new Vector2(jumpDistance * -transform.localScale.x, jumpHeight);
        myRigidbody.velocity = jumpVector; //* Time.deltaTime;
        Debug.Log("Jumping");
    }
    bool IsPlayerNear()
    {
        Vector2 boxSize = new Vector2(12f, transform.localScale.y);
        Vector2 boxOrigin = transform.position;
        RaycastHit2D hit = Physics2D.BoxCast(boxOrigin, boxSize, 0f, Vector2.left, 0.1f, playerLayer);

        if (hit != false) 
        {
            transform.localScale = new Vector2(Mathf.Sign(transform.position.x - hit.transform.position.x), transform.localScale.y);
            isCharging = true;
        }

        return hit.collider;
    }

    private bool IsWallHit()
    {
        Vector2 start = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = Vector2.left * transform.localScale;
        RaycastHit2D hit = Physics2D.Raycast(start, direction, 1f, groundLayer);
        return hit.collider;
    }
    bool IsGrounded()
    {
        Vector2 start = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = Vector2.down;
        RaycastHit2D hit = Physics2D.Raycast(start, direction, 1f, groundLayer);
        return hit.collider;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag  == "Hazard")
        {
            isAlive = false;
            myRigidbody.velocity = new Vector2(0f, 0f);
            myRigidbody.gravityScale = 0f;
            myAnimator.SetTrigger("isDead");
        }
    }

    public void DestroyGO()
    {
        Destroy(gameObject);
    }
}
