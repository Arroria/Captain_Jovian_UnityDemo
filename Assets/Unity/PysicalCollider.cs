using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PysicalCollider : MonoBehaviour
{
    public Vector2 colliderOffset;
    public Vector2 colliderSize;

    [System.Serializable] public class CollisionEvent : UnityEvent<Collision2D> {};
    [SerializeField] public CollisionEvent collisionEnter;
    [SerializeField] public CollisionEvent collisionStay;
    [SerializeField] public CollisionEvent collisionExit;


    void Start()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.offset = colliderOffset;
        collider.size = colliderSize;
    }

    private void Update()
    {
        transform.parent.position += transform.localPosition;
        transform.localPosition = Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)  { collisionEnter.Invoke(collision); }
    private void OnCollisionStay2D(Collision2D collision)   { collisionStay.Invoke(collision); }
    private void OnCollisionExit2D(Collision2D collision)   { collisionExit.Invoke(collision); }
}
