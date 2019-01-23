using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] Projectile projectile;
    [SerializeField] int damage;

    private void Start()
    { if (projectile == null) Debug.LogError("\"this.projectile\" is null"); }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.GetComponent<Player>().Hit(damage))
                Destroy(gameObject);
        }
    }
}
