using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldButton : MonoBehaviour
{
    Animator myAnimator;
    [SerializeField] private bool isWorldAccessible;

    [SerializeField][Tooltip("Set this to be equal to the world's first level's build index")] int levelIndex;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        if (!isWorldAccessible) isWorldAccessible = LevelSystem.IsOnTheLevelList(levelIndex); //this needs to be checked only in Start - checks wether the level is hardcoded accessible
        SetLevelAccessibilityOnButton();
    }
    public void SetLevelAccessibilityOnButton()
    {
        GetComponent<Image>().raycastTarget = isWorldAccessible;
        if (!isWorldAccessible)
        {
            myAnimator.SetTrigger("Disabled");
        }
        else myAnimator.SetTrigger("Normal");
    }
}
