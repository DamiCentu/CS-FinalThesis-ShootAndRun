using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDirectedMisil : MonoBehaviour, BossActions
{
    private BossSerpent boss;
    private Transform target;
    List<GameObject> misils;
    public int nBullet;
    public float angle;
    private float offset=1;
    public float offsetXz= 2;
    public GameObject prefabMisil;

    void BossActions.Begin(AbstractBoss boss)
    {
        this.boss = (BossSerpent)boss;
        target = this.boss.player.transform;
        Shoot();
    }

    private void Shoot()
    {
        misils = new List<GameObject>();


        Vector3 rotation = boss.player.transform.position - boss.transform.position;
        rotation.y = 0;
        for (int i = -nBullet + 1; i < nBullet; i++)
        {
            Vector3 shootPosition = new Vector3(boss.transform.position.x, boss.player.transform.position.y + offset, boss.transform.position.z+ offset*i);
            var curRot = Quaternion.AngleAxis(angle * i, Vector3.up) * rotation;


            var s = Instantiate(prefabMisil, shootPosition,Quaternion.Euler(new Vector3 (curRot.x, 0,-curRot.z)) );
            s.transform.forward = curRot;
            misils.Add(s);
        }

    }

    void BossActions.DeleteAll()
    {
        foreach (var item in misils)
        {
            if (item != null)   Destroy(item.gameObject);
        }
    }

    void BossActions.Finish(AbstractBoss boss)
    {

    }


    void BossActions.Update(Transform boss, Vector3 playerPosition)
    {

    }

    void BossActions.Upgrade()
    {

    }
}
