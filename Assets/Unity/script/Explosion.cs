using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] new private Collider2D collider;
    bool isWorked = false;

    void Update()
    {
        if (collider.enabled)
        {
            if (isWorked)
                collider.enabled = false;
            else
                isWorked = true;
        }
    }
}
