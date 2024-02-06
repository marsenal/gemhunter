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
        if (FindObjectOfType<StopWatch>() != null) //if this was a speedrun
        {
            endGameCanvas.enabled = false;
            speedrunCanvas.enabled = true;
            if (!FindObjectOfType<StopWatch>().IsThisAPartialRun()) //if this was a full speedrun
            {
                StopWatch stopWatch = FindObjectOfType<StopWatch>();
                float currentTime = stopWatch.GetTimer();
                float bestTime = LevelSystem.GetBestTime();
                currentTimeText.text = TimeSpan.FromSeconds(currentTime).ToString(@"m\:ss");
                if (currentTime <= bestTime || bestTime == 0f)
                {
                    bestTimeText.text = TimeSpan.FromSeconds(currentTime).ToString(@"m\:ss");
                    LevelSystem.SetBestTime(currentTime);
                    SaveSystem.SaveGame();  //save locally
                    FindObjectOfType<Authentication>().OpenSavedGame(true); //save to cloud
                    Social.ReportScore((long)currentTime, "CgkI967U96ofEAIQAw", (bool success) =>
                    { //post best (current) time to leaderboard
                        if (success == true)
                        {
                            Debug.Log("New best time posted to leaderboard");
                        }
                        else { Debug.Log("New best time failed to post to leaderboard"); }
                    });
                }
                else if (bestTime != 0f)
                {
                    bestTimeText.text = TimeSpan.FromSeconds(bestTime).ToString(@"m\:ss");
                }
            }
            else //if this was a partial (one world) speedrun
            {
                StopWatch stopWatch = FindObjectOfType<StopWatch>();
                float currentTime = stopWatch.GetTimer();
                currentTimeText.text = TimeSpan.FromSeconds(currentTime).ToString(@"m\:ss");
                float bestTime = new float();
                switch (stopWatch.world)
                {
                    case 1:
                        bestTime = LevelSystem.timeWorld1;
                        if (currentTime <= bestTime || bestTime == 0f)
                        {
                            bestTimeText.text = TimeSpan.FromSeconds(currentTime).ToString(@"m\:ss");
                            LevelSystem.timeWorld1 = currentTime;
                            SaveSystem.SaveGame();  //save locally
                            FindObjectOfType<Authentication>().OpenSavedGame(true); //save to cloud
                            Social.ReportScore((long)currentTime, "CgkI967U96ofEAIQCw", (bool success) =>
                            { //post best (current) time to leaderboard
                                if (success == true)
                                {
                                    Debug.Log("New best time posted to leaderboard");
                                }
                                else { Debug.Log("New best time failed to post to leaderboard"); }
                            });
                        }
                        break;
                    case 2:
                        bestTime = LevelSystem.timeWorld2;
                        if (currentTime <= bestTime || bestTime == 0f)
                        {
                            bestTimeText.text = TimeSpan.FromSeconds(currentTime).ToString(@"m\:ss");
                            LevelSystem.timeWorld2 = currentTime;
                            SaveSystem.SaveGame();  //save locally
                            FindObjectOfType<Authentication>().OpenSavedGame(true); //save to cloud
                            Social.ReportScore((long)currentTime, "CgkI967U96ofEAIQDA", (bool success) =>
                            { //post best (current) time to leaderboard
                                if (success == true)
                                {
                                    Debug.Log("New best time posted to leaderboard");
                                }
                                else { Debug.Log("New best time failed to post to leaderboard"); }
                            });
                        }
                        break;
                    case 3:
                        bestTime = LevelSystem.timeWorld3;
                        if (currentTime <= bestTime || bestTime == 0f)
                        {
                            bestTimeText.text = TimeSpan.FromSeconds(currentTime).ToString(@"m\:ss");
                            LevelSystem.timeWorld3 = currentTime;
                            SaveSystem.SaveGame();  //save locally
                            FindObjectOfType<Authentication>().OpenSavedGame(true); //save to cloud
                            Social.ReportScore((long)currentTime, "CgkI967U96ofEAIQDQ", (bool success) =>
                            { //post best (current) time to leaderboard
                                if (success == true)
                                {
                                    Debug.Log("New best time posted to leaderboard");
                                }
                                else { Debug.Log("New best time failed to post to leaderboard"); }
                            });
                        }
                        break;
                }


            }
        }
        else
        {
            endGameCanvas.enabled = true;
            speedrunCanvas.enabled = false;
        }


    }
}
