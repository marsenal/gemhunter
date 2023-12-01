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
    [SerializeField] GameObject shootingPlace;
    [SerializeField] int numberOfAttacksFirstPhase;
    [SerializeField] int numberOfAttacksSecondPhase;
    [SerializeField] float secondAttackDuration;
    [SerializeField] int numberOfAttacksThirdPhase;
    [SerializeField] float thirdAttackDuration;

    [Header("Other Elements")]
    [SerializeField] AccidSpawner accidSpawner;
    [SerializeField] float introDuration;
    [SerializeField] CrankColumn crankColumn;
    [SerializeField] CutScene cutSceneHandler;
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
        accidSpawner.enabled = false;
        impulseSource = GetComponent<CinemachineImpulseSource>();
        playableDirector = GetComponent<PlayableDirector>();
        myCollider = GetComponent<BoxCollider2D>();
        myCollider.enabled = false;
        myAnimator = GetComponent<Animator>();

        cutSceneHandler.GetComponent<BoxCollider2D>().enabled = false;

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

  /*  private void EnumMachine()
    {
        if (lives == 3 )
        {
            myPhase = AttackPhase.FirstPhase;
        }
        else if (lives == 2)
        {
            myPhase = AttackPhase.SecondPhase;
        }
        else if (lives == 1)
        {
            myPhase = AttackPhase.ThirdPhase;
        }

        if (numberOfAttacksFirstPhase <= 0 && numberOfAttacksSecondPhase > 0 && numberOfAttacksThirdPhase > 0)
        {
            myState = State.Vulnerable;
            if (timer2 <= 0f)
            {
                myState = State.Attacking;
                numberOfAttacksFirstPhase = 3;
                timer2 = vulnerableDuration;
            }
        }
        else if (numberOfAttacksSecondPhase<=0 && numberOfAttacksFirstPhase == 3 && numberOfAttacksThirdPhase > 0)
        {
            myState = State.Vulnerable;
            if (timer2 <= 0f)
            {
                myState = State.Attacking;
                numberOfAttacksSecondPhase = 3;
                timer2 = vulnerableDuration;
            }
        }
        else if (numberOfAttacksSecondPhase == 3 && numberOfAttacksFirstPhase == 3 && numberOfAttacksThirdPhase <= 0)
        {
            myState = State.Vulnerable;
            if (timer2 <= 0f)
            {
                myState = State.Attacking;
                numberOfAttacksThirdPhase = 3;
                timer2 = vulnerableDuration;
            }
        }

        if (timer3 <= 0f) //Intro handling
        {
            myState = State.Attacking;
            timer3 = introDuration;
        }
    }*/

    private void EnumMachine() //this for the easier boss - 2 lives
    {
        if (lives == 2)
        {
            myPhase = AttackPhase.FirstPhase;
        }
        else if (lives == 1)
        {
            myPhase = AttackPhase.SecondPhase;
        }

        if (numberOfAttacksFirstPhase <= 0 && numberOfAttacksSecondPhase > 0)
        {
            myState = State.Vulnerable;
            if (timer2 <= 0f)
            {
                myState = State.Attacking;
                numberOfAttacksFirstPhase = 3;
                timer2 = vulnerableDuration;
            }
        }
        else if (numberOfAttacksSecondPhase <= 0 && numberOfAttacksFirstPhase == 3)
        {
            myState = State.Vulnerable;
            if (timer2 <= 0f)
            {
                myState = State.Attacking;
                numberOfAttacksSecondPhase = 3;
                timer2 = vulnerableDuration;
            }
        }

        if (timer3 <= 0f) //Intro handling
        {
            myState = State.Attacking;
            timer3 = introDuration;
        }
    }
  /*  private void StateMachine()
    {
        switch (myPhase)
        {
            case AttackPhase.FirstPhase:
                if (myState == State.Attacking) timer -= Time.deltaTime;
                if (timer<0f && numberOfAttacksFirstPhase>0 && canAttack)
                {                    
                    ShootProjectile();
                    timer = timeBetweenAttacks;
                    numberOfAttacksFirstPhase--;
                }
                break;
            case AttackPhase.SecondPhase:
                if (myState == State.Attacking) timer -= Time.deltaTime;
                if (timer < 0f && numberOfAttacksSecondPhase > 0 && canAttack)
                {
                    StartCoroutine(SecondPhase());
                    timer = timeBetweenAttacks;
                }
                break;
            case AttackPhase.ThirdPhase:
                if (myState == State.Attacking) timer -= Time.deltaTime;
                if (timer < 0f && numberOfAttacksThirdPhase > 0 && canAttack)
                {
                    StartCoroutine(ThirdPhase());
                    timer = timeBetweenAttacks;
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
                cutSceneHandler.GetComponent<BoxCollider2D>().enabled = true;
                canAttack = false;
                break;
        }
    }*/

    private void StateMachine() // this for the easier boss - 2 phases, 2 lives
    {
        switch (myPhase)
        {
            case AttackPhase.FirstPhase:
                if (myState == State.Attacking) timer -= Time.deltaTime;
                if (timer < 0f && numberOfAttacksFirstPhase > 0 && canAttack)
                {
                    ShootProjectile();
                    timer = timeBetweenAttacks;
                    numberOfAttacksFirstPhase--;
                }
                break;
            case AttackPhase.SecondPhase:
                if (myState == State.Attacking) timer -= Time.deltaTime;
                if (timer < 0f && numberOfAttacksSecondPhase > 0 && canAttack)
                {
                    StartCoroutine(SecondPhase());
                    timer = timeBetweenAttacks;
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
                cutSceneHandler.GetComponent<BoxCollider2D>().enabled = true;
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


    private void SecondPhaseAttack()
    {
        //Instantiate(projectile, shootingPlace.transform.position, Quaternion.Euler(Vector3.forward*135f)); - this for rotating the projectile towards the target
        Instantiate(projectile, shootingPlace.transform.position, Quaternion.identity);
        AudioManager.instance.PlayClip("BossShoot");
    }

    IEnumerator SecondPhase()
    {
        SecondPhaseAttack();
        yield return new WaitForSecondsRealtime(0.5f);
        SecondPhaseAttack();
        yield return new WaitForSecondsRealtime(0.5f); 
        SecondPhaseAttack();
        yield return new WaitForSecondsRealtime(0.4f);
        numberOfAttacksSecondPhase--;
    }

    private void ThirdPhaseAttack()
    {
        Instantiate(projectile, shootingPlace.transform.position, Quaternion.identity);
        AudioManager.instance.PlayClip("BossShoot");
    }
    IEnumerator ThirdPhase()
    {
        ThirdPhaseAttack();
        yield return new WaitForSecondsRealtime(0.3f);
        ThirdPhaseAttack();
        yield return new WaitForSecondsRealtime(0.3f);
        ThirdPhaseAttack();
        yield return new WaitForSecondsRealtime(0.3f);
        numberOfAttacksThirdPhase--;
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
        lives--;
        myState = State.Hurt;
        timer2 = 1f; //timer2 keeps decreasing -> next attack phase will never trigger
    }

    IEnumerator Dying()
    {
        myState = State.Dying;
        AudioManager.instance.StopClipWithoutFade("BossTheme"); //stop boss music
        myAnimator.SetTrigger("isDead");
        AudioManager.instance.PlayClip("BossThud", true); //play thud clip looped version
        float timer = 0f;
        while (timer < dyingTimer)
        {
            timer += Time.deltaTime;
            //impulseSource.GenerateImpulseAtPositionWithVelocity(transform.position, impulseSource.m_DefaultVelocity);
            yield return null;
        }
        AudioManager.instance.StopClipWithoutFade("BossThud");
        Destroy(FindObjectOfType<BossTriggerSecondWorld>());
        endPortal.SetActive(true);
        accidSpawner.enabled = false;
    }

}
