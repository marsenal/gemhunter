using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    Vector2 moveInput;
    Animator myAnimator;
    BoxCollider2D myBodyCollider;

    [Header("Set these for Jumping")]
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] Transform feetPosition;

    [Header("Movement Values")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float bounceSpeed;
    [SerializeField] float maxDropSpeed;


    bool isMovingLeft = false;
    bool isMovingRight = false;

    bool isAlive = true;
    bool hasGem = false;

    int currentSceneIndex;
    public void  OnBack(InputAction.CallbackContext context)
    {
        Application.Quit();
        Debug.Log("Quitting");
    }
    enum State
    {
        Idle,
        Running,
        Jumping,
        Falling,
    }

    [SerializeField] State myState;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<BoxCollider2D>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void FixedUpdate()
    {
 
    }

    void Update()
    {
        if (!isAlive) { return; }
        Move();
        FlipSprite();
        StateMachine();
        EnumMachine();
    }

    public void OnMove(InputAction.CallbackContext context) //for keyboard control
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void MoveLeft(bool value)
    {
        isMovingLeft = value;
    }
    public void MoveRight(bool value)
    {
        isMovingRight = value;
    }

    private void Move()
    {
        if (isMovingLeft)
        {
            myRigidbody.velocity = new Vector2(-moveSpeed * Time.deltaTime, myRigidbody.velocity.y);
        }
        else if (isMovingRight)
        {
            myRigidbody.velocity = new Vector2(moveSpeed * Time.deltaTime, myRigidbody.velocity.y);
        }
        else
        {
            myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y);
        }
    }

    private void FlipSprite()
    {
        if (Mathf.Abs(myRigidbody.velocity.x) > 0f)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), transform.localScale.y);
        }
    }

    private void StateMachine()
    {
        if (Mathf.Abs(myRigidbody.velocity.x) > 0f && IsGrounded())
        {
            myState = State.Running;
        }
        else if (myRigidbody.velocity.y > 0f && !IsGrounded())
        {
            myState = State.Jumping;
        }
        else if (myRigidbody.velocity.y < 0f && !IsGrounded())
        {
            myState = State.Falling;
        }
        else
        { myState = State.Idle; }
    }

    private void EnumMachine()
    {
        switch (myState)
        {
            case State.Running:
                myAnimator.SetBool("isRunning", true);
                myAnimator.SetBool("isFalling", false);
                myAnimator.SetBool("isJumping", false);
                break;
            case State.Jumping:
                myAnimator.SetBool("isJumping", true);
                myAnimator.SetBool("isRunning", false);
                myAnimator.SetBool("isFalling", false);
                break;
            case State.Falling:
                myAnimator.SetBool("isFalling", true);
                myAnimator.SetBool("isRunning", false);
                myAnimator.SetBool("isJumping", false);
                break;
            case State.Idle:
                myAnimator.SetBool("isRunning", false);
                myAnimator.SetBool("isJumping", false);
                myAnimator.SetBool("isFalling", false);
                break;
        }
            
        
    }

    public void OnJump(InputAction.CallbackContext context) //for using keyboard
    {
        if (IsGrounded())
        {
            myRigidbody.velocity = new Vector2(moveInput.x * moveSpeed, jumpSpeed);
        }
    }

    public void Jump(bool value) //for using touch input
    {
        if (IsGrounded() && value && isAlive)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpSpeed);
        }
    }

    bool IsGrounded() //cast a box under the player which detects collision with ground layer
    {
        /*Vector2 position = feetPosition.position;
        Vector2 direction = Vector2.down;
        float distance = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayerMask);*/

        //float boxWidth = mySprite.bounds.size.x;
        float boxWidth = myBodyCollider.bounds.size.x;

        Vector2 boxSize = new Vector2(boxWidth, transform.localScale.y / 2);
        Vector2 boxOrigin = new Vector2 (transform.position.x, transform.position.y - transform.localScale.y / 2);
        RaycastHit2D hit = Physics2D.BoxCast(boxOrigin, boxSize, 0f, Vector2.down, 0.1f, groundLayerMask);
        return hit.collider != null;
    }

    private void Bounce(float force)
    { 
        myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, force*Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Hazard")
        {
            Die();
        }
        else if ((collision.tag == "Enemy" || collision.tag == "Bouncer") && collision.GetComponent<BoxCollider2D>() != null)
        {
            //float playerColPosDown = transform.position.y - myBodyCollider.bounds.size.y / 2; this didn't work - collision detection unreliability
            /*float colliderPosUp = collision.transform.position.y + collision.GetComponent<BoxCollider2D>().bounds.size.y / 2;
            if (transform.position.y - colliderPosUp > 0f )
            {
                Bounce();
                collision.GetComponent<BounceBlock>().Bounce(); //this also bounces the bounceblock (bounce animation)
            }
            else if (collision.tag == "Enemy")
            {
                Die();
            }*/ 
        }
        else if (collision.name == "EndPortal")
        {
            FreezePosition();
            transform.position = collision.transform.position; //align player with the portal

            LevelSystem.AddToLevelList(currentSceneIndex); //this checks the level in the db
            if (hasGem) LevelSystem.AddToGemsList(currentSceneIndex); //this checks the gem in the db - if it was collected
            SaveSystem.SaveGame();
            myAnimator.SetTrigger("isEnteringPortal");
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bouncer") //if I put this here, player will collide with the block - otherwise fall through it
        {
            float colliderPosUp = collision.transform.position.y + collision.gameObject.GetComponent<BoxCollider2D>().bounds.size.y / 2;
            float bounceSpeed = collision.gameObject.GetComponent<BounceBlock>().GetForce();
            if (transform.position.y - colliderPosUp > 0f)
            {
                Bounce(bounceSpeed);
                collision.gameObject.GetComponent<BounceBlock>().Bounce(); //this also bounces the bounceblock (bounce animation)
            }

        }
    }        

    public void PickedUpGem() //this is called in the Gem script and makes sure gem is only counted if the level is completed
    {
        hasGem = true;
    }

    private void Die() //freeze position and trigger death animation
    {
        myBodyCollider.enabled = false;
        FreezePosition();
        myAnimator.SetTrigger("isDead");
    }

    private void FreezePosition()
    {
        isAlive = false;
        myRigidbody.gravityScale = 0f;
        myRigidbody.velocity = new Vector2(0f, 0f);
    }

    /// <summary>
    /// This is used in the PlayerDeath animation last keyframe to destroy the player object. 
    /// </summary>
    public void DestroyGameobject()
    {
        Destroy(gameObject);
        FindObjectOfType<LevelChanger>().LoadScene(currentSceneIndex);
    }

    /// <summary>
    /// This is used in the PlayerPortal animation last keyframe to destroy the player object. 
    /// </summary>
    public void EnterPortal()
    {
        Destroy(gameObject);
        FindObjectOfType<LevelChanger>().LoadScene(currentSceneIndex + 1);
    }

}
