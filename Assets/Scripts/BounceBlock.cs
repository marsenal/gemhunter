using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBlock : MonoBehaviour
{
    Animator myAnimator;

    [SerializeField] float bounceForce;
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag =="Player")
        {
            
        }
    }

    public void Bounce() //call this in Player when colliding from below to bounce the block
    {
        myAnimator.SetTrigger("isBouncing");
    }

    public float GetForce() //call this in Player when colliding to get the force to apply on player
    {
        return bounceForce;
    }
}
