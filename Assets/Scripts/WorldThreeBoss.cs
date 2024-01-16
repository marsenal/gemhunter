using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class WorldThreeBoss : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int lives;
    [SerializeField] float vulnerableDuration;
    [SerializeField] float dyingTimer;

    [Header("General attacking details and objects")]
    [SerializeField] int numberOfAttacksThirdPhase;
    [SerializeField] float thirdAttackDuration;
    float startingXPosition;

    [Header("First Phase")]
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] ScreamWave projectile;
    [SerializeField] GameObject shootingPlace;
    [SerializeField] int numberOfAttacksScreamWave;


    [Header("Feather Attack")]
    [SerializeField] float secondAttackDuration;
    [SerializeField] Transform secondPhaseTransformOne;
    [SerializeField] Transform secondPhaseTransformTwo;
    private Vector2 secondPhasePositionOne;
    private Vector2 secondPhasePositionTwo;
    [SerializeField] ParticleSystem featherAttack;
    bool isMovingLeft;

    [Header("Other Elements")]
    [SerializeField] float introDuration;
   // [SerializeField] CutScene cutSceneHandler;
   // [SerializeField] GameObject endPortal;
    [SerializeField] ParticleSystem fallingRocks;
    [SerializeField] PlayableDirector endingScene;

    public bool isActive = false;
    bool canAttack;
    CinemachineImpulseSource impulseSource;
    PlayableDirector playableDirector;
    BoxCollider2D featherAttackCollider;
    Animator myAnimator;
    Player player;
    public float timer;
    public float timer2;
    public float timer3;
    public int counterFirstAttacks = 0;

    enum State
    {
        Dormant,
        Intro,
        Attacking,
        Idle,
        Hurt,
        Dying
    }

    enum AttackPhase
    {
        FirstPhase,
        SecondPhase,
        ThirdPhase
    }

    enum Difficulty
    {
        Normal,
        Hard
    }

    [SerializeField] State myState;
    [SerializeField] AttackPhase myPhase;
    [SerializeField] Difficulty myDifficulty;
    void Start()
    {
        startingXPosition = transform.position.x;

        impulseSource = GetComponent<CinemachineImpulseSource>();
        playableDirector = GetComponent<PlayableDirector>();
        featherAttackCollider = GetComponent<BoxCollider2D>();
        featherAttackCollider.enabled = false;
        myAnimator = GetComponent<Animator>();

        // cutSceneHandler.GetComponent<BoxCollider2D>().enabled = false;

        secondPhasePositionOne = secondPhaseTransformOne.position;
        secondPhasePositionTwo = secondPhaseTransformTwo.position;

        timer = timeBetweenAttacks;
        timer3 = introDuration;

        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (!isActive) return;
        StateMachine();
        EnumMachine();
        if (lives <= 0) //TODO: Improve this!
        {
            myState = State.Dying;
            endingScene.Play();
        }
        if (myPhase != AttackPhase.SecondPhase && player && //manage turning towards player if distance is big enough (5 units)
            Mathf.Abs(player.transform.position.x - transform.position.x) > 5f ) transform.localScale = new Vector2(Mathf.Sign(player.transform.position.x - transform.position.x), transform.localScale.y);
    }


    private void EnumMachine() //this for the easier boss - 2 lives
    {
        if (timer2 <= 0f)
        {
            myState = State.Attacking;
            timer3 = introDuration;
            switch (myPhase) {
                case AttackPhase.FirstPhase:
                    timer2 = 5f;
                    timer = timeBetweenAttacks;
                    break;
                case AttackPhase.SecondPhase:
                    timer2 = 5f;
                    timer = secondAttackDuration;
                        break;
            }
        }

        if (timer3 <= 0f) //Intro handling
        {
            myState = State.Attacking;
            timer3 = introDuration;
        }

        switch (lives)
        {
            /*case 3:
                myPhase = AttackPhase.FirstPhase;
                break;*/
           /* case 2:
                myPhase = AttackPhase.SecondPhase;
                break;
            case 1:
                myPhase = AttackPhase.ThirdPhase;
                break;*/
        }    
    }
   
    private void StateMachine() // 
    {
        switch (myPhase)
        {
            case AttackPhase.FirstPhase:
                if (myState == State.Attacking)
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0f)
                    {
                        ShootProjectile();
                        counterFirstAttacks++;
                        timer = timeBetweenAttacks;
                    }
                    else if (counterFirstAttacks >= numberOfAttacksScreamWave)
                    {
                        timer = secondAttackDuration;
                        myPhase = AttackPhase.SecondPhase;
                        counterFirstAttacks = 0;
                    }
                }
                break;
            case AttackPhase.SecondPhase:

                if (myState == State.Attacking &&  canAttack)
                {
                    timer -= Time.deltaTime;
                    if (timer > 0f)
                    {
                        if (transform.position.x == secondPhasePositionOne.x)
                        {
                            isMovingLeft = false;

                        }
                        else if (transform.position.x == secondPhasePositionTwo.x)
                        {
                            isMovingLeft = true;
                            featherAttackCollider.enabled = true;
                            ShootFeathers(1);
                        }

                        if (!isMovingLeft)
                        {
                            transform.localScale = new Vector2(1, 1);
                            transform.position = Vector2.MoveTowards(transform.position, secondPhasePositionTwo, 10f * Time.deltaTime);
                        }
                        else
                        {
                            transform.localScale = new Vector2(-1, 1);
                            transform.position = Vector2.MoveTowards(transform.position, secondPhasePositionOne, 10f * Time.deltaTime);
                        }
                    }
                    else
                    {
                        myPhase = AttackPhase.FirstPhase;
                        featherAttackCollider.enabled = false;
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(startingXPosition, transform.position.y), 1f);
                        timer = timeBetweenAttacks;
                    }
                }
                break;
            case AttackPhase.ThirdPhase:
                if (myState == State.Attacking && canAttack)
                {
                    Debug.Log("Third Phase");
                }
                break;
        }

        switch (myState)
        {
            case State.Dormant:
                canAttack = false;
                break;
            case State.Intro:
                if (player) player.CutsceneMode(false);
                canAttack = false;
                timer3 -= Time.deltaTime;
                break;
            case State.Attacking:
                canAttack = true;
                myAnimator.SetBool("isHurt", false);
                break;
            case State.Idle:
                canAttack = false;
                myAnimator.SetBool("isHurt", false);
                break;
            case State.Hurt:
                canAttack = false;
                myAnimator.SetBool("isHurt", true);
                timer2 -= Time.deltaTime;
                break;
            case State.Dying:
                canAttack = false;
                Dying();
                break;
        }
        myAnimator.SetBool("isAttacking", canAttack);
    }

    public void Intro()
    {
        AudioManager.instance.PlayClip("BossTheme", true);
        isActive = true;
        myState = State.Intro;
    }

    public void BossMusic(bool isMusicPlaying)
    {
        if (isMusicPlaying) AudioManager.instance.PlayClip("BossTheme", true);
        else AudioManager.instance.StopClipWithoutFade("BossTheme");

    }

    public void IntroCameraShake()
    {
        StartCoroutine(ShakeCamera());
    }

    IEnumerator ShakeCamera()
    {
        float timer = 0f;
        while (timer < 3f)
        {

            impulseSource.GenerateImpulseAtPositionWithVelocity(transform.position, impulseSource.m_DefaultVelocity);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void Activate()
    {
        if (!AudioManager.instance.IsMusicPlaying("BossTheme")) AudioManager.instance.PlayClip("BossTheme", true);
        myState = State.Intro;
        isActive = true;
        playableDirector.Play();
        IntroCameraShake();
    }


    private void ShootProjectile()
    {
        Instantiate(projectile, shootingPlace.transform.position, Quaternion.identity);
        AudioManager.instance.PlayClip("BossShoot");
    }

    /// <summary>
    /// Pass either 0 or 1 as parameter. 0 means false, 1 means true
    /// </summary>
    /// <param name="value"></param>
    public void ShootFeathers(int value)
    {
        /*Instantiate(projectile, shootingPlace.transform.position, Quaternion.identity);
        AudioManager.instance.PlayClip("BossShoot");*/
       
        if (value == 1) featherAttack.Play();
        else featherAttack.Stop();
 
    }

    public void GoToSecondPhase()
    {
        myPhase = AttackPhase.SecondPhase; //after rocks falling, phase should be the feather attack

    }

    public void RocksFalling()
    {
        IntroCameraShake();
        fallingRocks.Play();
        AudioManager.instance.PlayClip("BossThud");
        GoToSecondPhase();
        lives--;
        myState = State.Hurt;
        timer2 = 5f; //timer2 keeps decreasing -> next attack phase will never trigger
    }

    public void Dying()
    {
        StartCoroutine(DyingCoroutine());
    }

    IEnumerator DyingCoroutine()
    {
        AudioManager.instance.StopClipWithoutFade("BossTheme"); //stop boss music
        Destroy(FindObjectOfType<BossTriggerWorldThree>());
        endingScene.Play();
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        //endPortal.SetActive(true);
    }

    public void PlayThudSound() //this is used on the dying animation in keyframes
    {
        AudioManager.instance.StopClipWithoutFade("BossThud");
        impulseSource.GenerateImpulseAtPositionWithVelocity(transform.position, impulseSource.m_DefaultVelocity);
    }

}
