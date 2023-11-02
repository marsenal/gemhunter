using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    BoxCollider2D myCollider;
    SpriteRenderer mySpriteRenderer;
    Animator myAnimator;
    [SerializeField] float moveSpeed;

    [SerializeField] bool movingEnemy;
    bool isDead = false;

    [SerializeField] float respawnTimer;
    float timer;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        timer = respawnTimer;
    }

    void Update()
    {
        if (movingEnemy) Move();

       /* else if (isDead)
        {
            timer-= Time.deltaTime;
            if (timer<0f)
            {
                Respawn();
            }
        }*/
    }

    private void Move()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
    }

    private void Turnaround()
    {
        moveSpeed = -moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Turnaround();
        }        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


    }

    public void Die()
    {
        myAnimator.SetTrigger("isDead");

        myCollider.enabled = false;
       // mySpriteRenderer.enabled = false;
        //isDead = true;
    }

    private void Respawn()
    {
        myCollider.enabled = true;
       // mySpriteRenderer.enabled = true;
        //isDead = false;
        //timer = respawnTimer;
    }

}
