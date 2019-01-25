using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileEffector : MonoBehaviour
{
    [SerializeField] Projectile projectile;
    [SerializeField] string target;
    [SerializeField] int damage;

    private void Start()
    { if (projectile == null) Debug.LogError("\"this.projectile\" is null"); }

    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == target)
        {
            collision.GetComponent<Character>().Hit(damage);
            Destroy(gameObject);
        }
    }
}