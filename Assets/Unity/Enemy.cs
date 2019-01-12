using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected LRFliper lrFliper;
    protected Animator animator;


    [SerializeField] private int health;
    private float stateTime;
    private float stunTime;
    [SerializeField] private float stunTime_unit;

    private Vector2 viewDir;

    protected virtual void Start()
    {
        lrFliper = GetComponent<LRFliper>();
        animator = GetComponent<Animator>();

        stateTime = 0;
        stunTime = 0;
    }

    void Update()
    {
        if (health == 0)
            return;

        if (stunTime == 0 || TimeEx.Cooldown(ref stunTime) == 0)
        {
            if (TimeEx.Cooldown(ref stateTime) == 0)
                StateChange();
            StateUpdate();
        }
        animator.SetBool("isHit", stunTime != 0);

        //시선처리용 코드
        //릴리즈때 지워야댐
        //raycast test
        RaycastHit2D rcHit = Physics2D.Raycast(transform.position, viewDir, 10000.0f, 1 << LayerMask.NameToLayer("Map"));
        if (rcHit.collider != null)
        {
            Vector2 dirst = viewDir * rcHit.distance;
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
        viewDir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
        lrFliper.In(viewDir);
    }



    public virtual void HealthDown(int hp)
    {
        health -= hp;
        if (health <= 0)
        {
            animator.SetBool("isDead", true);
            health = 0;
        }
    }

    public virtual void Hit(int hp)
    {
        HealthDown(hp);
        if (health != 0)
            stunTime = stunTime_unit;
    }

    public Vector2 ViewDir() { return viewDir; }
    public void SetViewDir(Vector2 _viewDir)
    {
        viewDir = _viewDir;
        lrFliper.In(viewDir);
    }
}