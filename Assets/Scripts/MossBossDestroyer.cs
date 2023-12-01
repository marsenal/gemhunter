using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MossBossDestroyer : MonoBehaviour
{
    [SerializeField] Transform endPosition;
    [SerializeField] float speed;
    [SerializeField] DoorButton crank;
    Vector2 startPosition;
    Vector2 endPositionVector;
    public bool isActive = false;
    void Start()
    {
        startPosition = transform.position;
        endPositionVector = endPosition.transform.position;
    }

    void Update()
    {
        if (isActive)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPositionVector, speed);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, startPosition, speed);
        }
        if ((Vector2)transform.position == endPositionVector)
        {
            DeActivate();
        }
        if((Vector2)transform.position == startPosition)
        {
            crank.DeActivate();
        }
    }

    public void Activate()
    {
        isActive = true;
    }

    public void DeActivate()
    {
        isActive = false;
    }
}
