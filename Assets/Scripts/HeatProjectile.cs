using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class HeatProjectile : Projectile 
{
    public HeatProjectile() 
    {

    }

    public override void Init()
    {
        explosiveMass = 0.5f;
        velocity = 0.0f;
        damage = 100.0f;
    }

    public override void HitTarget(Collision2D collision)
    {
        return;
    }
}
