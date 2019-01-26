using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//보류
public class Explosive : MonoBehaviour
{
    [SerializeField] protected Projectile projectile;

    [SerializeField] protected float maxFuse;
    private float fuse;

    [SerializeField] protected GameObject explosionPrefab;

    void Start()
    {
        fuse = maxFuse;
    }

    // Update is called once per frame
    void Update()
    {
        //if (grenade.IsBraked())
        //{
        //    if (TimeEx.Cooldown(ref fuse) == 0)
        //    {
        //        Instantiate(explosionPrefab);
        //        Destroy(gameObject);
        //    }
        //}
    }
}
