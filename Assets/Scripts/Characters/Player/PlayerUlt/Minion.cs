using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour , IPauseable {
    private Vector3 _target;
    public float radius;
    public LayerMask hittableLayers;
    public Transform shootPos;
    public float minTimeTiShoot=0.5f;
    Timer _timer;
    // Use this for initialization

    bool _paused;
    public void OnPauseChange(bool v)
    {
        _paused = v;
    }
    void Start () {
         _timer = new Timer(minTimeTiShoot, Shoot);
    }

    private void Shoot()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, hittableLayers);
        int i = 0;
        float minDis = 10000;

        var filteredColliders = new List<Collider>(hitColliders)
            .FindAll(x => x.gameObject != null)
            .FindAll(x => {
                var enemyTurret = x.gameObject.GetComponent<EnemyTurretBehaviour>();
                return enemyTurret == null || (enemyTurret != null && enemyTurret._currentTypeOfTurret.GetType() != typeof(LaserTurretStrategy));
                });

        foreach (var collider in filteredColliders)
        {
            float dis = Vector3.Distance(transform.position, collider.transform.position);
            if (dis < minDis)
            {
                minDis = dis;
                this.transform.LookAt(collider.transform.position);
            }

        }

        if (minDis < 10000)
        { // si encontro a algun enemigo dispara
            NormalBullet b = BulletManager.instance.GetBulletFromPool();
            BulletManager.instance.SetBullet(b, shootPos.position, transform.forward);
        }
    }

    private void Update()
    {
        if (_paused)
            return;

        if (_timer.CheckAndRun()) {
            _timer.Reset();
        }
        
    }

}
