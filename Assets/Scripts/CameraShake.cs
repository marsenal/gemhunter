using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float shakeForce = 1f;
    Vector3 initialPos;
    void Start()
    {
        initialPos = transform.position;
    }

    void Update()
    {
        Shake();
    }

    private void Shake()
    {
        transform.position = transform.position + (Vector3)Random.insideUnitCircle * shakeForce;
    }
}
