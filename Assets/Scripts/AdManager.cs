using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidGameID;

    [SerializeField] private bool testMode = true;

    [SerializeField] private string androidAdUnitID;

    public static AdManager instance;

    private string gameID;

    private string adUnitID;

    private bool canPlay;
    public int numberOfDeaths;
    [SerializeField] private int deathNumbersToTriggerAd;

    private void Awake()
    {
        if (instance != null && instance != this)
        { Destroy(gameObject); }
        else
        {
            InitializeAds();

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void InitializeAds()
    {
        gameID = androidGameID;
        adUnitID = androidAdUnitID;
        if (!Advertisement.isInitialized)
        {
            Advertisement.Initialize(gameID, testMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Ads initizalization complete");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Ads initizalization failed");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Advertisement.Show(placementId, this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Ad showing failed to load");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("Ad showing failed to show: " + error.ToString() + message);
    }

    public void OnUnityAdsShowStart(string placementId)
    {

    }

    public void OnUnityAdsShowClick(string placementId)
    {

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(adUnitID) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)) //only if completed, if skipped dont do this
                                                                                                               //also, placementID check is needed if there are different type of ads setup
        {
            numberOfDeaths = 0;
            Debug.Log("Ad showing is completed");
        }
    }

    public void ShowAd()
    {

    }
    public void IncreaseDeathNumbers()
    {
        numberOfDeaths++;
        if (numberOfDeaths >= deathNumbersToTriggerAd)
        {
            Advertisement.Load(adUnitID, this);
        }
    }
}
