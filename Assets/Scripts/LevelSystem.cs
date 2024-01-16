using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelSystem
{   

    public static List<int> levels = new List<int>();
    public static List<int> gems = new List<int>();

    public static int deaths; //total number of deaths

    public static float bestTime;

    private const int NUMBER_OF_LEVELS = 30; //needed to check game completion
    private const int NUMBER_OF_GEMS = 30; //needed to check game 100% completion
    /// <summary>
    /// Loading the data with SaveSystem and assigning the values to the lists of this class.
    /// </summary>
    public static void SetData(LevelData data) //set data from cloud
    {
        //LevelData data = SaveSystem.LoadGame();
        if (data == null) { return; }
        levels = data.levelData;
        gems = data.gemData;
        /*levels.Add(2);
        levels.Add(13); //these need to be added so level 1 on both worlds will always be accessible TODO: make level 1 of world 2 not automatically accessible (dependant on world 1 completion)
        levels.Add(25);*/
    }

    public static void SetDataLocally() //set data from local device
    {        
        LevelData data = SaveSystem.LoadGame();
        if (data == null) { return; }
        levels = data.levelData;
        gems = data.gemData;

        deaths = data.deathData;
    }

    public static void EraseData() //clear the lists - clear all data (this is used in the Settings canvas with the save system's erase data)
    {
        levels.Clear();
        gems.Clear();
        //deaths = 0;
       /* levels.Add(2);
        levels.Add(13);
        levels.Add(25);*/
    }
    public static void AddToLevelList(int level) //The level is added to the list of levels - usually when entering the end portal
    {
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
