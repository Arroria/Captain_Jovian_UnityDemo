using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursor : MonoBehaviour {
    

	void Start ()
    {
        Update();
	}
	
	void Update ()
    {
        Vector3 mPos = Input.mousePosition;
        mPos.z = 0;
        transform.position = Camera.main.ScreenToWorldPoint(mPos);
	}
}
