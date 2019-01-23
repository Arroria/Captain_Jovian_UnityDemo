using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour {

    private LRFliper lrFliper;

    public float velocity;
    [HideInInspector] public Vector2 direction;

    void Start ()
    {
        lrFliper = GetComponent<LRFliper>();
        lrFliper.In(direction);
    }
	
	void Update ()
    {
        Vector3 movement = direction * velocity * Time.deltaTime;
        transform.position += movement;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            collision.transform.GetComponent<Enemy>().Hit(1);
        }
    }
}