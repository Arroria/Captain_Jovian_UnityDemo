using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [HideInInspector] public Vector2 direction;
    public float velocity;
    private SpriteRenderer spriteRenderer;

    void Start ()
    {
        //set flip
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = direction.x < 0;


        float angle = Vector2.Angle(direction, new Vector2(0, -1)) - 90;
        if (spriteRenderer.flipX)
            angle *= -1;
        Quaternion quat = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        transform.rotation = quat;
    }
	
	void Update ()
    {
        Vector3 movement = direction * velocity * Time.deltaTime;
        transform.position += movement;
	}
}
