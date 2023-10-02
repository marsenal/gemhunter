using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    Animator myAnimator;
    bool isPickedUp = false; //this helps so the gem can only be picked up once
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag =="Player" && !isPickedUp)
        {
            isPickedUp = true;
            myAnimator.SetTrigger("isPickedUp");
            collision.GetComponent<Player>().PickedUpGem();
            Debug.Log("Gem is picked up");
        }
    }

    public void DestroyYourself()
    {
        Destroy(gameObject);
    }
}
