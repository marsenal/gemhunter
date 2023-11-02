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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag =="Player" && !isPickedUp)
        {
            isPickedUp = true;
            AudioManager.instance.PlayClip("GemPickup");
            myAnimator.SetTrigger("isPickedUp");
            collision.GetComponent<Player>().PickedUpGem();
        }
    }

    public void DestroyYourself()
    {
        Destroy(gameObject);
    }
}
