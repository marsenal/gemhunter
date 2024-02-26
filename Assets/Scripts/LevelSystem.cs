using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System;

public static class LevelSystem
{   


    public static List<int> levels = new List<int>();
    public static List<int> gems = new List<int>();

    public static int deaths; //total number of deaths

    public static float bestTime;
    public static float timeWorld1;
    public static float timeWorld2;
    public static float timeWorld3;

    public static int setLanguageID = -1;

    public static bool areAdsRemoved = false;

    public static bool isLoginSkipped;

    private const int NUMBER_OF_LEVELS = 30; //needed to check game completion
    private const int NUMBER_OF_GEMS = 30; //needed to check game 100% completion


    /// <summary>
    /// Loading the data with SaveSystem and assigning the values to the lists of this class.
    /// </summary>
    public static void SetData(LevelData data) //set data from cloud
    {
        if (data == null) { return; }
        levels = data.levelData;
        gems = data.gemData;

        deaths = data.deathData;
        bestTime = data.bestTime;
        timeWorld1 = data.timeWorld1;
        timeWorld2 = data.timeWorld2;
        timeWorld3 = data.timeWorld3;

        setLanguageID = data.setLanguageID;

        areAdsRemoved = data.areAdsRemoved;
    }

    public static void SetDataLocally() //set data from local device
    {        
        LevelData data = SaveSystem.LoadGame();
        if (data == null) { return; }
        levels = data.levelData;
        gems = data.gemData;

        deaths = data.deathData;
        bestTime = data.bestTime;

        setLanguageID = data.setLanguageID;

        areAdsRemoved = data.areAdsRemoved;

        isLoginSkipped = data.isLoginSkipped;
    }

    public static void EraseData() //clear the lists - clear all data (this is used in the Settings canvas with the save system's erase data)
    {
        levels.Clear();
        gems.Clear();
    }
    public static void AddToLevelList(int level) //The level is added to the list of levels - usually when entering the end portal
    {
        /*if (level == 13) Social.ReportProgress("CgkI967U96ofEAIQDw", 100.0f, (bool success) => { //use this for debugging 
                                                                                                 /
        });*/
        if (!levels.Contains(level))
        {
            levels.Add(level);
        }
    }
    /// <summary>
    /// Checks wether the level is on the list of leveldata
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static bool IsOnTheLevelList(int level)
    {
        return levels.Contains(level);
    }

    public static void AddToGemsList(int level) //The gem(indexed same as level) is added to the list of levels - usually when entering the end portal
    {
        if (!gems.Contains(level))
        {
            gems.Add(level);
            if (gems.Count == 1)
            {
                Social.ReportProgress("CgkI967U96ofEAIQAg", 100.0f, (bool success) => { //Find a Gem achievement handling
                    // handle success or failure
                });
            }
            else if (gems.Count == 10)
            {
                Social.ReportProgress("CgkI967U96ofEAIQDw", 100.0f, (bool success) => { //Find a Gem achievement handling
                    // handle success or failure
                    //CgkI967U96ofEAIQBA
                });
            }
            else if (gems.Count == 20)
            {
                Social.ReportProgress("CgkI967U96ofEAIQBQ", 100.0f, (bool success) => { //Find a Gem achievement handling
                    // handle success or failure
                });
            }
            else if (gems.Count == 30)
            {
                Social.ReportProgress("CgkI967U96ofEAIQBg", 100.0f, (bool success) => { //Find a Gem achievement handling
                    // handle success or failure
                });
            }
        }
    }
    public static bool IsOnTheGemList(int level) //checks wether the given level-indexed gem is on the list of gems
    {
        return gems.Contains(level);
    }

    public static void IncreaseDeathCounter()
    {
        deaths++;
    }

    public static void SetBestTime(float value)
    {
        bestTime = value;
    }

    public static List<int> GetLevelList()
    {
        return levels;
    }
    public static List<int> GetGemList()
    {
        return gems;
    }

    public static int GetDeathCount() //returns the number of deaths total. Used in the high score canvas in the world select screen
    {
        return deaths;
    }

    public static float GetBestTime()
    {
        return bestTime;
    }

    public static bool IsEveryLevelCompleted() //return true if all levels are completed (on the levels list)
    {
        if (levels.Count >= NUMBER_OF_LEVELS)
        {
            return true;
        }
        else return false;
    }

    public static bool IsEveryGemCollected()
    {
        if (gems.Count >= NUMBER_OF_GEMS)
        {
            return true;
        }
        else return false;
    }

}
