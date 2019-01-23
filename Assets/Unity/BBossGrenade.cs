using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBossGrenade : MonoBehaviour
{
    private LRFliper lrFliper;

    [SerializeField] private float launchingForce;
    [SerializeField] private float brakingForce;
    private float brake = 0;
    private float velocity;
    [HideInInspector] public Vector2 direction;

    void Start()
    {
        lrFliper = GetComponent<LRFliper>();
        lrFliper.In(direction);
    }

    void Update()
    {
        if (brake < 1)
        {
            brake += brakingForce * Time.deltaTime;
            if (brake > 1)  brake = 1;

            velocity = Mathf.Cos(brake * Mathf.PI * 0.5f) * launchingForce;
            Vector3 movement = direction * velocity * Time.deltaTime;
            transform.position += movement;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.transform.tag == "Player")
        //{
        //    collision.transform.GetComponent<Player>().Hit(1);
        //    Destroy(gameObject);
        //}
    }
}
