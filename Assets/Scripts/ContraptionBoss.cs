using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContraptionBoss : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    Player playerToAttack;
    Animator myAnimator;
    bool isCharging;
    public bool isActive = false;
    [SerializeField] float chargeTimer;
    Vector2 chargeDestination;
    float direction;
    public float timer;
    [SerializeField] int lives;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        playerToAttack = FindObjectOfType<Player>();
        myAnimator = GetComponent<Animator>();
        timer = chargeTimer;
    }

    void Update()
    {
        if (!isActive) { return; }
        ChargePlayer();
        ChargerCountDown();
        transform.localScale = new Vector3(Mathf.Sign(myRigidbody.velocity.x), 1f, 1f);
    }

    public void Activate()
    {
        isActive = true;
    }

    private void ChargerCountDown()
    {
        timer = timer - Time.deltaTime;
        if (timer < 0f)
        {
            isCharging = true;
            //chargeDestination = new Vector2(playerToAttack.transform.position.x, transform.position.y);
            direction = playerToAttack.transform.position.x - transform.position.x;
            timer = chargeTimer;
        }
    }

    private void ChargePlayer()
    {
        if (isCharging)
        {
            //float direction = playerToAttack.transform.position.x - transform.position.x;            
            myRigidbody.velocity = new Vector2(Math.Sign(direction) * 10f, 0f);
        }
        else
        {
            myRigidbody.velocity = new Vector2(0f, 0f);
        }
        myAnimator.SetBool("isCharging", isCharging);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" ||collision.gameObject.tag == "Player")
        {
            isCharging = false;
        }
        if (collision.gameObject.tag == "Hazard")
        {
            lives--;
            if (lives<0)
            {
                Destroy(gameObject);
            }
        }
    }
}
