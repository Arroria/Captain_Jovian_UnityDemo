using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet_old : MonoBehaviour
{
    private LRFliper lrFliper;

    public float velocity;
    [HideInInspector] public Vector2 direction;

    void Start()
    {
        lrFliper = GetComponent<LRFliper>();
        lrFliper.In(direction);
    }

    void Update()
    {
        Vector3 movement = direction * velocity * Time.deltaTime;
        transform.position += movement;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<Player>().Hit(1);
            Destroy(gameObject);
        }
    }
}
