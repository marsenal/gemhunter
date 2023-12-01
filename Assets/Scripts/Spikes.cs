using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    Animator myAnimator;
    [SerializeField] float animationExitTime;
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }
}
