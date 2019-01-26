using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Projectile
{
    [SerializeField] private float launchingForce;
    [SerializeField] private float brakingForce;
    private float brake = 0;

    static private float brake_max = 100;
    protected override void Update()
    {
        if (brake < brake_max)
        {
            brake += brakingForce * Time.deltaTime;
            if (brake > brake_max) brake = brake_max;

            velocity = launchingForce * Mathf.Cos((brake / brake_max) * Mathf.PI * 0.5f);
            base.Update();
        }
    }

    public bool IsBraked() { return brake >= brake_max; }
}