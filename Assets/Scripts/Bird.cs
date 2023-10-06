using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    Animator myAnimator;
    bool isFlying = false;

    [SerializeField] Vector2 flyDirection;
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        //flyDirection = new Vector2 (Random.Range());
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
        float speedx = Random.Range(2f, 5f);
        float speedy = Random.Range(5f, 8f);
        transform.position += new Vector3(speedx * Time.deltaTime, speedy * Time.deltaTime, 0f);         
    }
}
