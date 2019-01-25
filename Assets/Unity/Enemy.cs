using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    protected LRFliper lrFliper;
    protected Animator animator;

    
    protected float stateTime;
    protected float stunTime;
    [SerializeField] private float stunTime_unit;


    protected virtual void Start()
    {
        lrFliper = GetComponent<LRFliper>();
        animator = GetComponent<Animator>();

        stateTime = 0;
        stunTime = 0;
    }

    void Update()
    {
        if (HealthPoint() == 0)
            return;

        if (stunTime_unit == 0 || stunTime == 0 || TimeEx.Cooldown(ref stunTime) == 0)
        {
            if (TimeEx.Cooldown(ref stateTime) == 0)
                StateChange();
            StateUpdate();
        }
        animator.SetBool("isHit", stunTime != 0);

        //시선처리용 코드
        //릴리즈때 지워야댐
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

    public virtual void OnCollisionEnter2DByPhysicalCollider(Collision2D collision) {}
    protected abstract void StateChange();
    protected abstract void StateUpdate();
    protected void ResetStateTime(float time) { stateTime = time; }

    protected void ResetViewDir()
    {
        float angle = Random.Range(0.0f, Mathf.PI * 2);
        direction = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
    }



    public override bool Hit(int _damage)
    {
        if (base.Hit(_damage))
        {
            if (HealthPoint() != 0)
                stunTime = stunTime_unit;
            else
                animator.SetBool("isDead", true);
            return true;
        }
        return false;
    }
}