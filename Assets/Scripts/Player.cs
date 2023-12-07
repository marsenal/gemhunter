using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Cinemachine;

public class Player : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    Vector2 moveInput;
    Animator myAnimator;
    BoxCollider2D myBodyCollider;
    CinemachineImpulseSource impulseSource;

    [Header("Set these for Jumping")]
    [SerializeField] LayerMask groundLayerMask;

    [Header("Movement Values")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] PhysicsMaterial2D highFrictMat;
    [SerializeField][Tooltip("Player is able to jump for this many seconds after walking off a cliff.")]
    [Range(0f,0.5f)] float coyoteSeconds;
    bool canJump;
    float coyoteTimer = 0f;
    [SerializeField] float dashSpeed;

    [Header("Visual things")]
    [SerializeField] DustTrail dustTrail;
    [SerializeField] DustTrail dustTrailLanding;

    [Header("Control")]
    [SerializeField] Canvas controlCanvas;
    [SerializeField] Button dashButton;

    bool isMovingLeft = false;
    bool isMovingRight = false;

    bool isAlive = true;
    bool hasGem = false;

    [HideInInspector] public bool isDashing;
    bool canDash = true;

    float dashingTime = 0.3f;
    float originalGravity;

    int currentSceneIndex;

    enum State
    {
        Idle,
        Running,
        Jumping,
        Falling,
        Dashing,
    }

    [SerializeField] State myState;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<BoxCollider2D>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        originalGravity = myRigidbody.gravityScale;

        isAlive = false;
    }

    void Update()
    {
        if (!isAlive) { return; }
        Move();
        FlipSprite();
        EnumMachine();
        StateMachine();
        CoyoteBuffer();
        DashCooldown();
    }

    private void FixedUpdate()
    {
        
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
        if (!isDashing)
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
    }

    private void FlipSprite()
    {
        if (Mathf.Abs(myRigidbody.velocity.x) > 0f)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), transform.localScale.y);
        }
    }

    private void EnumMachine()
    {
        if (Mathf.Abs(myRigidbody.velocity.x) > 0f && IsGrounded() && !isDashing)
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
        else if (isDashing)
        {
            myState = State.Dashing;
        }
        else
        { myState = State.Idle; }
    }

    private void StateMachine()
    {
        switch (myState)
        {
            case State.Running:
                myAnimator.SetBool("isRunning", true);
                myAnimator.SetBool("isFalling", false);
                myAnimator.SetBool("isJumping", false);
                myAnimator.SetBool("isDashing", false);
                break;
            case State.Jumping:
                myAnimator.SetBool("isJumping", true);
                myAnimator.SetBool("isRunning", false);
                myAnimator.SetBool("isFalling", false);
                myAnimator.SetBool("isDashing", false);
                break;
            case State.Falling:
                myAnimator.SetBool("isFalling", true);
                myAnimator.SetBool("isRunning", false);
                myAnimator.SetBool("isDashing", false);
                myAnimator.SetBool("isJumping", false);
                break;
            case State.Idle:
                myAnimator.SetBool("isRunning", false);
                myAnimator.SetBool("isJumping", false);
                myAnimator.SetBool("isDashing", false);
                myAnimator.SetBool("isFalling", false);
                break;
            case State.Dashing:
                myAnimator.SetBool("isDashing", true);
                myAnimator.SetBool("isRunning", false);
                myAnimator.SetBool("isJumping", false);
                myAnimator.SetBool("isFalling", false);
                break;
        }
            
        
    }

    public void OnDash(InputAction.CallbackContext context) //for keyboard control
    {
        //myRigidbody.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * moveSpeed, 0f);
        if (isAlive) Dash(true);

    }

    public void Dash(bool value) //for touch input
    {
        if (canDash & value && isAlive)
        {
            AudioManager.instance.PlayClip("Dash");
            myRigidbody.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * dashSpeed, 0f);
            myBodyCollider.sharedMaterial = null;
            myRigidbody.gravityScale = 0f;
            isDashing = true;
        }
        else if (isAlive)
        {
            isDashing = false;
            myBodyCollider.sharedMaterial = highFrictMat;
            myRigidbody.gravityScale = originalGravity;
        }
    }

   /* private void DashTimer() //it's working, but could be simplified FOR SURE
    {
        dashButton.interactable = canDash;
        if (!isDashing & dashCooldown == 0.4f)
        {
            canDash = true;
        }
        else if (isDashing)
        {
            canDash = false;
            dashingTime -= Time.deltaTime;
            if (dashingTime < 0f)
            {
                Dash(canDash);
                dashingTime = 0.2f;
                dashCooldown -= Time.deltaTime;
            }
        }
        else if (dashCooldown < 0.4f)
        {
            dashCooldown -= Time.deltaTime;
            canDash = false;
            if (dashCooldown  < 0f & IsGrounded())
            {
                dashCooldown = 0.4f;
            }
        }
    }*/

    private void DashCooldown() 
    {
        if (dashButton == null) return;
        dashButton.interactable = canDash;
        if (IsGrounded() && !isDashing)
        {
            canDash = true;
        }
        if (isDashing)
        {
            canDash = false;
            dashingTime -= Time.deltaTime;
            if (dashingTime < 0f)
            {
                Dash(canDash);
                dashingTime = 0.3f;
            }
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
        if ((IsGrounded() || canJump) &&  value && isAlive)
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
        canDash = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Hazard"))
        {
            Die();
        }
        else if (collision.CompareTag("Enemy") && collision.GetComponent<BoxCollider2D>() != null)
        {
            float colliderPosUp = collision.transform.position.y + collision.GetComponent<BoxCollider2D>().bounds.size.y / 2;
            if (transform.position.y - colliderPosUp > 0f )
            {
                Bounce(1200f);
                collision.GetComponent<Enemy>().Die();
                AudioManager.instance.PlayClip("JumpSound");
            }
            else
            {
                Die();
            }
        }
        else if (collision.tag == "EndPortal")
        {
            CutScene cutscene = FindObjectOfType<CutScene>(); //delete the dontdestroyonload objects
            BossTrigger bosstrigger = FindObjectOfType<BossTrigger>();
            BossTriggerSecondWorld secondlvlbosstrigger = FindObjectOfType<BossTriggerSecondWorld>(); 
            if (bosstrigger) Destroy(bosstrigger.gameObject);
            if (secondlvlbosstrigger) Destroy(secondlvlbosstrigger.gameObject);
            //if (cutscene != null) cutscene.DestroyMe(); //destroy the dontdestroyonload cutscene

            FreezePosition();
            transform.position = collision.transform.position; //align player with the portal
            myAnimator.SetTrigger("isEnteringPortal");
            impulseSource.GenerateImpulse();
            AudioManager.instance.PlayClip("Portal");

            LevelSystem.AddToLevelList(currentSceneIndex); //this checks out the level in the db
            if (hasGem) LevelSystem.AddToGemsList(currentSceneIndex); //this checks out the gem in the db - if it was collected
            SaveSystem.SaveGame();
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

    private void FreezePosition() //disable player movement and collision
    {
        isAlive = false;
        myBodyCollider.enabled = false;
        myRigidbody.gravityScale = 0f;
        myRigidbody.velocity = new Vector2(0f, 0f);
    }

    /// <summary>
    /// If true, in a cutscene hence player cant act - disable control canvas.
    /// </summary>
    /// <param name="value"></param>
    public void CutsceneMode(bool value) //for the boss (or any) cutscene
    {
        if (controlCanvas == null) return;
        controlCanvas.enabled = !value;
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
    /// <summary>
    /// This is used in the PlayerSpawn animation last keyframe to give control to player.
    /// </summary>
    private void Alive()
    {
        isAlive = true;
    }
}
