using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TankScript : MonoBehaviour
{
    [SerializeField] private string userName;
    [SerializeField] private int health;
    [SerializeField] private bool knockedOut;

    private float speed = 0.003f;

    private Vector2 aimDirection;
    private float aimPower;
    private float sTime;

    public static UnityEvent StoppedEvent;

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

    public void MoveTankTo(Vector3 point)
    {
        sTime = Time.time;
        StartCoroutine(movingCoroutine(point));
    }

    IEnumerator movingCoroutine(Vector3 point)
    {
        while (Vector2.Distance(this.transform.position, point) > speed)
        {
            if (Time.time - sTime > 5.0f)
            {
                StoppedEvent?.Invoke();
                yield break;
            }

            this.transform.position = Vector2.MoveTowards(this.transform.position, point, speed);
            yield return new WaitForEndOfFrame();
        }

        this.transform.position = point;
        StoppedEvent?.Invoke();
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
