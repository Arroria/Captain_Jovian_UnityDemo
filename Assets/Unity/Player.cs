using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Player : Character
{

    public float velocity;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public GameObject cursor;

    public GameObject myWeapon;
    private Weapon myWeaponScript;


    [SerializeField] private float hitCooldown_unit;
    private float hitCooldown;
    [SerializeField] private float hitEffect_unit;


    private LRFliper lrFliper;

    void Start ()
    {
        lrFliper = GetComponent<LRFliper>();

        myWeaponScript = myWeapon.GetComponent<Weapon>();
    }

    void Update()
    {
        if (HealthPoint() == 0)
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
        direction = (curPos - myPos);
        direction.Normalize();

        lrFliper.In(direction);
    }



    public override bool Hit(int _damage)
    {
        if (hitCooldown > 0)
            return false;

        if (base.Hit(_damage))
        {
            if (HealthPoint() == 0)
                animator.SetBool("isDead", true);
            else
                hitCooldown = hitCooldown_unit;
            return true;
        }
        return false;
        
    }
}
