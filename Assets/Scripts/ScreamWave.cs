using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamWave : MonoBehaviour
{
    Vector2 destination;
    Rigidbody2D myRigidbody;
    [SerializeField] float speed;
    bool isFlying = true;

    [SerializeField] WorldThreeBoss boss;

    void Start()
    {
        if (FindObjectOfType<Player>())
        {
            Player player = FindObjectOfType<Player>();
            destination = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y); 
        }
        transform.rotation = Quaternion.Euler (0f,0f, Vector2.SignedAngle(transform.position, destination));
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isFlying) MoveTowardsPlayer();
    }

    void FixedUpdate()
    {
        //if (isFlying) MoveTowardsPlayer();
    }


    public void MoveTowardsPlayer()
    {
        if (FindObjectOfType<Player>())
        {
            myRigidbody.velocity = Vector2.MoveTowards(myRigidbody.velocity, destination, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground" || collision.tag == "Weapon")
        {
            //myAnimator.SetTrigger("isDestroyed");
            
            if (collision.tag =="Weapon")
            {
                DestroyMe();
                AudioManager.instance.PlayClip("EnemyDead");
                isFlying = false;
                myRigidbody.velocity = new Vector2(0f, 0f);
                FindObjectOfType<WorldThreeBoss>().RocksFalling();
                collision.gameObject.GetComponent<Animator>().SetTrigger("isDestroyed");
            }
        }
    }
    public void IsFlyingTrue() //For the animation keyframe
    {
        isFlying = true;
    }
    public void DestroyMe()//For the animation keyframe
    {
        Destroy(gameObject);
    }
}
