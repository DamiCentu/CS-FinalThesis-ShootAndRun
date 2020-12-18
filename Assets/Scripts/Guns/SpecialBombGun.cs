using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBombGun : IShootable {
    public GameObject prefabBombBullet;
    GameObject b;
    public override void Shoot(Transform shootPosition, Vector3 forward)
    {
        //print("asd");
        if (this._canShoot)
        {
            _timer = cooldown;
            //print("holu");
             b =Instantiate(prefabBombBullet, shootPosition.transform.position, Quaternion.Euler(forward));
            b.transform.forward = forward;
        }


    }


}


