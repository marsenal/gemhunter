using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings Data", fileName = "SettingsData")]

public class SettingsData : ScriptableObject
{
    public bool isMusicEnabled;
    public bool isSoundEnabled;
    public float musicVolume;
}

