using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private enum BehaviorState
    {
        Idle,
        Moving,
        Attack,
    };

    public GameObject myWeapon;
    private EnemyWeapon myWeaponController;

    public float weaponRotateSpeed;

    public int health;
    private BehaviorState state;
    private float stateTime;
    private float stunTime;

    [HideInInspector] public Vector2 direction;

    private LRFliper lrFliper;
    private Animator animator;

    void Start()
    {
        lrFliper = GetComponent<LRFliper>();
        animator = GetComponent<Animator>();
        myWeaponController = myWeapon.GetComponent<EnemyWeapon>();

        state = BehaviorState.Idle;
        stateTime = 0;
        stunTime = 0;
    }

    void Update()
    {
        if (health == 0)
            return;

        if (!StunCooling())
        {
            if (!StateCooling())
                StateChange();

            if (BehaviorState.Attack == state)
            {
                if (PlayerTracking())
                {
                    Vector2 playerDir;
                    {
                        GameObject player = GameObject.FindWithTag("Player");
                        playerDir = (Vector2ex.By3(player.transform.position) - Vector2ex.By3(transform.position)).normalized;
                    }

                    float angle = Vector2.Angle(direction, playerDir);
                    if (angle <= weaponRotateSpeed * Time.deltaTime)
                    {
                        direction = playerDir;
                        myWeaponController.WeaponFire();
                    }
                    else
                    {
                        bool isRevClockwise = direction.x * playerDir.y - direction.y * playerDir.x >= 0;
                        direction = Vector2ex.Rotate(direction, weaponRotateSpeed * Mathf.Deg2Rad * Time.deltaTime * (isRevClockwise ? 1 : -1));
                    }
                }
                else
                    SetStateIdle();
            }
            else if (BehaviorState.Moving == state)
            {
                Vector3 dir = direction;
                transform.position += dir;
            }
        }


        //raycast test
        RaycastHit2D rcHit = Physics2D.Raycast(transform.position, direction, 10000.0f, 1 << LayerMask.NameToLayer("Map"));
        if (rcHit.collider != null)
        {
            Vector2 dirst = direction * rcHit.distance;
            Vector3 collPos = transform.position + new Vector3(dirst.x, dirst.y, 0);
            Vector3 linePos = new Vector3(transform.position.x, transform.position.y, 0);

            Debug.DrawLine(linePos, collPos, Color.red);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "AllyAttack")
        {
            HealthDown(1);
            stunTime = 0.2f;
            animator.SetBool("isHit", true);
        }
    }

    public void OnCollisionEnter2DByPhysicalCollider(Collision2D collision) { SetStateMove(); }



    private bool StunCooling()
    {
        if (stunTime == 0)
            return false;

        if (stunTime > Time.deltaTime)
        {
            stunTime -= Time.deltaTime;
            return true;
        }
        else
            stunTime = 0;

        animator.SetBool("isHit", false);
        return false;
    }


    private bool StateCooling()
    {
        if (stateTime == 0)
            return false;

        if (stateTime > Time.deltaTime)
        {
            stateTime -= Time.deltaTime;
            return true;
        }
        else
            stateTime = 0;

        return false;
    }
    private void StateChange()
    {
        if (Random.Range(0.0f, 1.0f) <= 0.8f && PlayerTracking())   SetStateAttack();
        else if (Random.Range(0, 2) == 0)                           SetStateIdle();
        else                                                        SetStateMove();
    }
    private void SetStateIdle()
    {
        state = BehaviorState.Idle;
        animator.SetBool("isMoving", false);

        ResetStateTime();
    }
    private void SetStateMove()
    {
        ResetDirection();

        state = BehaviorState.Moving;
        animator.SetBool("isMoving", true);

        ResetStateTime();
    }
    private void SetStateAttack()
    {
        state = BehaviorState.Attack;
        animator.SetBool("isMoving", false);

        ResetStateTime();
    }
    private void ResetStateTime() { stateTime = Random.Range(0.5f, 2.0f); }

    private void ResetDirection()
    {
        float angle = Random.Range(0.0f, Mathf.PI * 2);
        direction = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
        lrFliper.In(direction);
    }

    private void HealthDown(int value)
    {
        if ((health -= value) <= 0)
        {
            health = 0;
            animator.SetBool("isDead", true);
        }
    }


    private bool PlayerTracking()
    {
        Vector2 playerDir;
        float playerDist;
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null) return false;

            Vector2 playerPos = player.transform.position;
            Vector2 myPos = transform.position;
            Vector2 playerDirst = playerPos - myPos;
            playerDist = playerDirst.magnitude;
            playerDir = playerDirst / playerDist;
        }

        RaycastHit2D rcHit = Physics2D.Raycast(transform.position, playerDir, 10000.0f, 1 << LayerMask.NameToLayer("Map"));
        if (rcHit.collider != null)
        {
            if (rcHit.distance < playerDist)
                return false;
        }
        return true;
    }
}
