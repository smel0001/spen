//TEMP SHOOTING MECHANICS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public GameObject bullet;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Bullet newBullet = Instantiate<GameObject>(bullet, transform.position, Quaternion.identity, this.transform).GetComponent<Bullet>();
        newBullet.dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
    }
}
