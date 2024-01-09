using System.Collections.Generic;
using System.IO;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using UnityEngine;
/// <summary>
/// Authenticate to the cloud. Methods for saving the level progress to cloud and loading it
/// </summary>
public class Authentication : MonoBehaviour
{

    private async void Awake()
    {
        int numberOfInstances = FindObjectsOfType<Authentication>().Length;
        if (numberOfInstances > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        await UnityServices.InitializeAsync(); //Authenticate initialization
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        LoadProgressFromCloud(); //when done, load progress > and save it locally ?
    }

    public async void SaveProgressToCloud() //save progress to the cloud in file named "save"
    {
        string path = Application.persistentDataPath + "/save.snld";
        byte[] file = File.ReadAllBytes(path);
        await CloudSaveService.Instance.Files.Player.SaveAsync("save", file);
    }
    public async void LoadProgressFromCloud() //create a stream from the "save" in the cloud 
    {                                         //deserialize it as LevelData >>>> Use that to set the Data          
        Stream file = await CloudSaveService.Instance.Files.Player.LoadStreamAsync("save");
        if (file == null)
        {
            return;
        }    
        else
        {
            LevelSystem.SetData(SaveSystem.LoadDataFromCloud(file));
        }
    }
}
