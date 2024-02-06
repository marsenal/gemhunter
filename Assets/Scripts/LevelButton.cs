using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField] int levelIndex;
    [SerializeField] Image gem;
    [SerializeField] Image checkMark;
    [SerializeField] private bool isLevelCompleted;
    [SerializeField] private bool isLevelAccessible;
    private bool isGemCollected;
    void Start()
    {
        RefreshData();
        if (!isLevelAccessible) isLevelAccessible = LevelSystem.IsOnTheLevelList(levelIndex - 1); //this needs to be checked only in Start - checks wether the level is hardcoded accessible      
    }

    private void SetLevelCompletionOnButton()
    {
        checkMark.enabled = isLevelCompleted;
    }
    private void SetGemCollectionOnButton()
    {
        gem.enabled = isGemCollected;
    }
    private void SetLevelAccessibilityOnButton()
    {
       GetComponent<Button>().interactable = isLevelAccessible;        
    }

    public void RefreshData()
    {
        isGemCollected = LevelSystem.IsOnTheGemList(levelIndex);
        isLevelCompleted = LevelSystem.IsOnTheLevelList(levelIndex);
        if (!isLevelAccessible) isLevelAccessible = LevelSystem.IsOnTheLevelList(levelIndex - 1); 
        SetLevelAccessibilityOnButton();
        SetLevelCompletionOnButton();
        SetGemCollectionOnButton();
    }
}
