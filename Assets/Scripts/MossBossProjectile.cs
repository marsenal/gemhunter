using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MossBossProjectile : MonoBehaviour
{
    Vector2 destination;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    [SerializeField] float speed;
    bool isFlying;

    void Start()
    {
        // destination = FindObjectOfType<Player>().transform.position;
        if (FindObjectOfType<Player>()) destination = new Vector2(FindObjectOfType<Player>().transform.position.x - transform.position.x, FindObjectOfType<Player>().transform.position.y - transform.position.y);
        Debug.Log("The X coordinate: " + destination.x + "\n and the y coodrinate: " + destination.y);
        
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        //MoveTowardsPlayer();
    }

    private void Update()
    {
        /*  if (transform.position.y < startingPosition.y)
          {
              transform.position = startingPosition;
          }
          transform.localScale = new Vector2(transform.localScale.x, Mathf.Sign(myRigidbody.velocity.y));*/
       
    }

    void FixedUpdate()
    {
        if (isFlying) MoveTowardsPlayer();
    }


    public void MoveTowardsPlayer()
    {
       // myRigidbody.velocity = Vector2.up * speed;

        //transform.position = Vector2.MoveTowards(transform.position, destination, speed);
        if (FindObjectOfType<Player>()) myRigidbody.velocity = Vector2.MoveTowards(myRigidbody.velocity, destination, speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            myAnimator.SetTrigger("isDestroyed");
            isFlying = false;
            myRigidbody.velocity = new Vector2(0f, 0f);
        }
    }
    public void IsFlyingTrue() //For the animation keyframe
    {
        isFlying = true;
    }
    public void DestroyMe()//For the animation keyframe
    {
        Destroy(gameObject);
    }
}
