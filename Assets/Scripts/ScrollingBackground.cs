using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    float startPosX;
    float startPosY;
    [SerializeField] float scrollSpeedX;
    [SerializeField] float scrollSpeedY;
    [SerializeField][Tooltip("If not enabled, only scrolls if the camera moves.")] bool isAlwaysScrolling;
    Material myMaterial;
    Vector2 offset;

    Camera mainCamera;
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        mainCamera = Camera.main;
        myMaterial = GetComponent<Renderer>().material;
        offset = new Vector2(scrollSpeedX, 0f);
    }

    void Update()
    {
        if (isAlwaysScrolling && myMaterial != null)
        {
            myMaterial.mainTextureOffset += offset * Time.deltaTime;
        }
        else
        {
            transform.position = new Vector2(startPosX + mainCamera.transform.position.x * scrollSpeedX, startPosY + mainCamera.transform.position.y * scrollSpeedY);
        }
    }
}
