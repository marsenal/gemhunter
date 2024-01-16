using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class MossBoss : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int lives;
    [SerializeField] float vulnerableDuration;
    [SerializeField] float dyingTimer;

    [Header("Attacking details and objects")]
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] MossBossProjectile projectile;
    [SerializeField] List<MossBossSecondPhase> projectileSecondPhaseLeft;
    [SerializeField] List<MossBossSecondPhase> projectileSecondPhaseRight;
    [SerializeField] GameObject shootingPlace;
    [SerializeField] int numberOfAttacksFirstPhase;

    [Header("Other Elements")]
    [SerializeField] AccidSpawner accidSpawner;
    [SerializeField] float introDuration;
    [SerializeField] CrankColumn crankColumn;
    [SerializeField] GameObject endPortal;

    public bool isActive = false;
    bool canAttack;
    bool canBeHurt;
    CinemachineImpulseSource impulseSource;
    PlayableDirector playableDirector;
    BoxCollider2D myCollider;
    Animator myAnimator;
    public float timer;
    public float timer2;
    public float timer3;
    private int counterFirstAttacks;
    public bool isSecondAttackFinished = false;
    public bool isSecondAttackStarted = false;

    enum State
    {
        Dormant,
        Intro,
        Attacking,
        Vulnerable,
        Hurt,
        Dying
    }

    enum AttackPhase
    {
        FirstPhase,
        SecondPhase,
        ThirdPhase
    }

    [SerializeField] State myState;
    [SerializeField] AttackPhase myPhase;
    void Start()
    {
        counterFirstAttacks = numberOfAttacksFirstPhase;

        accidSpawner.enabled = false;
        impulseSource = GetComponent<CinemachineImpulseSource>();
        playableDirector = GetComponent<PlayableDirector>();
        myCollider = GetComponent<BoxCollider2D>();
        myCollider.enabled = false;
        myAnimator = GetComponent<Animator>();


        timer = timeBetweenAttacks;
        timer2 = vulnerableDuration;
        timer3 = introDuration;
    }

    void Update()
    {
        if (!isActive) return;
        StateMachine();
        EnumMachine();
        if (lives <= 0) //TODO: Improve this!
        {
            StartCoroutine(Dying());
        }
    }

    private void EnumMachine() //this for the easier boss - 2 lives
    {
        if (lives == 2)
        {
            myPhase = AttackPhase.FirstPhase;

            if (counterFirstAttacks <= 0)
            {
                myState = State.Vulnerable;
                if (timer2 <= 0f)
                {
                    myState = State.Attacking;
                    counterFirstAttacks = numberOfAttacksFirstPhase;
                    timer2 = vulnerableDuration;
                }
            }
        }
        else if (lives == 1)
        {
            myPhase = AttackPhase.SecondPhase;
            if (isSecondAttackFinished)
            {
                myState = State.Vulnerable;
                canAttack = false;
                if (timer2 <= 0f)
                {
                    canAttack = true;
                    myState = State.Attacking;
                    timer2 = vulnerableDuration;
                }
            }
            else
            {
                myState = State.Attacking;
            }
        }

        if (timer3 <= 0f) //Intro handling
        {
            myState = State.Attacking;
            timer3 = introDuration;
        }
    }

    private void StateMachine() // this for the easier boss - 2 phases, 2 lives
    {
        switch (myPhase)
        {
            case AttackPhase.FirstPhase:
                if (myState == State.Attacking) timer -= Time.deltaTime;
                if (timer < 0f && counterFirstAttacks > 0 && canAttack)
                {
                    ShootProjectile();
                    timer = timeBetweenAttacks;
                    counterFirstAttacks--;
                }
                break;
            case AttackPhase.SecondPhase:

                if (myState == State.Attacking && !isSecondAttackStarted && canAttack)
                {
                    StartCoroutine(SecondPhase());
                }
                break;
        }

        switch (myState)
        {
            case State.Dormant:
                myCollider.enabled = false;
                canAttack = false;
                myAnimator.SetBool("isVulnerable", false);
                myAnimator.SetBool("isAttacking", canAttack);
                break;
            case State.Intro:
                myCollider.enabled = false;
                canAttack = false;
                timer3 -= Time.deltaTime;
                myAnimator.SetBool("isVulnerable", false);
                myAnimator.SetBool("isAttacking", canAttack);
                break;
            case State.Attacking:
                myCollider.enabled = false;
                canAttack = true;
                myAnimator.SetBool("isVulnerable", false);
                myAnimator.SetBool("isAttacking", canAttack);
                crankColumn.DeActivate();
                break;
            case State.Vulnerable:
                myCollider.enabled = true;
                canAttack = false;
                canBeHurt = true;
                timer2 -= Time.deltaTime;
                myAnimator.SetBool("isVulnerable", canBeHurt);
                myAnimator.SetBool("isAttacking", canAttack);
                crankColumn.Activate();
                break;
            case State.Hurt:
                myCollider.enabled = false;
                canAttack = false;
                canBeHurt = false;
                myAnimator.SetTrigger("isHurt");
                myAnimator.SetBool("isVulnerable", canBeHurt);
                myAnimator.SetBool("isAttacking", canAttack);
                crankColumn.DeActivate();
                break;
            case State.Dying:
                myCollider.enabled = false;
                canAttack = false;
                break;
        }
    }

    public void Intro()
    {
        AudioManager.instance.PlayClip("BossTheme", true);
        isActive = true;
        myState = State.Intro;
        //accidSpawner.enabled = true;
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
        FindObjectOfType<Player>().CutsceneMode(false);
     }

    public void Activate()
    {
        if (!AudioManager.instance.IsMusicPlaying("BossTheme")) AudioManager.instance.PlayClip("BossTheme", true);
        myState = State.Intro;
        isActive = true;
        playableDirector.Play();
        IntroCameraShake();
       // accidSpawner.enabled = true;
    }


    private void ShootProjectile()
    {
        Instantiate(projectile, shootingPlace.transform.position, Quaternion.identity);
        AudioManager.instance.PlayClip("BossShoot");
    }


    private void SecondPhaseAttack(int wave)
    {
        Instantiate(projectileSecondPhaseLeft[wave], shootingPlace.transform.position, Quaternion.identity);
        Instantiate(projectileSecondPhaseRight[wave], shootingPlace.transform.position, Quaternion.identity);
        
        AudioManager.instance.PlayClip("BossShoot");
    }

    IEnumerator SecondPhase()
    {
        isSecondAttackStarted = true;
        isSecondAttackFinished = false;
        SecondPhaseAttack(0);
        yield return new WaitForSecondsRealtime(1f);
        SecondPhaseAttack(1);
        yield return new WaitForSecondsRealtime(1f); 
        SecondPhaseAttack(2);
        yield return new WaitForSecondsRealtime(1f);
        SecondPhaseAttack(2);
        yield return new WaitForSecondsRealtime(1f);
        SecondPhaseAttack(1);
        yield return new WaitForSecondsRealtime(1f);
        SecondPhaseAttack(0);
        yield return new WaitForSecondsRealtime(1f);
        SecondPhaseAttack(0);
        yield return new WaitForSecondsRealtime(1f);
        SecondPhaseAttack(1);
        yield return new WaitForSecondsRealtime(1f);
        SecondPhaseAttack(2);
        yield return new WaitForSecondsRealtime(1f);
        isSecondAttackFinished = true;
        canAttack = false;
        isSecondAttackStarted = false;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Weapon" && canBeHurt)
        {
            canBeHurt = false;
            Hurt();
        }
    }
    public void Hurt()
    {
        impulseSource.GenerateImpulseAtPositionWithVelocity(transform.position, impulseSource.m_DefaultVelocity);
        AudioManager.instance.PlayClip("BossThud");
        lives--;
        myState = State.Hurt;
        timer2 = vulnerableDuration; //timer2 keeps decreasing -> next attack phase will never trigger
    }

    IEnumerator Dying()
    {
        myState = State.Dying;
        AudioManager.instance.StopClipWithoutFade("BossTheme"); //stop boss music
        myAnimator.SetTrigger("isDead");
      //  AudioManager.instance.PlayClip("BossThud", true); //play thud clip looped version
      //  float timer = 0f;
       // while (timer < dyingTimer)
        {
            timer += Time.deltaTime;
            impulseSource.GenerateImpulseAtPositionWithVelocity(transform.position, impulseSource.m_DefaultVelocity);
            yield return null;
        }
        Destroy(FindObjectOfType<BossTriggerSecondWorld>());
        accidSpawner.enabled = false;
    }

    public void PlayThudSound() //this is used on the dying animation in keyframes
    {
        AudioManager.instance.StopClipWithoutFade("BossThud");
        impulseSource.GenerateImpulseAtPositionWithVelocity(transform.position, impulseSource.m_DefaultVelocity);
    }

    public void EnableEndPortal()
    {
        endPortal.SetActive(true);
    }

}
