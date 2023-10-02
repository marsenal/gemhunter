using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    Animator myAnimator;
    bool isFlying = false;
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isFlying)
        {
            FlyAway();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isFlying = true;
        myAnimator.SetTrigger("isFlying");
    }

    private void FlyAway()
    {
        transform.position += new Vector3(2f * Time.deltaTime, 5f * Time.deltaTime, 0f);         
    }
}
