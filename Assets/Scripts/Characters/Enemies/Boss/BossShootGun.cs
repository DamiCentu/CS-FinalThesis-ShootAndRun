using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShootGun : MonoBehaviour, BossActions, IPauseable {

    private Transform target;
    public float timeDefault = 2;
    public BossSerpent boss;
    public float stopTime = 0.15f;
    public float angle=15;
    public int nBullet = 4;
    private float offset;

    bool _paused;
    public int extraBullet=2;
    public float upgradeAngle=10;
    private float randomRange=0;
    public float upgradeRandomRange=5;
    public float reduceTime=0f;

    public void OnPauseChange(bool v)
    {
        _paused = v;
    }

    void BossActions.Begin(AbstractBoss boss)
    {
        this.boss = (BossSerpent)boss;
        target = this.boss.player.transform;
        StartCoroutine("ShootCorutine");
    }

    void BossActions.DeleteAll()
    {
    }

    void BossActions.Finish(AbstractBoss boss)
    {
        StopCoroutine("ShootCorutine");
    }

    void BossActions.Update(Transform boss, Vector3 playerPosition)
    {

    }


    IEnumerator ShootCorutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeDefault);

            while (_paused)
                yield return null;

            boss.StopMoving(true);
            yield return new WaitForSeconds(stopTime);
            while (_paused)
                yield return null;

            Shoot();

            yield return new WaitForSeconds(stopTime);
            while (_paused)
                yield return null;
            boss.StopMoving(false);
        }
    }

    private void Shoot()
    {
        Vector3 shootPosition = new Vector3(boss.transform.position.x, boss.player.transform.position.y + offset, boss.transform.position.z);

        Vector3 rotation = boss.player.transform.position - boss.transform.position ;
        rotation.y = 0;

        for (int i = -nBullet+1; i < nBullet; i++)
        {

            var curRot = Quaternion.AngleAxis(angle * i+ UnityEngine.Random.Range(-randomRange, randomRange), Vector3.up) * rotation;

            var s = EnemyBulletManager.instance.giveMeEnemyBullet();
            s.SetPos(shootPosition).SetDir(curRot).gameObject.SetActive(true);

        }

    }
    void BossActions.Upgrade()
    {
        nBullet += extraBullet;
        angle = upgradeAngle;
        randomRange = upgradeRandomRange;
        timeDefault -= reduceTime;
    }
    public void Upgrade()
    {
        nBullet += extraBullet;
        angle = upgradeAngle;
        randomRange = upgradeRandomRange;
        timeDefault = reduceTime;
    }
}
