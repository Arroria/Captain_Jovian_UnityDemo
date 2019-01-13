using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : Enemy {
    private enum BehaviorState
    {
        Idle,
        Moving,
        Attack,
    };

    public GameObject myWeapon;
    private EnemyWeapon myWeaponController;

    [SerializeField] private float weaponRotateSpeed;

    private BehaviorState state;

    private Vector2 destDir;
    private Vector2 dashDir;

    private new void Start()
    {
        base.Start();
        myWeaponController = myWeapon.GetComponent<EnemyWeapon>();
        state = BehaviorState.Idle;
    }

    protected override void StateUpdate()
    {
        if (BehaviorState.Attack == state)
        {
            if (PlayerTracking())
            {
                GameObject player = GameObject.FindWithTag("Player");
                Vector2 destDir = (Vector2ex.By3(player.transform.position) - Vector2ex.By3(transform.position)).normalized;

                Vector2 viewDir = ViewDir();
                float angle = Vector2.Angle(destDir, viewDir);
                if (angle <= weaponRotateSpeed * Time.deltaTime)
                {
                    destDir = viewDir;
                    myWeaponController.WeaponFire();
                }
                else
                {
                    bool isRevClockwise = viewDir.x * destDir.y - viewDir.y * destDir.x >= 0;
                    SetViewDir(Vector2ex.Rotate(viewDir, weaponRotateSpeed * Mathf.Deg2Rad * Time.deltaTime * (isRevClockwise ? 1 : -1)));
                }

                transform.position += Vector2ex.To3(dashDir * 1);
            }
            else
                SetStateIdle();
        }
        else if (BehaviorState.Moving == state)
            transform.position += Vector2ex.To3(ViewDir());
    }

    public override void OnCollisionEnter2DByPhysicalCollider(Collision2D collision) { SetStateMove(); }

    protected override void StateChange()
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
        ResetViewDir();

        state = BehaviorState.Moving;
        animator.SetBool("isMoving", true);

        ResetStateTime();
    }
    private void SetStateAttack()
    {
        state = BehaviorState.Attack;
        animator.SetBool("isMoving", true);

        GameObject player = GameObject.FindWithTag("Player");
        dashDir = (Vector2ex.By3(player.transform.position) - Vector2ex.By3(transform.position)).normalized;

        ResetStateTime();
    }
    private void ResetStateTime() { ResetStateTime(Random.Range(0.5f, 2.0f)); }


    private bool PlayerTracking()
    {
        Vector2 playerDir;
        float playerDist;
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null) return false;

            Vector2 playerDirst = Vector2ex.By3(player.transform.position) - Vector2ex.By3(transform.position);
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
