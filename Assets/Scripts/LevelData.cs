using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serializable data container with a constructor that creates two lists for the data to be contain in.
/// </summary>
[System.Serializable]
public class LevelData
{
    public List<int> levelData;
    public List<int> gemData;
    public int deathData;
    public float bestTime;
    public int setLanguageID;

    public float timeWorld1;
    public float timeWorld2;
    public float timeWorld3;

    public LevelData ()
    {
        levelData = LevelSystem.GetLevelList();
        gemData = LevelSystem.GetGemList();
        deathData = LevelSystem.GetDeathCount();
        bestTime = LevelSystem.GetBestTime();
        setLanguageID = LevelSystem.setLanguageID;
        timeWorld1 = LevelSystem.timeWorld1;
        timeWorld2 = LevelSystem.timeWorld2;
        timeWorld3 = LevelSystem.timeWorld3;
    }
}
