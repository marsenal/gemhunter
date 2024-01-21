using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowHelper : MonoBehaviour
{
    // Start is called before the first frame update

    SpriteMask spriteMask;
    SpriteRenderer parenteSpriteRenderer;
    void Start()
    {
        spriteMask = GetComponent<SpriteMask>();
        parenteSpriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteMask.sprite = parenteSpriteRenderer.sprite;
    }
}
