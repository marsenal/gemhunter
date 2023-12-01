using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelSystem
{   

    public static List<int> levels = new List<int>();
    public static List<int> gems = new List<int>();
    /// <summary>
    /// Loading the data with SaveSystem and assigning the values to the lists of this class.
    /// </summary>
    public static void SetData()
    {
        LevelData data = SaveSystem.LoadGame();
        if (data == null) { return; }
        levels = data.levelData;
        gems = data.gemData;
        levels.Add(2);
        levels.Add(13); //these need to be added so level 1 on both worlds will always be accessible TODO: make level 1 of world 2 not automatically accessible (dependant on world 1 completion)
    }

    public static void EraseData() //clear the lists - clear all data (this is used in the Settings canvas with the save system's erase data)
    {
        levels.Clear();
        gems.Clear();
        levels.Add(2);
        levels.Add(13);
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

    public static List<int> GetLevelList()
    {
        return levels;
    }
    public static List<int> GetGemList()
    {
        return gems;
    }

}
