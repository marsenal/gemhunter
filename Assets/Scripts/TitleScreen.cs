using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buttonToBlink;
    [SerializeField] float blinkTime;

    float timer;
    bool isShowing;
    void Start()
    {
        timer = blinkTime;
        LevelSystem.SetDataLocally();
    }

    void Update() //set timer to the blink time and make the text disappear if the timer reaches zero
    {
        timer = timer - Time.deltaTime;
        if (timer < 0 )
        {
            isShowing = !isShowing;
            timer = blinkTime;
        }
        ButtonBlink(isShowing);
    }
    private void ButtonBlink(bool value)
    {
        if (buttonToBlink == null) { return; }
        buttonToBlink.enabled = value;
    }

    public void PlaySound(string soundName)
    {
        AudioManager.instance.PlayClip(soundName);
    }
}
