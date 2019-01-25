using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRFliper : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spriteRenderer;

    public virtual void In(Vector2 dir)
    {
        spriteRenderer.flipX = dir.x < 0;
    }
}
