using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [SerializeField] private Character weaponOwner;
    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private LRFliper lrFliper;


    [SerializeField] private float maxCooldown;
    private float cooldown;



	void Update ()
    {
        lrFliper.In(weaponOwner.Direction());

        TimeEx.Cooldown(ref cooldown);
    }

    public bool WeaponFire()
    {
        if (cooldown != 0)
            return false;


        cooldown = maxCooldown;

        GameObject bullet = Instantiate(projectilePrefab);
        bullet.transform.position = transform.position;
        bullet.GetComponent<Projectile>().SetDirection(weaponOwner.Direction());
        return true;
    }
}
