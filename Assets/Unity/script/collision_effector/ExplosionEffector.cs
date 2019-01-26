using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffector : MonoBehaviour
{
    [SerializeField] string target;
    [SerializeField] int damage;

    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == target)
            collision.GetComponent<Character>().Hit(damage);
    }
}
