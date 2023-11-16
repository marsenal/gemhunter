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
    [Header("Attacking details and objects")]
    [SerializeField] float timeBetweenAttacks;
    [SerializeField] MossBossProjectile projectile;
    [SerializeField] GameObject shootingPlace;
    [SerializeField] int numberOfAttacksFirstPhase;
    [SerializeField] int numberOfAttacksSecondPhase;
    [SerializeField] float secondAttackDuration;
    [SerializeField] int numberOfAttacksThirdPhase;
    [SerializeField] float thirdAttackDuration;
    public bool isActive = false;
    CinemachineImpulseSource impulseSource;
    PlayableDirector playableDirector;
    float timer;
    float timer2;

    enum State
    {
        FirstPhase,
        SecondPhase,
        ThirdPhase,
        Vulnerable,
        Dying
    }

    State myState;
    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        playableDirector = GetComponent<PlayableDirector>();

        timer = timeBetweenAttacks;
        timer2 = secondAttackDuration;
    }

    void Update()
    {
        if (!isActive) return;
        StateMachine();
        EnumMachine();
    }

    private void EnumMachine()
    {
        if (lives == 3)
        {
            myState = State.FirstPhase;
        }
        else if (lives == 2)
        {
            myState = State.SecondPhase;
        }
        else if (lives == 1)
        {
            myState = State.ThirdPhase;
        }
    }
    private void StateMachine()
    {
        switch (myState)
        {
            case State.FirstPhase:
                timer -= Time.deltaTime;
                if (timer<0f && numberOfAttacksFirstPhase>0)
                {                    
                    ShootProjectile();
                    timer = timeBetweenAttacks;
                    numberOfAttacksFirstPhase--;
                }
                break;
            case State.SecondPhase:
                timer -= Time.deltaTime;
                if (timer < 0f && numberOfAttacksSecondPhase > 0 && timer2 > 0f)
                {
                    StartCoroutine(SecondPhase());
                    numberOfAttacksSecondPhase--;
                }
                break;
            case State.ThirdPhase:
                timer -= Time.deltaTime;
                if (timer < 0f && numberOfAttacksThirdPhase > 0)
                {
                    ThirdPhaseAttack();
                    timer = timeBetweenAttacks;
                    numberOfAttacksThirdPhase--;
                }
                break;
            case State.Vulnerable:
                break;
            case State.Dying:
                break;
        }
    }

    public void Intro(bool isMusicPlaying)
    {
        if (isMusicPlaying) AudioManager.instance.PlayClip("BossTheme", true);
        else AudioManager.instance.StopClipWithoutFade("BossTheme");
        isActive = true;
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
        isActive = true;
        playableDirector.Play();
        IntroCameraShake();
    }

    private void ShootProjectile()
    {
        Instantiate(projectile, shootingPlace.transform.position, Quaternion.identity);       
    }


    private void SecondPhaseAttack()
    {
        Instantiate(projectile, shootingPlace.transform.position, Quaternion.identity);  
    }

    IEnumerator SecondPhase()
    {
        SecondPhaseAttack();
        yield return new WaitForSecondsRealtime(0.2f);
        SecondPhaseAttack();
        yield return new WaitForSecondsRealtime(0.2f); 
        SecondPhaseAttack();
        yield return new WaitForSecondsRealtime(0.2f);
    }

    private void ThirdPhaseAttack()
    {

    }

    public void Hurt()
    {
        lives--;
    }
}
