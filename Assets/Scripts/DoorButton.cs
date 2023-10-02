using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [SerializeField] Door door;

    enum Type
    {
        Button,
        Crank
    }

    [SerializeField] Type myType;
    bool isPressed;
    Animator myAnimator;
    BoxCollider2D myCollider;
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
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
        if (myType == Type.Button) return;
        else
        {
            Activate();
            Debug.Log(collision.gameObject.name);
        }
    }

    public void Activate() //activation in case of button - continuous detection
    {
        if (isPressed) { return; }
        isPressed = true;
        myAnimator.SetBool("isPressed", isPressed);
        door.Open();
        //myCollider.enabled = false;
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1f);
        //Debug.Log(hit.collider.gameObject.name);
        return hit.collider != null;
    }
}
