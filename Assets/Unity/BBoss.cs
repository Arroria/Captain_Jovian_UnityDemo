using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBoss : Enemy {
    private enum BehaviorState
    {
        Idle,
        Moving,
        Bike_in,
        Bike_riding,
        Bike_out,
        Attack_machinegun,
        Attack_machinegun_prefire,
        Attack_grenadelauncher,
    };

    public GameObject myWeapon_MG;
    public GameObject myWeapon_GL;
    private EnemyWeapon myWeaponController_MG;
    private EnemyWeapon myWeaponController_GL;


    [SerializeField] private float bikeSpeed;
    [SerializeField] private float weaponRotateSpeed;

    private BehaviorState state;

    private Astar astarPathFinder;
    private float astar_cooldown = 0;
    private bool astar_lastSucceeded = false;
    private int astar_index = 0;
    private float astar_remainDist = 0;
    private Vector2 astar_dir;

    private new void Start()
    {
        base.Start();
        myWeaponController_MG = myWeapon_MG.GetComponent<EnemyWeapon>();
        myWeaponController_GL = myWeapon_GL.GetComponent<EnemyWeapon>();
        myWeaponController_GL.GetComponent<SpriteRenderer>().enabled = false;
        astarPathFinder = GetComponent<Astar>();
        state = BehaviorState.Idle;
    }

    protected override void StateUpdate()
    {
        if (BehaviorState.Bike_riding == state)
        {
            if (PlayerTracking())
            {
                if (Vector2.Distance(Vector2ex.By3(GameObject.FindWithTag("Player").transform.position), Vector2ex.By3(transform.position)) < 100)
                {
                    SetState_BikeOut();
                    animator.SetTrigger("isBikeFinish");
                }
            }



            //으아 길찾기
            if (TimeEx.Cooldown(ref astar_cooldown) == 0)
            {
                astar_cooldown = 0.5f;
                if ((astar_lastSucceeded = astarPathFinder.경로검색(GameObject.FindWithTag("Player").transform.position)) == true)
                {
                    List<Astar.최단경로노드> path = astarPathFinder.GetPath();
                    astar_index = 0;
                    astar_dir = Vector2ex.By3(path[0].월드위치) - Vector2ex.By3(transform.position);
                    astar_remainDist = astar_dir.magnitude;
                    astar_dir /= astar_remainDist;
                }
            }
            if (astar_lastSucceeded)
            {
                float movement = bikeSpeed * Time.deltaTime;
                while (true)
                {
                    if (astar_remainDist < movement)
                    {
                        transform.position += Vector2ex.To3(astar_dir) * astar_remainDist;
                        movement -= astar_remainDist;


                        List<Astar.최단경로노드> path = astarPathFinder.GetPath();
                        astar_index++;
                        if (astar_index >= path.Count)
                        {
                            astar_cooldown = 0;
                            break;
                        }
                        astar_dir = Vector2ex.By3(path[astar_index].월드위치) - Vector2ex.By3(transform.position);
                        astar_remainDist = astar_dir.magnitude;
                        astar_dir /= astar_remainDist;
                    }
                    else
                    {
                        transform.position += Vector2ex.To3(astar_dir) * movement;
                        astar_remainDist -= movement;
                        break;
                    }
                }
            }

            lrFliper.In(astar_dir);
        }
        else if (BehaviorState.Bike_out == state)
        {
            float movement = bikeSpeed * Time.deltaTime;
            transform.position += Vector2ex.To3(astar_dir) * movement;
        }
        else if (BehaviorState.Attack_machinegun == state ||
                BehaviorState.Attack_machinegun_prefire == state ||
                BehaviorState.Attack_grenadelauncher == state)
        {
            if (PlayerTracking())
            {
                GameObject player = GameObject.FindWithTag("Player");
                Vector2 destDir = (Vector2ex.By3(player.transform.position) - Vector2ex.By3(transform.position)).normalized;

                Vector2 viewDir = ViewDir();
                float angle = Vector2.Angle(destDir, viewDir);
                if (angle <= weaponRotateSpeed * Time.deltaTime)
                {
                    if (state == BehaviorState.Attack_grenadelauncher)  myWeaponController_GL.WeaponFire();
                    else                                                myWeaponController_MG.WeaponFire();
                    SetViewDir(destDir);
                }
                else
                {
                    bool isRevClockwise = viewDir.x * destDir.y - viewDir.y * destDir.x >= 0;
                    SetViewDir(Vector2ex.Rotate(viewDir, weaponRotateSpeed * Mathf.Deg2Rad * Time.deltaTime * (isRevClockwise ? 1 : -1)));
                }
            }
            else
            {
                if (BehaviorState.Attack_machinegun == state)
                {
                    stateTime -= 1;
                    if (stateTime <= 0)
                        SetState_Idle();
                    else
                        SetState_AttackMachinegun_Prefire();
                }
            }
        }
        else if (BehaviorState.Moving == state)
            transform.position += Vector2ex.To3(ViewDir());
    }

    public override void OnCollisionEnter2DByPhysicalCollider(Collision2D collision)
    {
        if (state == BehaviorState.Bike_in ||
            state == BehaviorState.Bike_riding ||
            state == BehaviorState.Bike_out ||
            state == BehaviorState.Attack_machinegun)
            return;
        SetState_Move();
    }

    protected override void StateChange()
    {
        if (state == BehaviorState.Bike_in ||
            state == BehaviorState.Bike_riding ||
            state == BehaviorState.Bike_out)
            return;

        float playerDist = Vector2.Distance(Vector2ex.By3(GameObject.FindWithTag("Player").transform.position), Vector2ex.By3(transform.position));
        if (!PlayerTracking() ||
            Random.Range(0.0f, 1.0f) < playerDist / 200.0f)
        {
            SetState_BikeIn();
        }
        else
        {
            if (state == BehaviorState.Attack_machinegun ||
                state == BehaviorState.Attack_machinegun_prefire ||
                state == BehaviorState.Attack_grenadelauncher)
            {
                float rnd = Random.Range(0, 100);
                if (rnd < 65)   SetState_Idle();
                else            SetState_Move();
            }
            else
            {
                float rnd = Random.Range(0, 100);
                //if (rnd < 65)   SetState_AttackMachinegun();
                //else
                    SetState_AttackGrenadelauncher();
            }
        }
    }
    private void SetState_Idle()                        { state = BehaviorState.Idle;                       animator.SetBool("isMoving", false);    ResetStateTime(Random.Range(0.5f, 2.0f)); }
    private void SetState_Move()                        { state = BehaviorState.Moving;                     animator.SetBool("isMoving", true);     ResetStateTime(Random.Range(0.5f, 2.0f));   ResetViewDir(); }
    private void SetState_BikeIn()                      { state = BehaviorState.Bike_in;                    animator.SetTrigger("isBike");          ResetStateTime(0);                          myWeaponController_MG.GetComponent<SpriteRenderer>().enabled = false; }
    private void SetState_BikeRiding()                  { state = BehaviorState.Bike_riding;                                                        ResetStateTime(0); }
    private void SetState_BikeOut()                     { state = BehaviorState.Bike_out;                                                           ResetStateTime(0); }
    private void SetState_AttackMachinegun()            { state = BehaviorState.Attack_machinegun;          animator.SetBool("isMoving", false);    ResetStateTime(Random.Range(0.5f, 5.0f)); }
    private void SetState_AttackMachinegun_Prefire()    { state = BehaviorState.Attack_machinegun_prefire;  animator.SetBool("isMoving", false); }
    private void SetState_AttackGrenadelauncher()       { state = BehaviorState.Attack_grenadelauncher;     animator.SetBool("isMoving", false);    ResetStateTime(Random.Range(1.0f, 3.0f)); }

    public void BikeInEndEvent()
    {
        SetState_BikeRiding();
        animator.ResetTrigger("isBike");
    }
    public void BikeOutEndEvent()
    {
        SetState_AttackMachinegun();
        animator.ResetTrigger("isBikeFinish");
        myWeaponController_MG.GetComponent<SpriteRenderer>().enabled = true;
    }


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
