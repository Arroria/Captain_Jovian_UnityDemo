﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : Enemy {
    private enum BehaviorState
    {
        Idle,
        Moving,
        Attack,
    };

    [SerializeField] private Weapon weapon;
    [SerializeField] private float weaponRotateSpeed;

    private BehaviorState state;

    private Vector2 destDir;
    private Vector2 dashDir;
    private bool playerLost;

    private new void Start()
    {
        base.Start();
        state = BehaviorState.Idle;
    }

    protected override void StateUpdate()
    {
        if (BehaviorState.Attack == state)
        {
            if (playerLost)
                transform.position += Vector2ex.To3(dashDir * 1);
            else
            {
                if (PlayerTracking())
                {
                    GameObject player = GameObject.FindWithTag("Player");
                    destDir = (Vector2ex.By3(player.transform.position) - Vector2ex.By3(transform.position)).normalized;

                    float angle = Vector2.Angle(destDir, direction);
                    if (angle <= weaponRotateSpeed * Time.deltaTime)
                    {
                        direction = destDir;
                        weapon.WeaponFire();
                    }
                    else
                    {
                        bool isRevClockwise = direction.x * destDir.y - direction.y * destDir.x >= 0;
                        direction = Vector2ex.Rotate(direction, weaponRotateSpeed * Mathf.Deg2Rad * Time.deltaTime * (isRevClockwise ? 1 : -1));
                    }

                    transform.position += Vector2ex.To3(dashDir * 1);
                }
                else
                    playerLost = true;
            }
        }
        else if (BehaviorState.Moving == state)
            transform.position += Vector2ex.To3(direction);
    }

    public override void OnCollisionEnter2DByPhysicalCollider(Collision2D collision)
    {
        if (state == BehaviorState.Attack)
            SetStateAttack();
        else
            SetStateMove();
    }

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
        playerLost = false;

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
