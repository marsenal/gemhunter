using System.Collections.Generic;
using System.Collections;
using System.IO;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using System;
using System.Threading.Tasks;
/// <summary>
/// Authenticate to the cloud. Methods for saving the level progress to cloud and loading it
/// </summary>
public class Authentication : MonoBehaviour
{

    public string Token;
    public string Error;

    public bool isLoggedIn = false;

    
    void Awake()
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
        PlayGamesPlatform.Activate(); 
    }

    async void Start()
    {
        await UnityServices.InitializeAsync();

        await LoginGooglePlayGames();
        await SignInWithGooglePlayGamesAsync(Token);
        //await LinkWithGooglePlayGamesAsync(Token);
        OpenSavedGame(false);

    }
    //Fetch the Token / Auth code
    public Task LoginGooglePlayGames()
    {
        var tcs = new TaskCompletionSource<object>();
        PlayGamesPlatform.Instance.ManuallyAuthenticate(success =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login with Google Play games successful.");
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    Debug.Log("Authorization code: " + code);
                    Token = code;
                    // This token serves as an example to be used for SignInWithGooglePlayGames

                    tcs.SetResult(null);
                });
            }
            else
            {
                Error = "Failed to retrieve Google play games authorization code";
                Debug.Log("Login Unsuccessful" + Error);
                tcs.SetException(new Exception("Failed"));
                LevelSystem.SetDataLocally();
            }
        });
        return tcs.Task;
    }



    async Task SignInWithGooglePlayGamesAsync(string authCode)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);
            isLoggedIn = true;
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}"); //Display the Unity Authentication PlayerID
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
    async Task LinkWithGooglePlayGamesAsync(string authCode) //promote anonymous to registered
    {
        try
        {
            await AuthenticationService.Instance.LinkWithGooglePlayGamesAsync(authCode);
            Debug.Log("Link is successful.");
        }
        catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
        {
            // Prompt the player with an error message.
            Debug.LogError("This user is already linked with another account. Log in instead.");
        }

        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    async Task UnlinkGooglePlayGamesAsync(string idToken)
    {
        try
        {
            await AuthenticationService.Instance.UnlinkGooglePlayGamesAsync();
            isLoggedIn = false;
            Debug.Log("Unlink is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    private bool isSaving;

    /// <summary>
    /// Open a saved game from cloud - parameter determines wether to save it or load it.
    /// </summary>
    /// <param name="saving"></param>
    public void OpenSavedGame(bool saving)
    {
        if (Social.localUser.authenticated)
        {
            isSaving = saving;
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.OpenWithAutomaticConflictResolution("save.snld", DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
        }
    }

    public void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata meta) //callback method on opening save from cloud - if true, successful, else failed
    {
        if (status == SavedGameRequestStatus.Success)
        {
            if (isSaving) //saving to the cloud
            {
                string path = Application.persistentDataPath + "/save.snld";
                byte[] binaryData = File.ReadAllBytes(path);

                SavedGameMetadataUpdate updateForMetadata = new SavedGameMetadataUpdate.Builder().WithUpdatedDescription("Metadata was updated at: " + System.DateTime.Now.ToString()).Build();

                PlayGamesPlatform.Instance.SavedGame.CommitUpdate(meta, updateForMetadata, binaryData, SaveCallback);

                Debug.Log("Save file opened from cloud for saving");
            }
            else //loading from the cloud
            {
                PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(meta, LoadCallBack);
                Debug.Log("Save file opened from cloud for loading");
            }
        }
        else
        {
            Debug.Log("Opening save file failed \n Opening Save File from locally");

            LevelSystem.SetDataLocally();

        }
    }

    private void LoadCallBack(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Stream file = new MemoryStream(data);
            LevelSystem.SetData(SaveSystem.LoadDataFromCloud(file));
            Debug.Log("Successfully loaded data from cloud");
        }
    }

    private void SaveCallback(SavedGameRequestStatus status, ISavedGameMetadata meta) //callback method with response wether save was successful
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Game saved successfully to the cloud");
        }
        else
        {
            Debug.Log("Game save failed");
        }
    }

    public async Task LoginOrLogout()
    {
        Debug.Log("Logging in or logging out");
       // if (!isLoggedIn)
       // {
            PlayGamesPlatform.Instance.ManuallyAuthenticate(success =>
            {
                if (success == SignInStatus.Success)
                {
                    Debug.Log("Login manually with Google Play games successful.");
                    PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                    {
                        Debug.Log("Authorization code: " + code);
                        Token = code;
                    });
                    LevelSystem.isLoginSkipped = false;
                    SaveSystem.SaveGame();
                    OpenSavedGame(false);
                }
                else
                {
                    Debug.Log("Login manually failed");
                }
            });
            await SignInWithGooglePlayGamesAsync(Token);
       // }
       // else
      //  {

                await UnlinkGooglePlayGamesAsync(Token);
                AuthenticationService.Instance.SignOut(true);
            //isLoggedIn = false;
            LevelSystem.isLoginSkipped = true;
                SaveSystem.SaveGame();
      //  }
        FindObjectOfType<GoogleLoginButton>().UpdateIcon();
    }

    public async Task ManuallyAuthenticate()
    {
        Debug.Log("Starting Authentication");
        PlayGamesPlatform.Instance.ManuallyAuthenticate(success =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login manually with Google Play games successful.");
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    Debug.Log("Authorization code: " + code);
                    Token = code;
                }); 
                //LevelSystem.isLoginSkipped = false;
                //SaveSystem.SaveGame();
                OpenSavedGame(false);
            }
            else
            {
                Debug.Log("Login manually failed");
            }
        });
        await SignInWithGooglePlayGamesAsync(Token);

        FindObjectOfType<GoogleLoginButton>().UpdateIcon();
    }

    public async void UnlinkAccount()
    {
        Debug.Log("Starting Unlinking");
        await UnlinkGooglePlayGamesAsync(Token);
        AuthenticationService.Instance.SignOut(true);
        AuthenticationService.Instance.ClearSessionToken();
        Debug.Log("User ID: " + AuthenticationService.Instance.GetPlayerInfoAsync().Result.GetGooglePlayGamesId());
        // LevelSystem.isLoginSkipped = true;
        // SaveSystem.SaveGame();

        FindObjectOfType<GoogleLoginButton>().UpdateIcon();
    }

    public bool IsAuthenticatedAndLoggedIn()
    {
        return isLoggedIn;
    }
}
