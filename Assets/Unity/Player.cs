using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Player : MonoBehaviour {

    public float velocity;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public GameObject cursor;

    public GameObject myWeapon;
    private Weapon myWeaponScript;

    [HideInInspector] public Vector2 myDirection;
    private LRFliper lrFliper;

    void Start ()
    {
        lrFliper = GetComponent<LRFliper>();

        myWeaponScript = myWeapon.GetComponent<Weapon>();
    }

    void Update()
    {
        Vector2 movement = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.A)) movement.x--;
        if (Input.GetKey(KeyCode.D)) movement.x++;
        if (Input.GetKey(KeyCode.W)) movement.y++;
        if (Input.GetKey(KeyCode.S)) movement.y--;
        movement *= velocity * Time.deltaTime;
        transform.position += new Vector3(movement.x, movement.y);

        animator.SetBool("isMoving", (movement.x != 0) || (0 != movement.y));


        UpdateDirection();


        if (Input.GetKey(KeyCode.Mouse0))
            myWeaponScript.WeaponFire();
    }

    private void UpdateDirection()
    {
        Vector2 curPos = cursor.transform.position;
        Vector2 myPos = transform.position;
        myDirection = (curPos - myPos);
        myDirection.Normalize();
        lrFliper.In(myDirection);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }

    public void OnCollisionEnter2DByPhysicalCollider(Collision2D collision)
    {
    }
}
