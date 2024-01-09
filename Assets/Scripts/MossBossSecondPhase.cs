using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MossBossSecondPhase : MonoBehaviour
{
    [SerializeField] float speed;
    Transform startingPosition;

    Rigidbody2D myRigidbody;
    Animator myAnimator;
    SpriteRenderer mySpriteRenderer;
    void Start()
    {
        startingPosition = transform;

        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myRigidbody.gravityScale = 0f;

        FlyWithPhysics();
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(transform.position, myRigidbody.velocity));
    }

    public void FlyWithPhysics()
    {
        myRigidbody.gravityScale = 1f;
        mySpriteRenderer.enabled = true;
        myRigidbody.velocity = new Vector2(speed, Mathf.Abs(speed));
    }

    public void DestroyMe()//reset position, status
    {
       // transform.position = startingPosition.position;
       // mySpriteRenderer.enabled = false;

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            myAnimator.SetTrigger("isDestroyed");
            myRigidbody.velocity = new Vector2(0f, 0f);
            myRigidbody.gravityScale = 0f;
        }
    }
}
