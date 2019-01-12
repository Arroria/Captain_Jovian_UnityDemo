using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    public static class Vector2ex
    {
        public static Vector2 By3(Vector3 vec3) { return new Vector2(vec3.x, vec3.y); }
        public static Vector3 To3(Vector2 vec2) { return new Vector3(vec2.x, vec2.y, 0); }

        public static Vector2 Rotate(Vector2 origin, float radianAngle)
        {
            float _x = origin.x;
            float _y = origin.y;

            float _cos = Mathf.Cos(radianAngle);
            float _sin = Mathf.Sin(radianAngle);

            float _x2 = _x * _cos - _y * _sin;
            float _y2 = _x * _sin + _y * _cos;

            return new Vector2(_x2, _y2);
        }
    }

public static class TimeEx
{
    public static float Cooldown(ref float time)
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = 0;
            return time;
        }
        return time;
    }
}
