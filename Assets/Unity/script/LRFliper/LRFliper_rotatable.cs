using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRFliper_rotatable : LRFliper
{
    [SerializeField] public bool isRotatable;
    [HideInInspector] public float angle;

    public override void In(Vector2 dir)
    {
        base.In(dir);
        if (!isRotatable) return;

        angle = Vector2.Angle(dir, new Vector2(0, -1));
        if (dir.x < 0)
            angle = 180 - angle;
        angle -= 90;

        transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
    }
}
