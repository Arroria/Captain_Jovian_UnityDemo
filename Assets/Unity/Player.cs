using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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



        myDirection = (cursor.transform.position - transform.position).normalized;
        lrFliper.In(myDirection);



        if (Input.GetKey(KeyCode.Mouse0))
            myWeaponScript.WeaponFire();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("CollT");
    }

    public void OnCollisionEnter2DByPhysicalCollider(Collision2D collision)
    {
        Debug.Log("CollC");
    }
}
