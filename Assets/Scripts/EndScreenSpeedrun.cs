using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EndScreenSpeedrun : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentTimeText;
    [SerializeField] TextMeshProUGUI bestTimeText;
    [SerializeField] Canvas endGameCanvas;
    [SerializeField] Canvas speedrunCanvas;
    void Start()
    {
        if (FindObjectOfType<StopWatch>() != null)
        {
            StopWatch stopWatch = FindObjectOfType<StopWatch>();
            endGameCanvas.enabled = false;
            speedrunCanvas.enabled = true;
            float currentTime = stopWatch.GetTimer();
            float bestTime = LevelSystem.GetBestTime();
            currentTimeText.text = TimeSpan.FromSeconds(currentTime).ToString(@"m\:ss");
            if (currentTime <= bestTime || bestTime==0f)
            {
                bestTimeText.text = TimeSpan.FromSeconds(currentTime).ToString(@"m\:ss");
                LevelSystem.SetBestTime(currentTime);
                SaveSystem.SaveGame();
            }
            else if(bestTime != 0f)
            {
                bestTimeText.text = TimeSpan.FromSeconds(bestTime).ToString(@"m\:ss");
            }

        }
        else
        {
            endGameCanvas.enabled = true;
            speedrunCanvas.enabled = false;
        }


    }
}
