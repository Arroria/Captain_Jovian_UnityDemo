using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    private Player player;
    private LRFliper lrFliper;

    public GameObject bulletPrefab;

    public float coolTimeMax;
    private float coolTime;

    void Start ()
    {
        lrFliper = GetComponent<LRFliper>();
        player = transform.parent.GetComponent<Player>();
    }
	
	void Update ()
    {
        lrFliper.In(_my_dir());

        //Cooldown
        if (coolTime > Time.deltaTime)
            coolTime -= Time.deltaTime;
        else
            coolTime = 0;
    }

    public bool WeaponFire()
    {
        if (coolTime != 0)
            return false;


        coolTime = coolTimeMax;

        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = transform.position;
        bullet.GetComponent<Bullet>().direction = _my_dir();
        return true;
    }

    Vector2 _my_dir() { return player.myDirection; }
}
