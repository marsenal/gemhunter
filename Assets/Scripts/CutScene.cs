using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;


public class CutScene : MonoBehaviour
{

    PlayableDirector playableDirector;
    Canvas skipButtonCanvas;
    Button dashButton; //not the best, TODO: think of reworking!

    public bool hasPlayed = false;
    bool playItAgain = false;

    public static CutScene instance;
    /*private void Awake() //this is needed here to know after death wether the cutscene already played
    {
        if (instance == null)  //singleton pattern
        {
            instance = this;
        }
        else { Destroy(gameObject); }

        DontDestroyOnLoad(gameObject);
    }*/
    void Start()
    {
        
        skipButtonCanvas = GetComponentInChildren<Canvas>();
        if (skipButtonCanvas != null) skipButtonCanvas.enabled = false;
        
        playableDirector = GetComponent<PlayableDirector>();
        /*if (hasPlayed)
        {
            dashButton.enabled = true;
            dashButton.image.enabled = true;
        }*/
        
        //skipButtonCanvas.enabled = false;
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasPlayed)
        {
            if (skipButtonCanvas != null) skipButtonCanvas.enabled = true;
            playableDirector.Play();
            collision.GetComponent<Player>().CutsceneMode(true);
            hasPlayed = true;
        }
        else
        {
                /*skipButtonCanvas = GetComponentInChildren<Canvas>();
                playableDirector.Play();
                collision.GetComponent<Player>().CutsceneMode(true);
                skipButtonCanvas.enabled = true;*/
            
        }
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void Skip()
    {
        FindObjectOfType<SceneChanger>().CutSceneFade();
        playableDirector.Stop();       
        skipButtonCanvas.enabled = false;
        FindObjectOfType<Player>().CutsceneMode(false);
        DestroyMe();
    }


}
