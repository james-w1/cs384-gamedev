using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankScript : MonoBehaviour
{
    [SerializeField] private string userName;
    [SerializeField] private int health;
    [SerializeField] private bool knockedOut;
    private Vector2 aimDirection;
    private float aimPower;

    void Start()
    {
        this.health = 100;
        aimDirection = new Vector2(0, 0);
        aimPower = 0.0f;
    }

    public void knockOut()
    {
        this.knockedOut = true;
        // set sprite to knocked out state.
    }

    public void updatePower(GameObject obj, float f)
    {
        if (this != obj)
            return;

        this.aimPower += f;
    }

    public void updateDirection(GameObject obj, float f)
    {
        if (this != obj)
            return;

        this.aimDirection.x += f;
    }

    /*
     * param damage the damage it should take from an attack.
     * return bool has this attack killed the tank.
     */
    public bool damage(int damage)
    {
        this.health -= damage;

        if (this.health <= 0)
            this.knockOut();

        return this.health <= 0;
    }
}
