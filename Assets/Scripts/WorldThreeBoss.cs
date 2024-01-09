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

    [Header("Attacking details and objects")]
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] ScreamWave projectile;
    [SerializeField] GameObject shootingPlace;
    [SerializeField] int numberOfAttacksScreamWave;
    [SerializeField] float secondAttackDuration;
    [SerializeField] int numberOfAttacksThirdPhase;
    [SerializeField] float thirdAttackDuration;

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
    BoxCollider2D myCollider;
    Animator myAnimator;
    Player player;
    public float timer;
    public float timer2;
    public float timer3;
    public int counterFirstAttacks;

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
        impulseSource = GetComponent<CinemachineImpulseSource>();
        playableDirector = GetComponent<PlayableDirector>();
        myCollider = GetComponent<BoxCollider2D>();
        myCollider.enabled = false;
        myAnimator = GetComponent<Animator>();

       // cutSceneHandler.GetComponent<BoxCollider2D>().enabled = false;

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
        if (player && //manage turning towards player if distance is big enough (5 units)
            Mathf.Abs(player.transform.position.x - transform.position.x) > 5f ) transform.localScale = new Vector2(Mathf.Sign(player.transform.position.x - transform.position.x), transform.localScale.y);
    }


    private void EnumMachine() //this for the easier boss - 2 lives
    {
        if (timer2 < 0f)
        {
            myState = State.Attacking;
            timer3 = introDuration;
        }

        if (timer3 <= 0f) //Intro handling
        {
            myState = State.Attacking;
            timer3 = introDuration;
        }

        switch (lives)
        {
            case 3:
                myPhase = AttackPhase.FirstPhase;
                break;
            case 2:
                myPhase = AttackPhase.SecondPhase;
                break;
            case 1:
                myPhase = AttackPhase.ThirdPhase;
                break;
        }    
    }
   
    private void StateMachine() // this for the easier boss - 2 phases, 2 lives
    {
        switch (myPhase)
        {
            case AttackPhase.FirstPhase:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    ShootProjectile();
                    timer = timeBetweenAttacks;
                }
                break;
            case AttackPhase.SecondPhase:

                if (myState == State.Attacking &&  canAttack)
                {
                    Debug.Log("Second Phase");
                    if (timer <= 0f)
                    {
                        SecondPhaseAttack();
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
                myCollider.enabled = false;
                canAttack = false;
                break;
            case State.Intro:
                myCollider.enabled = false;
                if (player) player.CutsceneMode(false);
                canAttack = false;
                timer3 -= Time.deltaTime;
                break;
            case State.Attacking:
                myCollider.enabled = true;
                canAttack = true;
                myAnimator.SetBool("isHurt", false);
                break;
            case State.Idle:
                myCollider.enabled = true;
                canAttack = false;
                myAnimator.SetBool("isHurt", false);
                break;
            case State.Hurt:
                myCollider.enabled = true;
                canAttack = false;
                myAnimator.SetBool("isHurt", true);
                timer2 -= Time.deltaTime;
                break;
            case State.Dying:
                myCollider.enabled = false;
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

    private void SecondPhaseAttack()
    {

    }


    public void RocksFalling()
    {
        IntroCameraShake();
        fallingRocks.Play();
        AudioManager.instance.PlayClip("BossThud");
        lives--;
        myState = State.Hurt;
        counterFirstAttacks = numberOfAttacksScreamWave;
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
