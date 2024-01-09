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

    private void Awake()
    {
        if (LevelSystem.IsEveryLevelCompleted())
        {
            stopWatchButton.enabled = true;
            int numberOfTriggers = FindObjectsOfType<BossTriggerSecondWorld>().Length;
            if (numberOfTriggers > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else stopWatchButton.enabled = false;
    }
    void Start()
    {
          
    }

    void Update()
    {
        if (isStopWatchRunning)
        {
            stopWatchText.enabled = true;
            time += Time.deltaTime;
            stopWatchText.text = TimeSpan.FromSeconds(time).ToString(@"m\:ss");
        }
        else
        {
            stopWatchText.enabled = false;
        }
    }

    public void StarTimer()
    {
        time = 0f;
        isStopWatchRunning = true;
    }

    public void DestroyTimer()
    {
        Destroy(gameObject);
    }

    public float GetTimer()
    {
        return time;
    }
}
