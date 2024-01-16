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

    public LevelData ()
    {
        levelData = LevelSystem.GetLevelList();
        gemData = LevelSystem.GetGemList();
        deathData = LevelSystem.GetDeathCount();
        bestTime = LevelSystem.GetBestTime();
    }
}
