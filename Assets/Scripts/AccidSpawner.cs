using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccidSpawner : MonoBehaviour
{
    [SerializeField] float shootingInterval;
    static float PREPARE_TIME = 1f;
    public float timer;
    public bool hasShot = false;
    Accid waterShootup;
    Animator myAnimator;
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        waterShootup = GetComponentInChildren<Accid>();
       // shootingInterval = waterShootup.GetShootInterval();
        timer = shootingInterval;
    }

    void Update()
    {
        if (timer - PREPARE_TIME < 0f)
        {
            myAnimator.SetTrigger("isPreparing");
            hasShot = true;
            timer = shootingInterval;
        }
        else if (!hasShot)
        {
            timer -= Time.deltaTime;
        }

        //IF the shotting interval - prepare time is over -
        //THEN switch animaton to preparing
        
    }

    public void Shoot() //used in the animation keyframe
    {
        waterShootup.ShootUp();
        hasShot = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Accid>()) 
        { 
        hasShot = false;
        myAnimator.SetTrigger("hitWater");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Accid>())
        {
           // hasShot = true;
        }
    }
}
