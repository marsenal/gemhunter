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
    [SerializeField] PhysicsMaterial2D highFrictMat;
    [SerializeField][Tooltip("Player is able to jump for this many seconds after walking off a cliff.")]
    [Range(0f,0.5f)] float coyoteSeconds;
    bool canJump;
    float coyoteTimer = 0f;

    [Header("Visual things")]
    [SerializeField] DustTrail dustTrail;
    [SerializeField] DustTrail dustTrailLanding;

    bool isMovingLeft = false;
    bool isMovingRight = false;

    bool isAlive = true;
    bool hasGem = false;

    int currentSceneIndex;

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

    void Update()
    {
        if (!isAlive) { return; }
        Move();
        FlipSprite();
        StateMachine();
        EnumMachine();
        CoyoteBuffer();
    }
    public void OnBack(InputAction.CallbackContext context) //for pushing back button - this doesn't work currently
    {
        Application.Quit();
        Debug.Log("Quitting");
    }
    public void OnMove(InputAction.CallbackContext context) //for keyboard control
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void MoveLeft(bool value) //for left button touch input
    {
        isMovingLeft = value;
    }
    public void MoveRight(bool value)//for right button touch input
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
        if ((IsGrounded() || canJump) && isAlive)
        {
            myRigidbody.velocity = new Vector2(moveInput.x * moveSpeed, jumpSpeed);
            Instantiate(dustTrail, transform.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().PlayClip("JumpSound");
        }
    }

    public void Jump(bool value) //for using touch input
    {
        if ((IsGrounded() || canJump) &&  value && isAlive )
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpSpeed);
            Instantiate(dustTrail, transform.position, Quaternion.identity);
            AudioManager.instance.PlayClip("JumpSound");
        }
    }

    bool IsGrounded() //cast a box under the player which detects collision with ground layer
    {
        float boxWidth = myBodyCollider.bounds.size.x;
        if (myState == State.Idle) myBodyCollider.sharedMaterial = highFrictMat; //change to a high friction material, so moving platforms would drag me
        else  myBodyCollider.sharedMaterial = null; //change the material back to null, so running is possible
        Vector2 boxSize = new Vector2(boxWidth, transform.localScale.y / 2);
        Vector2 boxOrigin = new Vector2 (transform.position.x, transform.position.y - transform.localScale.y / 2);
        RaycastHit2D hit = Physics2D.BoxCast(boxOrigin, boxSize, 0f, Vector2.down, 0.1f, groundLayerMask);
        return hit.collider != null;
    }

    private void CoyoteBuffer() //this enabled coyote timer - slight delay on the ability to jump after falling from a platform
    {
        if (!IsGrounded())
        {
            coyoteTimer += Time.deltaTime;

            if (myState == State.Falling)
            {
                if (coyoteTimer < coyoteSeconds)
                {
                    canJump = true;
                }
                else
                {
                    canJump = false;
                }
            }
            else
            {
                canJump = false;
            }
        }
        else
        {
            canJump = false;
            coyoteTimer = 0f;
        }
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
        else if (collision.tag == "EndPortal")
        {
            FreezePosition();
            transform.position = collision.transform.position; //align player with the portal
            AudioManager.instance.PlayClip("Portal");
            LevelSystem.AddToLevelList(currentSceneIndex); //this checks out the level in the db
            if (hasGem) LevelSystem.AddToGemsList(currentSceneIndex); //this checks out the gem in the db - if it was collected
            SaveSystem.SaveGame();
            myAnimator.SetTrigger("isEnteringPortal");
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bouncer") //if I put this here, player will collide with the block - otherwise fall through it
        {
            BounceBlock block = collision.gameObject.GetComponent<BounceBlock>();
            float collidersTopPosition = collision.transform.position.y + collision.gameObject.GetComponent<BoxCollider2D>().bounds.size.y / 2;
            float bounceSpeed = block.GetForce();
            if (transform.position.y - collidersTopPosition > 0f) //IF player vertical pos is higher than block's top vertical position
            {
                Bounce(bounceSpeed);
                block.Bounce(); //this also bounces the bounceblock (bounce animation)
            }
        }
        if (IsGrounded() & collision.gameObject.tag == "Ground") //dusttrail when landing on Ground
        {
            Instantiate(dustTrailLanding, transform.position, Quaternion.identity);
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
        AudioManager.instance.PlayClip("Dying");
        myAnimator.SetTrigger("isDead");
    }

    private void FreezePosition() //disable player movement 
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
        FindObjectOfType<SceneChanger>().LoadScene(currentSceneIndex);
    }

    /// <summary>
    /// This is used in the PlayerPortal animation last keyframe to destroy the player object. 
    /// </summary>
    public void EnterPortal()
    {
        Destroy(gameObject);
        FindObjectOfType<SceneChanger>().LoadScene(currentSceneIndex + 1);
    }

}
