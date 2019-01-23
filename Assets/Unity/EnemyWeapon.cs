using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private Enemy enemy;
    private LRFliper lrFliper;

    public GameObject bulletPrefab;


    public float cooldownMax;
    protected float cooldown;

    protected void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        lrFliper = GetComponent<LRFliper>();
    }

    protected void Update()
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
        bullet.GetComponent<Projectile>().SetDirection(_my_dir());
        return true;
    }


    private void Cooldown()
    {
        if (cooldown > Time.deltaTime)  cooldown -= Time.deltaTime;
        else                            cooldown = 0;
    }
    private Vector2 _my_dir() { return enemy.ViewDir(); }
}
