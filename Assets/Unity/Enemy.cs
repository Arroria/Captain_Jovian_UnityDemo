using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private enum BehaviorState
    {
        Idle,
        Moving,
    };

    public int health;
    private BehaviorState state;
    private float stateTime;
    private float stunTime;

    private Vector2 direction;

    private LRFliper lrFliper;
    private Animator animator;

    void Start()
    {
        lrFliper = GetComponent<LRFliper>();
        animator = GetComponent<Animator>();

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
                ResetState();

            if (BehaviorState.Moving == state)
            {
                Vector3 dir = direction;
                transform.position += dir;
            }
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

    public void OnCollisionEnter2DByPhysicalCollider(Collision2D collision)
    {
        SetStateMove();
    }



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

    private void ResetState()
    {
        animator.SetBool("isMoving", false);

        if (Random.Range(0, 2) == 0)
            SetStateIdle();
        else
            SetStateMove();
    }

    private void SetStateIdle()
    {
        state = BehaviorState.Idle;

        ResetStateTime();
    }

    private void SetStateMove()
    {
        ResetDirection();

        state = BehaviorState.Moving;
        animator.SetBool("isMoving", true);

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
}
