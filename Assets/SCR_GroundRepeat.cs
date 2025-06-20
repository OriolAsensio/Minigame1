using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_GroundRepeat : MonoBehaviour
{
    private float spriteWidth;

    void Start()
    {
        
        BoxCollider2D groundCollider = GetComponent<BoxCollider2D>();
        spriteWidth = groundCollider.size.x;

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -spriteWidth)
        {

            transform.Translate(Vector2.right * 1f * spriteWidth);
        
        }
    }
}
