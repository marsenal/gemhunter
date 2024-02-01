using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class Leaderboard : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ShowLeaderboard()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkI967U96ofEAIQAw");
    }
}
