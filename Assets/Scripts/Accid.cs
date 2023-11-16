using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accid : MonoBehaviour
{
    Vector2 startingPosition;
    Rigidbody2D myRigidbody;
    [SerializeField] float speed;

    float timer;
    void Start()
    {
        startingPosition = transform.position;
        timer = 0f;
        myRigidbody = GetComponent<Rigidbody2D>();
        //ShootUp();
    }

    private void Update()
    {
        if (transform.position.y < startingPosition.y)
        {
            transform.position = startingPosition;
        }
        transform.localScale = new Vector2(transform.localScale.x, Mathf.Sign(myRigidbody.velocity.y));

    }

    void FixedUpdate()
    {  
    }


    public void ShootUp()
    {
        myRigidbody.velocity = Vector2.up * speed;
    }
}
