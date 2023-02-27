using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankScript : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] bool knockedOut;

    void Start()
    {
        this.health = 100;
    }

    public void knockOut()
    {
        this.knockedOut = true;
        // set sprite to knocked out state.
    }

    /*
     * param damage the damage it should take from an attack.
     * return bool has this attack killed the tank.
     */
    public bool damage(int damage)
    {
        this.health -= damage;
        return this.health <= 0;
    }
}
