using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] float interval;
    float timer;
    void Start()
    {
        timer = interval;
    }

    
    void Update()
    {
        timer-= Time.deltaTime;
        if (timer < 0)
        {
            Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            timer = interval;
        }
    }
}
