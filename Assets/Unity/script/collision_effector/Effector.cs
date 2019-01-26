using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//보류
public class Effector : MonoBehaviour
{
    [SerializeField] string target;
    [SerializeField] int damage;

    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == target)
        {
            collision.GetComponent<Character>().Hit(damage);
            Destroy(gameObject);
        }
    }
}