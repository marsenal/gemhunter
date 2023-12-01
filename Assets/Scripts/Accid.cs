using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accid : MonoBehaviour
{
    Vector2 startingPosition;
    Rigidbody2D myRigidbody;
    [SerializeField] float speed;

    void Start()
    {
        startingPosition = transform.position;
        myRigidbody = GetComponent<Rigidbody2D>();
        //ShootUp();
    }

    private void Update()
    {
        if (transform.position.y < startingPosition.y)
        {
            transform.position = startingPosition;
            myRigidbody.gravityScale = 0f;
        }
        transform.localScale = new Vector2(transform.localScale.x, Mathf.Sign(myRigidbody.velocity.y));

    }

    void FixedUpdate()
    {  
    }


    public void ShootUp()
    {
        myRigidbody.gravityScale = 1f;
        myRigidbody.velocity = Vector2.up * speed;
    }
}
