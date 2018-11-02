using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThrowFire : MonoBehaviour, BossActions
{
      DamagePath damagePath;
    private Transform target;
    private Timer timer;
    public float timeDefault=2;
    public LayerMask maskThatBlockVisionToPlayer;
    public float speed=5;
    public AbstractBoss boss;
    void BossActions.Begin(AbstractBoss boss)
    {
        this.boss = boss;
        target = this.boss.player.transform;
        timer = new Timer(timeDefault, Shoot);
        damagePath = GetComponent<DamagePath>();
    }

    void BossActions.DeleteAll()
    {

    }

    void BossActions.Finish(AbstractBoss boss)
    {

    }

    void BossActions.Update(Transform boss, Vector3 playerPosition)
    {
        if (timer.CheckAndRun()) timer.Reset();
    }

    void BossActions.Upgrade()
    {

    }


    private void Shoot()
    {
        print("shoot");
        Vector3 direct = target.position - boss.transform.position;
        if (Physics.Raycast(transform.position, direct, direct.magnitude, maskThatBlockVisionToPlayer)) {

            print("no disparoo");
            return;
        }
        direct.y = 0;
        damagePath.SpawnDirection(boss.transform.position + new Vector3(0f, 1, 0f), direct.normalized, speed);
    }



}
