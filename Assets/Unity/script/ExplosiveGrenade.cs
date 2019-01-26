using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveGrenade : MonoBehaviour
{
    [SerializeField] private Grenade grenade;

    [SerializeField] private float maxFuse;
    private float fuse;

    [SerializeField] private GameObject explosionPrefab;

    void Start()
    {
        fuse = maxFuse;
    }

    // Update is called once per frame
    void Update()
    {
        if (grenade.IsBraked())
        {
            if (TimeEx.Cooldown(ref fuse) == 0)
            {
                GameObject explosion = Instantiate(explosionPrefab);
                explosion.transform.position = transform.position;
                Destroy(gameObject);
            }
        }
    }
}
