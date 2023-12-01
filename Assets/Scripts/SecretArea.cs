using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
/// <summary>
/// Behaviour of secret area tilemap, change alpha to 0.2 while in the area.
/// </summary>
public class SecretArea : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<Tilemap>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
