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


    [SerializeField] private int health;
    [SerializeField] private float hitCooldown_unit;
    private float hitCooldown;
    [SerializeField] private float hitEffect_unit;


    [HideInInspector] public Vector2 myDirection;
    private LRFliper lrFliper;

    void Start ()
    {
        lrFliper = GetComponent<LRFliper>();

        myWeaponScript = myWeapon.GetComponent<Weapon>();
    }

    void Update()
    {
        if (health == 0)
            return;

        if (hitCooldown > 0)
        {
            bool enable;
            if (TimeEx.Cooldown(ref hitCooldown) == 0)
                enable = true;
            else
                enable = ((int)((hitCooldown_unit - hitCooldown) / hitEffect_unit) & 1) != 0;

            spriteRenderer.enabled = enable;
            //자식 렌더러는 어케끄냐
        }


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



    public bool Hit(int hp)
    {
        if (hitCooldown > 0)
            return false;

        HealthDown(hp);
        if (health != 0)
        {
            hitCooldown = hitCooldown_unit;
        }
        return true;
    }

    public void HealthDown(int hp)
    {
        health -= hp;
        if (health <= 0)
        {
            animator.SetBool("isDead", true);
            health = 0;
        }
    }
}
