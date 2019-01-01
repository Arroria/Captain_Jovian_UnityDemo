using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public float velocity;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public GameObject cursor;

	void Start ()
    {
        cursor = GameObject.Find("Cursor");
	}
	
	void Update ()
    {
        Vector2 movement = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.A))    movement.x--;
        if (Input.GetKey(KeyCode.D))    movement.x++;
        if (Input.GetKey(KeyCode.W))    movement.y++;
        if (Input.GetKey(KeyCode.S))    movement.y--;
        movement *= velocity * Time.deltaTime;
        transform.position += new Vector3(movement.x , movement.y);

        bool lookAtRight = transform.position.x <= cursor.transform.position.x;
        spriteRenderer.flipX = !lookAtRight;

        animator.SetBool("isMoving", (movement.x != 0) || (0 != movement.y));
    }
}
