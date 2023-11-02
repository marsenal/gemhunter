using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewAbility : MonoBehaviour
{
    [SerializeField] Button okButton;
    Canvas myCanvas;
    void Start()
    {
        myCanvas = GetComponent<Canvas>();
    }

    public void DisableCanvas(bool value)
    {
        myCanvas.enabled = value;
    }
}
