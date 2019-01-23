using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    [SerializeField] protected int healthPoint;

    virtual public int HealthPoint() { return healthPoint; }
    virtual public void SetHealthPoint(int _healthPoint) { healthPoint = _healthPoint; }
    virtual public void Hit(int _damage)
    {
        if (healthPoint > 0)
        {
            healthPoint -= _damage;
            if (healthPoint <= 0)
            {
                healthPoint = 0;
                Dead();
            }
        }
    }
    virtual public void Dead() {}
}
