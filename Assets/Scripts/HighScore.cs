using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
/// <summary>
/// Manage best time and death count texts.
/// </summary>
public class HighScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bestTimeText;
    [SerializeField] TextMeshProUGUI deathCountText;
    [SerializeField] TextMeshProUGUI gemCountText;
    void Start()
    {
        deathCountText.text = LevelSystem.GetDeathCount().ToString();
        bestTimeText.text = TimeSpan.FromSeconds(LevelSystem.GetBestTime()).ToString(@"m\:ss");
        Debug.Log(TimeSpan.FromSeconds(LevelSystem.GetBestTime()).ToString(@"m\:ss"));
        gemCountText.text = LevelSystem.GetGemList().Count.ToString() + "/30";
    }

    public void RefreshData()
    {
        deathCountText.text = LevelSystem.GetDeathCount().ToString();
        gemCountText.text = LevelSystem.GetGemList().Count.ToString();
    }
}