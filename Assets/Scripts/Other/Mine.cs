﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Mine : IShootable {

    public GameObject prefabMine;
    bool mineSet= false;
    GameObject mine;
    public float radius;
    public LayerMask hittableLayer;
    public float timeToBoom = 2;
    Timer timer;
    public int damage = 3;
    private int maxNumber = 3;
    private float waitTime = 2;
    private List<AbstractEnemy> enemiesToAffects;
    private float minDistance = 5;
    MineBullet bullet; 

    public override void Shoot(Transform shootPosition, Vector3 forward)
    {
        mine = Instantiate(prefabMine, shootPosition.transform.position, Quaternion.Euler(forward));
         bullet = mine.GetComponent<MineBullet>();
        bullet.SetRadius(radius);
        bullet.Boom();
        StartCoroutine("StartBoom");
        /*   if (mineSet) {
               mine.GetComponent<MineBullet>().Boom();
               mineSet = false;
           }

           else if (this._canShoot)
           {
               _timer = cooldown;
               mine = Instantiate(prefabMine, shootPosition.transform.position, Quaternion.Euler(forward));
               mine.transform.forward = forward;
               mineSet = true;
           }*/

    }

    private IEnumerator StartBoom()
    {

        yield return new WaitForSeconds(bullet.waitTime);
        GetComponentInChildren<AudioSource>().Play();

    }
}
