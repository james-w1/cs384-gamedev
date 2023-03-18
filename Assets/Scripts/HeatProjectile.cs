using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeatProjectile : Projectile 
{
    public HeatProjectile(float explosiveMass, float velocity, float damage)
    {
        _explosiveMass = explosiveMass;
        _velocity = velocity;
        _damage = damage;
    }

    public override void Explode(Collision collision)
    {
        return;
    }

    public override void HitTarget(Collision collision)
    {
        return;
    }
}
