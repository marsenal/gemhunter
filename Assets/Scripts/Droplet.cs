using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Droplet currently run via gravity - rigidbody. Is this expensive? (Switched to this, because with transform)
/// </summary>
public class Droplet : MonoBehaviour
{
    Animator myAnimator;
    Rigidbody2D myRigidbody2D;
    [SerializeField] float dropSpeed;
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            myRigidbody2D.gravityScale = 0f;
            myRigidbody2D.velocity = new Vector2(0f, 0f);
            
            myAnimator.SetTrigger("groundHit");
        }
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

}
