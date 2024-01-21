using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class WorldThreeBoss : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int lives;
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
    BoxCollider2D featherAttackCollider;
    bool isMovingLeft;

    [Header("Other Elements")]
    [SerializeField] float introDuration;
    [SerializeField] float idleDuration;
    [SerializeField] ParticleSystem fallingRocks;
    [SerializeField] PlayableDirector endingScene;

    public bool isActive = false;
    CinemachineImpulseSource impulseSource;
    PlayableDirector playableDirector;
    Animator myAnimator;
    Player player;
    public float timer;
    public float timer3;
    public int counterFirstAttacks = 0;

    enum State
    {
        Dormant,
        Intro,
        ScreamWave,
        FeatherAttack,
        Idle,
        Hurt,
        Dying
    }


    enum Difficulty //for a possible harder difficulty
    {
        Normal,
        Hard
    }

    [SerializeField] State myState;
    [SerializeField] Difficulty myDifficulty;
    void Start()
    {
        startingXPosition = transform.position.x;

        impulseSource = GetComponent<CinemachineImpulseSource>();
        playableDirector = GetComponent<PlayableDirector>();
        featherAttackCollider = GetComponent<BoxCollider2D>();
        featherAttackCollider.enabled = false;
        myAnimator = GetComponent<Animator>();

        secondPhasePositionOne = secondPhaseTransformOne.position;
        secondPhasePositionTwo = secondPhaseTransformTwo.position;

        timer = idleDuration;
        timer3 = introDuration;

        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (!isActive) return;
        StateMachine();
        if (lives <= 0) //TODO: Improve this!
        {
            myState = State.Dying;
            endingScene.Play();
        }
        if (myState != State.FeatherAttack && player && //manage turning towards player if distance is big enough (5 units)
            Mathf.Abs(player.transform.position.x - transform.position.x) > 5f ) transform.localScale = new Vector2(Mathf.Sign(player.transform.position.x - transform.position.x), transform.localScale.y);
    }

    private void StateMachine()
    {
        switch (myState)
        {
            case State.Dormant:
                isActive = false;
                break;
            case State.Intro:
                if (player) player.CutsceneMode(false);
                timer3 -= Time.deltaTime; 
                if (timer3 <= 0f) //Intro handling
                {
                    myState = State.Idle;
                    timer3 = introDuration;
                    timer = idleDuration;
                }
                break;
            case State.Idle:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    myState = State.FeatherAttack;
                    timer = secondAttackDuration;
                }
                myAnimator.SetBool("isHurt", false);
                myAnimator.SetBool("featherAttack", false);
                myAnimator.SetBool("isIdle", true);
                myAnimator.SetBool("isAttacking", false);
                break;
            case State.FeatherAttack:
                myAnimator.SetBool("featherAttack", true);
                myAnimator.SetBool("isHurt", false);
                myAnimator.SetBool("isAttacking", false);
                myAnimator.SetBool("isIdle", false);
                FeatherAttack();
                break;
            case State.ScreamWave:
                featherAttackCollider.enabled = false;
                myAnimator.SetBool("isHurt", false);
                myAnimator.SetBool("isAttacking", true);
                myAnimator.SetBool("isIdle", false);
                myAnimator.SetBool("featherAttack", false);
                ScreamWave();
                break;
            case State.Hurt:
                myAnimator.SetBool("isHurt", true);
                myAnimator.SetBool("isAttacking", false);
                myAnimator.SetBool("isIdle", false);
                myAnimator.SetBool("featherAttack", false);
                timer = idleDuration;
                break;
            case State.Dying:
                Dying();
                break;
        }


    }   
   
    public void Intro() //activate boss. Used on the Timeline
    {
        AudioManager.instance.PlayClip("BossTheme", true);
        myState = State.Intro;
        isActive = true;
    }

    public void BossMusic(bool isMusicPlaying) //play the boss theme, if it is not already playing. Used on the Timeline
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
        AudioManager.instance.PlayClip("ScreamWave");
    }

    private void ScreamWave()
    {
            ShootFeathers(0);

            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                ShootProjectile();
                counterFirstAttacks++;
                timer = timeBetweenAttacks;
            }
            else if (counterFirstAttacks >= numberOfAttacksScreamWave)
            {
                timer = idleDuration;
                myState = State.Idle;
                counterFirstAttacks = 0;
            }
        
    }

    /// <summary>
    /// Pass either 0 or 1 as parameter. 0 means false, 1 means true
    /// </summary>
    /// <param name="value"></param>
    public void ShootFeathers(int value)
    {
        if (value == 1)
        {
            featherAttack.Play();

            AudioManager.instance.PlayClip("FlapWing", true);
        }
        else
        {
            featherAttack.Stop();

            AudioManager.instance.StopClipWithoutFade("FlapWing");
        }
        }

    private void FeatherAttack()
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
            myState = State.ScreamWave;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(startingXPosition, transform.position.y), 3f); //go back to the middle of the arena
            timer = timeBetweenAttacks;
        }
    }

    public void GoToIdlePhase() //used on the Hurt animation to switch to Idle
    {
        myState = State.Idle; 

    }

    public void RocksFalling() //falling rocks animation and other effects
    {
        IntroCameraShake();
        fallingRocks.Play();
        AudioManager.instance.PlayClip("BossThud");
        lives--;
        myState = State.Hurt;
        timer = idleDuration;
        counterFirstAttacks = 0;
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
    }

    public void PlayThudSound() //this is used on the dying animation in keyframes
    {
        AudioManager.instance.StopClipWithoutFade("BossThud");
        impulseSource.GenerateImpulseAtPositionWithVelocity(transform.position, impulseSource.m_DefaultVelocity);
    }

}
