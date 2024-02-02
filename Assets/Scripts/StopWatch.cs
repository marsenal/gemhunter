using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class StopWatch : MonoBehaviour
{
    private float time;
    private bool isStopWatchRunning;

    [SerializeField] TextMeshProUGUI stopWatchText;
    [SerializeField] Image stopWatchButton;
    [SerializeField] Image stopWatchBackground;

    private void Awake()
    {
        if (LevelSystem.IsEveryLevelCompleted())
        {
            stopWatchButton.enabled = true;
        }
        else stopWatchButton.enabled = false;
    }

    void Update()
    {
        if (isStopWatchRunning)
        {
            stopWatchBackground.enabled = true;
            stopWatchText.enabled = true;
            time += Time.deltaTime;
            stopWatchText.text = TimeSpan.FromSeconds(time).ToString(@"m\:ss");
        }
        else
        {
            stopWatchBackground.enabled = false;
            stopWatchText.enabled = false;
        }
    }

    public void StartTimer() //used on the StopWatch image event trigger
                             //TODO: use a new bool to recognize if it is a speedrun - treat slightly differently 
    {

        int numberOfTriggers = FindObjectsOfType<BossTriggerSecondWorld>().Length;
        if (numberOfTriggers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        FindObjectOfType<SceneChanger>().LoadScene(36);
        time = 0f;
        isStopWatchRunning = true;
    }

    public void StopTimer() //to finish the timer (if you finish all levels)
    {
        isStopWatchRunning = false;
    }

    public void DestroyTimer() //to destroy this when exiting to the title screen from a level
    {
        Destroy(gameObject);
    }

    public float GetTimer() //used on the end screen (speedrun) script, where times are read in text fields and also saved in file
    {
        StopTimer();
        return time;
    }

    public bool IsInSpeedrunMode()
    {
        return isStopWatchRunning;
    }

    public void DisableSpeedrunMode()
    {
        stopWatchButton.enabled = false;
    }
}
