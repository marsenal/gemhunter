using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContraptionBoss : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    Player playerToAttack;
    Animator myAnimator;
    public bool isCharging;
    public bool isActive = false;
    [SerializeField] float chargeTimer;
    Vector2 chargeDestination;
    public float direction;
    public float timer;
    [SerializeField] int lives;
    [SerializeField] [Tooltip("The extra distance boss covers with charge.")] float overChargeDistance;
    [SerializeField] GameObject dustTrailRight;
    [SerializeField] GameObject dustTrailLeft;
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
        PlayParticle();
        /*if (Mathf.Abs(myRigidbody.velocity.x) > 0f)
        {
            transform.localScale = new Vector3(Mathf.Sign(myRigidbody.velocity.x), 1f, 1f);
        }*/
    }

    public void Activate()
    {
        isActive = true;
    }

    private void ChargerCountDown()
    {
        if (timer < 0f)
        {
            isCharging = true;
            float sign = Mathf.Sign(transform.position.x - playerToAttack.transform.position.x);
            chargeDestination = new Vector2(playerToAttack.transform.position.x - sign * overChargeDistance, transform.position.y);

            Debug.Log("Charge destination modified to: " + chargeDestination.x);
            direction = playerToAttack.transform.position.x - transform.position.x;
            timer = chargeTimer;
        }
        else if (!isCharging)
        {
            timer = timer - Time.deltaTime;
        }
    }

    private void ChargePlayer()
    {
        if (isCharging)
        {
            myRigidbody.velocity = new Vector2(Math.Sign(direction) * 7f, 0f);
            //transform.position = Vector3.MoveTowards(transform.position, finalDestination, Time.deltaTime*8f);
            float difference = transform.position.x - chargeDestination.x;
            Debug.Log("Actual position: " + transform.position.x + " minus the final position: " + chargeDestination.x + " equals: " + difference);
            if (Mathf.Abs(difference) < 1f)
            {
                isCharging = false;
            }
        }
        else
        {
            myRigidbody.velocity = new Vector2(0f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" ||collision.gameObject.tag == "Player")
        {
            isCharging = false;
        }
        if (collision.gameObject.tag == "Hazard")
        {
            Debug.Log("Damaged");
            lives--;
            myAnimator.SetTrigger("isDamaged");
            if (lives<0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void PlayParticle()
    {
        if (!isCharging)
        {
            dustTrailRight.SetActive(false);
            dustTrailLeft.SetActive(false);
        }
        else if (myRigidbody.velocity.x < 0)
        {
            dustTrailLeft.SetActive(false);
            dustTrailRight.SetActive(true);
        }
        else if (myRigidbody.velocity.x > 0)
        {
            dustTrailLeft.SetActive(true);
            dustTrailRight.SetActive(false);
        }
    }
}
