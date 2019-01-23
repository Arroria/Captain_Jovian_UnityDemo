using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneFrameStayer : MonoBehaviour
{
    private bool isMaybeRendered = false;
    void Update()
    {
        if (isMaybeRendered)
            Destroy(gameObject);
        else
            isMaybeRendered = true;
    }
}
