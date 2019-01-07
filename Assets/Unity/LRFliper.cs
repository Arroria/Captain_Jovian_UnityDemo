using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRFliper : MonoBehaviour {
    public bool isUseRotation;
    private SpriteRenderer spriteRenderer;

    [HideInInspector] public float angle;

    public void In(Vector2 dir)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = dir.x < 0;

        angle = Vector2.Angle(dir, new Vector2(0, -1));
        if (dir.x < 0)
            angle = 180 - angle;
        angle -= 90;

        if (isUseRotation)
            transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
    }
}
