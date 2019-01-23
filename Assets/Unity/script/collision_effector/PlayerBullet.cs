using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] Projectile projectile;
    [SerializeField] int damage;

    private void Start()
    { if (projectile == null) Debug.LogError("\"this.projectile\" is null"); }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().Hit(damage);
            Destroy(gameObject);
        }
    }
}
