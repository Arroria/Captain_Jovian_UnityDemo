using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private Enemy enemy;
    private LRFliper lrFliper;

    public GameObject bulletPrefab;


    public float cooldownMax;
    private float cooldown;

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        lrFliper = GetComponent<LRFliper>();
    }

    void Update()
    {
        lrFliper.In(_my_dir());

        Cooldown();
    }


    public bool WeaponFire()
    {
        if (cooldown != 0)  return false;
        cooldown = cooldownMax;

        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = transform.position;
        bullet.GetComponent<EnemyBullet>().direction = _my_dir();
        return true;
    }


    private void Cooldown()
    {
        if (cooldown > Time.deltaTime)  cooldown -= Time.deltaTime;
        else                            cooldown = 0;
    }
    private Vector2 _my_dir() { return enemy.direction; }
}
