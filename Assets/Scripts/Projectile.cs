using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Projectile : MonoBehaviour
{
    public float _explosiveMass;
    public float _velocity;
    public float _damage;

    public Projectile()
    {}

    public Projectile(float explosiveMass, float velocity, float damage)
    {
        _explosiveMass = explosiveMass;
        _velocity = velocity;
        _damage = damage;
    }

    public Projectile(float velocity, float damage)
    {
        _explosiveMass = 0;
        _velocity = velocity;
        _damage = damage;
    }

    public abstract void Explode(Collision collision);
    public abstract void HitTarget(Collision collision);

    void OnCollisionEnter(Collision collision)
    {
        HitTarget(collision);

        if (_explosiveMass > 0)
            Explode(collision);
    }       
}
