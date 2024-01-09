using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
/// <summary>
/// Static class for saving and loading game data. 
/// </summary>
public static class SaveSystem
{
    public static void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/save.snld";

        FileStream stream = new FileStream(path, FileMode.Create);

        LevelData data = new LevelData();

        formatter.Serialize(stream, data);
        stream.Close();        
    }

    public static LevelData LoadGame()
    {
        string path = Application.persistentDataPath + "/save.snld";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelData data = formatter.Deserialize(stream) as LevelData;
            stream.Close();

            return data;
        } else
        {
            Debug.LogWarning("Save file not found in " + path);
            return null;
        }
    }

    public static void EraseData() //write an empty file into the save file - this is used in conjuction with the levelsystem's erase data
    {   
            BinaryFormatter formatter = new BinaryFormatter();

            string path = Application.persistentDataPath + "/save.snld";

            FileStream stream = new FileStream(path, FileMode.Create);

            LevelData data = new LevelData();

            formatter.Serialize(stream, data);
            stream.Close();
        
    }

    public static LevelData LoadDataFromCloud(Stream stream)
    {
       // string path = Application.persistentDataPath + "/save.snld";

       // if (File.Exists(path))
       // {
            BinaryFormatter formatter = new BinaryFormatter();
           // FileStream stream = new FileStream(path, FileMode.Open);

            LevelData data = formatter.Deserialize(stream) as LevelData;
            stream.Close();

            return data;
      //  }
      //  else
      //  {
       //     Debug.LogWarning("Save file not found in " + path);
      //      return null;
      //  }
    }

}
