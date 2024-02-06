using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class Leaderboard : MonoBehaviour
{
    public void ShowLeaderboard(string leaderBoardID)
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderBoardID);
    }
}
