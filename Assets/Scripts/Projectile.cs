using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public abstract class Projectile : MonoBehaviour
{
    public float explosiveMass;
    public float velocity;
    public float damage;
    public Vector2 impactPoint;
    public Tilemap tilemap;
    public GameObject[] tanks;

    public GameObject explosion;

    public void Start()
    {
        tilemap = GameObject.Find("/Grid/Ground").GetComponent<Tilemap>();
        tanks = GameObject.FindGameObjectsWithTag("tank");
        Init();
    }

    public Projectile()
    {

    }

    public abstract void Init();

    public void Explode(Collision2D collision)
    {
        Destroy(gameObject);
        impactPoint = collision.GetContact(0).point;

        Instantiate(explosion, impactPoint, Quaternion.identity);

        // remove the tilemap tiles in the radius of the explosion
        for (float x = impactPoint.x - explosiveMass; x < impactPoint.x + explosiveMass; x += 0.01f)
        {
            for (float y = impactPoint.y - explosiveMass; y < impactPoint.y + explosiveMass; y += 0.01f)
            {
                if (Vector2.Distance(impactPoint, new Vector2(x, y)) < explosiveMass)
                {
                    var tilePos = tilemap.WorldToCell(new Vector2(x, y));
                    tilemap.SetTile(tilePos, null);
                }
            }
        }

        var colliders = Physics2D.OverlapCircleAll(impactPoint, explosiveMass);
        foreach (var collider in colliders)
        {
            //Debug.Log($"{collider.gameObject.name} is nearby");
            foreach (GameObject tank in tanks) {
                if (tank == collider.gameObject) {
                    tank.SendMessage("damage", 100);
                }
            }
        }
        // work out distance between enemy and explosion then deal dmg
        
        return;
    }

    public abstract void HitTarget(Collision2D collision);

    void OnCollisionEnter2D(Collision2D collision)
    {
        HitTarget(collision);

        if (explosiveMass > 0)
            Explode(collision);
    }       
}
