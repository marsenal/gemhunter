using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Level Select scene.
/// </summary>
public class LevelSelect : MonoBehaviour
{
    [SerializeField] Button leftArrow;
    [SerializeField] Button rightArrow;
    [SerializeField] Canvas canvas1;
    [SerializeField] Canvas canvas2;
    [SerializeField] AudioManager audioManager;

    private int shownScreen = 1;

    float canvas1Height;
    float canvas1Width;
    float canvas2Height;
    float canvas2Width;
    void Start()
    {    
        canvas1Height = canvas1.GetComponent<RectTransform>().rect.height;
        canvas2Height = canvas2.GetComponent<RectTransform>().rect.height;

        canvas1Width = canvas1.GetComponent<RectTransform>().rect.width;

        canvas2Width = canvas2.GetComponent<RectTransform>().rect.width;

        canvas1.transform.position = transform.position;
        canvas2.transform.position = new Vector2 ( transform.position.x + canvas1Width * 1.5f, transform.position.y);
    }

    void Update()
    {
        if (shownScreen == 1)
        {
            leftArrow.interactable = false;
            rightArrow.interactable = true;
        }
        else
        {
            leftArrow.interactable = true;
            rightArrow.interactable = false;
        }
        SwipeCanvas(shownScreen);


    }

    public void ChangeToScreen(int screen) //call this when pressing arrows
    {
        switch (screen)
        {
            case 1:
                shownScreen = 1;
                break;
            case 2:
                shownScreen = 2;
                break;
        }
        AudioManager.instance.PlayClip("Menu");
       // audioManager.;
    }

    private void SwipeCanvas(int canvas)
    {
        if (canvas == 1)
        {
            canvas1.transform.position = Vector2.MoveTowards(canvas1.transform.position, transform.position, 4000f * Time.deltaTime);
            canvas2.transform.position = Vector2.MoveTowards(canvas2.transform.position, new Vector2(transform.position.x + canvas2Width * 1.5f, transform.position.y), 4000f*Time.deltaTime);
        }
        else if (canvas == 2)
        {
            canvas1.transform.position = Vector2.MoveTowards(canvas1.transform.position, new Vector2(transform.position.x - canvas1Width*1.5f, transform.position.y), 4000f * Time.deltaTime);
            canvas2.transform.position = Vector2.MoveTowards(canvas2.transform.position, transform.position, 4000f * Time.deltaTime);
        }
    }
}
