using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileVisualizingSupporter : LRFliper
{
    [SerializeField] private Projectile projectile;
    private void Update() { In(projectile.Direction()); }
}
