using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float velocity;
    protected Vector2 direction;

    protected virtual void Update()
    {
        transform.position += Vector2ex.To3( velocity * Time.deltaTime * direction );
    }

    virtual public float Velocity() { return velocity; }
    public Vector2 Direction() { return direction; }
    public void SetDirection(Vector2 _direction) { direction = _direction; }
}