using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    private Player player;
    private SpriteRenderer spriteRenderer;
    //[HideInInspector]
    public Vector2 dir;

    public GameObject bulletPrefab;

    void Start ()
    {
        dir = new Vector2(1, 0);

        spriteRenderer = GetComponent<SpriteRenderer>();
        player = transform.parent.GetComponent<Player>();
    }
	
	void Update ()
    {
        //Call Flip State from Player
        bool isFlip = spriteRenderer.flipX = player.spriteRenderer.flipX;

        //Dir Setting
        dir = player.cursor.transform.position - transform.position; dir.Normalize();
        float angle = Vector2.Angle(dir, new Vector2(0, -1));
        if (isFlip)
            angle = 180 - angle;
        angle -= 90;

        //Set Rotation
        transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));

    }

    public bool WeaponFire()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = transform.position;
        bullet.GetComponent<Bullet>().direction = dir;
        return true;
    }
}
