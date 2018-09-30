﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour {
    private Vector3 _target;
    public float radius;
    public LayerMask hittableLayers;
    public Transform shootPos;
    public float minTimeTiShoot;
    Timer _timer;
    // Use this for initialization
    void Start () {
         _timer = new Timer(minTimeTiShoot, Shoot);
    }

    private void Shoot()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, hittableLayers);
        int i = 0;
        float minDis = 10000;
        while (i < hitColliders.Length)
        {
            float dis = Vector3.Distance(transform.position, hitColliders[i].transform.position);
            if (dis < minDis)
            {
                minDis = dis;
                this.transform.LookAt(hitColliders[i].transform.position);
            }
            i++;
        }
        if (minDis < 10000)
        { // si encontro a algun enemigo dispara
            var s = EnemyBulletManager.instance.giveMeEnemyBullet();
            s.SetPos(shootPos.position).SetDir(Quaternion.LookRotation(transform.forward)).gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (_timer.CheckAndRun()) {
            _timer.Reset();
        }
        
    }

}
