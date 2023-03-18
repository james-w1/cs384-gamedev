using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AmmoType = {HEAT, APFSDS, HEP, ATGM}

public class TankScript : MonoBehaviour
{
    [SerializeField] private string userName;
    [SerializeField] private int health;
    [SerializeField] private int ammo;
    [SerializeField] private bool knockedOut;

    public bool stopped = false;

    private float speed = 0.003f;

    private Vector2 aimDirection;
    private float aimPower;
    private float sTime;

    private float minElevation = -8;
    private float maxElevation = 20;

    [SerializeField] public UnityEvent<string> StoppedEvent;

    void Start()
    {
        this.health = 100;
        this.ammo = 10;
        aimDirection = new Vector2(0, 0);
        aimPower = 0.0f;
    }

    public void knockOut()
    {
        this.knockedOut = true;
        // set sprite to knocked out state.
    }

    public void Fire()
    {
        if (ammo <= 0)
            return;

        Debug.Log("Power = " + this.aimPower);
        Debug.Log("angle = " + this.aimDirection);
    }

    public void UpdatePower(float f)
    {
        this.aimPower += f;
    }

    public void UpdateAngle(float f)
    {
        this.aimDirection.x = clampElevation(this.aimDirection.x + f);
    }

    float clampElevation(float f)
    {
        if (f > maxElevation)
            f = maxElevation;
        if (f < minElevation)
            f = minElevation;

        return f;
    }

    public void MoveTankTo(Vector3 point)
    {
        sTime = Time.time;
        StartCoroutine(movingCoroutine(point));
    }

    IEnumerator movingCoroutine(Vector3 point)
    {
        while (Vector2.Distance(this.transform.position, point) > speed * 10)
        {
            if (Time.time - sTime > 5.0f)
            {
                StoppedEvent?.Invoke("Tank Stopped");
                yield break;
            }

            this.transform.position = Vector2.MoveTowards(this.transform.position, point, speed);
            yield return new WaitForEndOfFrame();
        }

        StoppedEvent?.Invoke("Tank Stopped");
        this.transform.position = point;
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
