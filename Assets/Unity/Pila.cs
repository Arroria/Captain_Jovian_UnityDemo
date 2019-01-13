using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Pila : Enemy {
    private enum BehaviorState
    {
        Idle,
        Moving,
    };
    
    private BehaviorState state;

    private new void Start()
    {
        base.Start();
        state = BehaviorState.Idle;
    }

    protected override void StateUpdate()
    {
        if (BehaviorState.Moving == state)
            transform.position += Vector2ex.To3(ViewDir());
    }

    public override void OnCollisionEnter2DByPhysicalCollider(Collision2D collision) { SetStateMove(); }


    
    protected override void StateChange()
    {
        if (Random.Range(0, 2) == 0)    SetStateIdle();
        else                            SetStateMove();
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
    private void ResetStateTime() { ResetStateTime(Random.Range(0.5f, 2.0f)); }
}
