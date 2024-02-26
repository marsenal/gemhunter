using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoogleLoginButton : MonoBehaviour
{
    [SerializeField] Sprite loginSprite;
    [SerializeField] Sprite logoutSprite;
    Image myImage;
    void Start()
    {
        myImage = GetComponent<Image>();
        UpdateIcon();
    }

    public void UpdateIcon()
    {
        if (FindObjectOfType<Authentication>().IsAuthenticatedAndLoggedIn())
        {
            myImage.sprite = logoutSprite;
        }
        else myImage.sprite = loginSprite;
    }

public void SendScoreToLeaderboard()
    {
        Social.ReportScore(10300, "CgkI967U96ofEAIQCw", (bool success) =>
        { //post best (current) time to leaderboard
            if (success == true)
            {
                Debug.Log("New best time posted to leaderboard of world 1");
            }
            else { Debug.Log("New best time failed to post to leaderboard of world 1"); }
        });
    }
}
