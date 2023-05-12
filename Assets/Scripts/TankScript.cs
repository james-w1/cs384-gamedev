using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AmmoType {HEAT, APFSDS, HEP, ATGM}

public class TankScript : MonoBehaviour
{
    [SerializeField] private string userName;
    [SerializeField] private int health;
    [SerializeField] private bool knockedOut;

    [SerializeField] private List<AmmoType> ammo;
    [SerializeField] private AmmoType selectedAmmo;

    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject cannon;
    [SerializeField] private GameObject endOfCannon;

    public bool stopped = false;

    private float speed = 0.003f;

    private float aimAngle;
    private float aimPower;
    private float sTime;

    private float minElevation = 8;
    private float maxElevation = 30;

    [SerializeField] public Rigidbody2D projectile;

    [SerializeField] public UnityEvent<string> StoppedEvent;

    void Start()
    {
        health = 100;
        ammo = new List<AmmoType>();
        for (int i = 0; i < GlobalData.loadedPlayer.ammo.Count; i++)
            ammo.Add(AmmoType.HEAT);
        selectedAmmo = AmmoType.HEAT;
        aimAngle = 0f;
        aimPower = 1000.0f;
    }

    public void knockOut()
    {
        knockedOut = true;
        Destroy(this.gameObject);
        // set sprite to knocked out state.
    }

    public void Fire()
    {
        if (ammo.Count <= 0)
            return;

        if (checkAndRemoveAmmo(selectedAmmo))
        {
            var clone = 
                Instantiate(projectile, endOfCannon.transform.position, cannon.transform.rotation)
                as Rigidbody2D;
            clone.GetComponent<Rigidbody2D>().AddForce(cannon.transform.right * aimPower);
        } else {
            Debug.Log("no ammo");
        }
    }

    private bool checkAndRemoveAmmo(AmmoType ammoType)
    {
        int index = ammo.IndexOf(ammoType);

        if (index < 0)
            return false;

        ammo.RemoveAt(index);
        return true;
    }

    public void UpdatePower(float f)
    {
        aimPower += f;
    }

    public void UpdateAngle(float f)
    {
        // bad practice :3
        var x = cannon.transform.eulerAngles.z;
        var y = turret.transform.eulerAngles.z;

        if (f < 0 && Mathf.Abs(Mathf.DeltaAngle(x, y)) < maxElevation) // up
            cannon.transform.RotateAround(cannon.transform.position, Vector3.back, f);
        if (f > 0 && Mathf.DeltaAngle(x, y) < minElevation) // down
            cannon.transform.RotateAround(cannon.transform.position, Vector3.back, f);
    }

    public void MoveTankTo(Vector3 point)
    {
        sTime = Time.time;
        StartCoroutine(movingCoroutine(point));
    }

    IEnumerator movingCoroutine(Vector3 point)
    {
        while (Vector2.Distance(transform.position, point) > speed * 10)
        {
            if (Time.time - sTime > 5.0f)
            {
                StoppedEvent?.Invoke("Tank Stopped");
                yield break;
            }

            transform.position = Vector2.MoveTowards(transform.position, point, speed);
            yield return new WaitForEndOfFrame();
        }

        StoppedEvent?.Invoke("Tank Stopped");
        transform.position = point;
    }

    /*
     * param damage the damage it should take from an attack.
     * return bool has attack killed the tank.
     */
    public bool damage(int damage)
    {
        health -= damage;

        Debug.Log(this);

        if (health <= 0)
            knockOut();

        return health <= 0;
    }
}
