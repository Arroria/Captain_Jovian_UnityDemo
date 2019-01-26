using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//보류
public class GrenadeEffector : MonoBehaviour
{
    [SerializeField] Grenade grenade;
    [SerializeField] string target;
    [SerializeField] int damage;

    private void Start()
    { if (grenade == null) Debug.LogError("\"this.projectile\" is null"); }

    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == target)
        {
            collision.GetComponent<Character>().Hit(damage);
            Destroy(gameObject);
        }
    }
}