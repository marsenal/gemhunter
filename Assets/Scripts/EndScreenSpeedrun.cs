using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

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
                SaveSystem.SaveGame();  //save locally
                FindObjectOfType<Authentication>().OpenSavedGame(true); //save to cloud
                Social.ReportScore((long)currentTime, "CgkI967U96ofEAIQAw", (bool success) => { //post best (current) time to leaderboard
                    if (success == true)
                    {
                        Debug.Log("New best time posted to leaderboard");
                    }
                    else { Debug.Log("New best time failed to post to leaderboard"); }
                });
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
