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
        if (!isLevelAccessible) isLevelAccessible = LevelSystem.IsOnTheLevelList(levelIndex - 1); //this needs to be checked only in Start
    }

    private void CheckLevelCompleted()
    {
        checkMark.enabled = isLevelCompleted;
    }
    private void CheckGemCollected()
    {
        gem.enabled = isGemCollected;
    }
    private void CheckLevelAccessible()
    {
       GetComponent<Button>().interactable = isLevelAccessible;        
    }

    public void RefreshData()
    {
        isGemCollected = LevelSystem.IsOnTheGemList(levelIndex);
        isLevelCompleted = LevelSystem.IsOnTheLevelList(levelIndex);
        isLevelAccessible = LevelSystem.IsOnTheLevelList(levelIndex - 1); //this is not checked here, because 
        CheckLevelAccessible();
        CheckLevelCompleted();
        CheckGemCollected();
    }
}
