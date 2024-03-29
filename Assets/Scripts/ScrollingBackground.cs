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
         myMaterial = GetComponent<Renderer>().material; //for always scrolling bckgrnd
         offset = new Vector2(scrollSpeedX, 0f);
     }

     void Update()
     {

     }

     private void FixedUpdate()
     {
         if (isAlwaysScrolling && myMaterial != null)
         {
             myMaterial.mainTextureOffset += offset * Time.deltaTime; //for always scrolling bckgrnd
         }
         else
         {
             transform.position = new Vector2(startPosX + mainCamera.transform.position.x * scrollSpeedX, startPosY + mainCamera.transform.position.y * scrollSpeedY);
         }
     }

    /*[SerializeField] Vector2 scrollSpeed;

    Transform cameraTransform;

    private Vector3 lastCameraPosition;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3( deltaMovement.x * scrollSpeed.x, deltaMovement.y * scrollSpeed.y);
        lastCameraPosition = cameraTransform.position;
    }*/
}
