using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float speed;
    Animator myAnimator;
    bool canBeDestroyed;
    bool isDestroyed;

    void Start()
    {
        canBeDestroyed = false;
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if(!isDestroyed) Move();
    }

    private void Move()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision) //what to do when hits the wall
    {
        if ((collision.CompareTag("Player") || collision.CompareTag("Ground") )&& canBeDestroyed)
        {
            myAnimator.SetTrigger("hasHitWall");
            isDestroyed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //avoid hitting the wall on spawn and instatnly destroyed
    {
        if(collision.CompareTag("Ground"))
        {
            canBeDestroyed = true;
        }
    }

    public void DestroyMe() //use on the death animation keyframe to destroy object
    {
        Destroy(gameObject);
    }
}
