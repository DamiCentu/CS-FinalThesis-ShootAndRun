using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossThrowFire : MonoBehaviour, BossActions
{
      DamagePath damagePath;
    private Transform target;

    public float timeDefault=2;
    public LayerMask maskThatBlockVisionToPlayer;
    public float speed=5;
    public BossSerpent boss;
    public float stopTime=0.15f;
    private bool upgraded=false;

    void BossActions.Begin(AbstractBoss boss)
    {
        this.boss = (BossSerpent)boss;
        target = this.boss.player.transform;
        if(upgraded)
            this.boss.SpawnEnemies("FireUpgrade");
        damagePath = GetComponent<DamagePath>();
        StartCoroutine("ThrowFiretCorutine");
    }

    void BossActions.DeleteAll()
    {

    }

    void BossActions.Finish(AbstractBoss boss)
    {
        StopCoroutine("ThrowFiretCorutine");
    }

    void BossActions.Update(Transform boss, Vector3 playerPosition)
    {

    }


    IEnumerator ThrowFiretCorutine() {
        while (true) {

            yield return new WaitForSeconds(timeDefault);
            print("shoot");
            Vector3 direct = target.position - boss.transform.position;
            if (!Physics.Raycast(transform.position, direct, direct.magnitude, maskThatBlockVisionToPlayer))
            {
                boss.StopMoving(true);
                yield return new WaitForSeconds(stopTime);
                print("stopTime:" + stopTime);
                direct.y = 0;
                damagePath.SpawnDirection(boss.transform.position + new Vector3(0f, 1, 0f), direct.normalized, speed);
                yield return new WaitForSeconds(stopTime);
                boss.StopMoving(false);
            }
        }
    }

    void BossActions.Upgrade()
    {

        upgraded = true;
    }

    public void Upgrade()
    {
        upgraded = true;
    }
}
