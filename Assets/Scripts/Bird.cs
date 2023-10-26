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
        flyDirection = new Vector2(Random.Range(-5f, 5f), Random.Range(5f, 8f));
        transform.localScale = new Vector2(Mathf.Sign(flyDirection.x), transform.localScale.y);
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
        //float speedx = Random.Range(-5f, 5f);
        //float speedy = Random.Range(5f, 8f);
        //transform.position += new Vector3(speedx * Time.deltaTime, speedy * Time.deltaTime, 0f);
        transform.position += (Vector3)flyDirection * Time.deltaTime;
    }
}
