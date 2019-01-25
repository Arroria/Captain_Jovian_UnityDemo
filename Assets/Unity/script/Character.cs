using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    //  Health Point Part
    [SerializeField] protected int maxHealthPoint;
    [SerializeField] protected int healthPoint;

    virtual public int HealthPoint() { return healthPoint; }
    virtual public void SetHealthPoint(int _healthPoint) { healthPoint = _healthPoint; }
    virtual public bool Hit(int _damage)
    {
        if (healthPoint > 0)
        {
            healthPoint -= _damage;
            if (healthPoint <= 0)
                healthPoint = 0;
            return true;
        }
        return false;
    }
    virtual public bool Heal(int _healPoint)
    {
        if (healthPoint > 0)
        {
            healthPoint += _healPoint;
            if (healthPoint > maxHealthPoint)
                healthPoint = maxHealthPoint;
            return true;
        }
        return false;
    }



    //  Direction Part
    [SerializeField] protected Vector2 direction;
    virtual public Vector2 Direction() { return direction; }
}
