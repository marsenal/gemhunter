using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class Store : MonoBehaviour
{
    public void OnPurchaseComplete(Product product)
    {
        LevelSystem.areAdsRemoved = true;
        SaveSystem.SaveGame();
        if (FindObjectOfType<Authentication>()) FindObjectOfType<Authentication>().OpenSavedGame(true);
        if (FindObjectOfType<AdManager>()) FindObjectOfType<AdManager>().AdsRemovedDestroyThis();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription reason)
    {
        Debug.Log("Purchase failed");
    }
}
