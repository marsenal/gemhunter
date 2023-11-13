using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [SerializeField] Door door;
    [SerializeField] BounceBlock bounceBlock;
    [SerializeField] Animator[] spikes;
    [SerializeField] Box platform;

    enum Type
    {
        Button,
        Crank
    }


    [SerializeField] Type myType;
    bool isPressed;
    Animator myAnimator;
    SpriteRenderer mySprite;
    //BoxCollider2D myCollider;
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        mySprite = GetComponent<SpriteRenderer>();
        //myCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (myType == Type.Button)
        {
            if (IsSomethingOnIt())
            {
                Activate();
            }
            else
            {
                DeActivate();
            }
        }
        else
        { return; }
    }

    private void OnTriggerEnter2D(Collider2D collision) //collision detection in case this is a crank - one time activation
    {     
            if (!isPressed) AudioManager.instance.PlayClip("CrankActivate");
            Activate();        
    }

    public void Activate() //activation in case of button - continuous detection
    {
        if (isPressed) { return; }
        AudioManager.instance.PlayClip("CrankActivate");
        isPressed = true;
        myAnimator.SetBool("isPressed", isPressed);
        if (door) door.Open();
        //myCollider.enabled = false;
        if (bounceBlock)
        { bounceBlock.Reveal(); }
        if (spikes != null)
        {
            foreach (Animator a in spikes)
            {
                a.enabled = false;
            }
        }
        if (platform) platform.Activate();
    }

    private void DeActivate()
    {
        if (isPressed)
        {
            isPressed = false;
            myAnimator.SetBool("isPressed", isPressed);
            door.Close();            
        }
    }

    bool IsSomethingOnIt()
    {
        float boxWidth = mySprite.bounds.size.x; 
        Vector2 boxSize = new Vector2(boxWidth, transform.localScale.y / 2);
        Vector2 boxOrigin = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hit = Physics2D.BoxCast(boxOrigin, boxSize, 0f, Vector2.down, 0.1f);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.5f);
        //Debug.Log(hit.collider.gameObject.name);
        return hit.collider;// != null;
    }
}
