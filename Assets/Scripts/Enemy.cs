using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    [SerializeField] float moveSpeed;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
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
}
