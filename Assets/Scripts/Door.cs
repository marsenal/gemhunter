using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Transform openPos;
    Vector2 openPosition;
    Vector2 closedPosition;
    bool isOpen = false;
    [SerializeField] bool isClosing;
    [SerializeField] float speed;
    Animator myAnimator;
    void Start()
    {
        closedPosition = transform.position;
        openPosition = openPos.position;
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {        
            if (isOpen)
            {
                transform.position = Vector2.MoveTowards(transform.position, openPosition, speed/2 * Time.deltaTime); //open slowly
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, closedPosition, speed * Time.deltaTime);
            }
        
    }

    public void Open()
    {
        if (!isOpen)
        {
            isOpen = true;
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            isOpen = false;
        }
    }
}
