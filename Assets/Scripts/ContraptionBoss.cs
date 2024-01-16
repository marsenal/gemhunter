using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
public class ContraptionBoss : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    Player playerToAttack;
    Animator myAnimator;
    BoxCollider2D myCollider;
    bool isCharging;
    public bool isActive = false;
    [SerializeField] bool isSecondWorldVersion;
    bool portalSpawned = false; //this is only needed if we use the spawning portal version
    Vector2 chargeDestination;
    float direction;
    float timer;

    [Header("Main attributes")]
    [SerializeField] int lives;
    [SerializeField] float dyingTimer = 5f;

    [Header("Charge attributes")]
    [SerializeField] [Tooltip("The extra distance boss covers with charge.")] float overChargeDistance;
    [SerializeField] float chargeTimer;

    [Header("Miscellaneous objects")]
    [SerializeField] GameObject dustTrailRight;
    [SerializeField] GameObject dustTrailLeft;
    [SerializeField] GameObject endPortal;
    [SerializeField] GameObject portalPosition;
    [SerializeField] TextMeshPro bossText;

    CinemachineImpulseSource impulseSource;
    private void Awake()
    {
        /*if (isSecondWorldVersion)
        {
            int numberOfBosses = FindObjectsOfType<ContraptionBoss>().Length;
            if (numberOfBosses > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }*/
    }
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        playerToAttack = FindObjectOfType<Player>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        timer = chargeTimer;
    }

    void Update()
    {
        if (!isActive) { return; }
        ChargePlayer();
        ChargerCountDown();
        PlayParticle();
    }

    public void Activate()
    {
        isActive = true;
        if (!AudioManager.instance.IsMusicPlaying("BossTheme"))  AudioManager.instance.PlayClip("BossTheme", true);
        playerToAttack.CutsceneMode(false);
        bossText.text = "";
    }

    private void ChargerCountDown()
    {
        if (timer < 0f && playerToAttack!= null)
        {
            isCharging = true;
            float sign = Mathf.Sign(transform.position.x - playerToAttack.transform.position.x);
            chargeDestination = new Vector2(playerToAttack.transform.position.x - sign * overChargeDistance, transform.position.y);


            direction = playerToAttack.transform.position.x - transform.position.x;
            timer = chargeTimer;
        }
        else if (!isCharging)
        {
            timer = timer - Time.deltaTime;
        }
    }

    private void ChargePlayer()
    {
        if (isCharging)
        {
            myRigidbody.velocity = new Vector2(Math.Sign(direction) * 7f, 0f);

            float difference = transform.position.x - chargeDestination.x;
            if (Mathf.Abs(difference) < 1f)
            {
                isCharging = false;
            }
        }
        else
        {
            myRigidbody.velocity = new Vector2(0f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" ||collision.gameObject.tag == "Player")
        {
            isCharging = false;
        }
        if (collision.gameObject.tag == "Hazard")
        {
            lives--;
            ShakeCamera();
            myAnimator.SetTrigger("isDamaged");
            AudioManager.instance.PlayClip("BossThud");
            if (lives<0)
            {
                StartCoroutine(Dying());
            }
        }
    }
    IEnumerator Dying()
    {
        myCollider.enabled = false; //so cannot kill player any more
        AudioManager.instance.StopClipWithoutFade("BossTheme"); //stop boss music
        myAnimator.SetTrigger("isDead"); 
        AudioManager.instance.PlayClip("BossThud", true); //play thud clip looped version
        float timer = 0f;
        while (timer < dyingTimer)
        {
            timer += Time.deltaTime;
            ShakeCamera();
            yield return null;
        }
        isActive = false; //so it stops charging
        AudioManager.instance.StopClipWithoutFade("BossThud");
        Destroy(FindObjectOfType<BossTrigger>());
        // if (!portalSpawned) Instantiate(endPortal, portalPosition.transform.position, Quaternion.identity);
        // portalSpawned = true;

        endPortal.gameObject.SetActive(true);
    }

    private void PlayParticle()
    {
        if (!isCharging)
        {
            dustTrailRight.SetActive(false);
            dustTrailLeft.SetActive(false);
        }
        else if (myRigidbody.velocity.x < 0)
        {
            dustTrailLeft.SetActive(false);
            dustTrailRight.SetActive(true);
        }
        else if (myRigidbody.velocity.x > 0)
        {
            dustTrailLeft.SetActive(true);
            dustTrailRight.SetActive(false);
        }
    }

    private void ShakeCamera() //use this to shake the camera on command
    {
        impulseSource.GenerateImpulseAtPositionWithVelocity(transform.position, impulseSource.m_DefaultVelocity);
    }

}
