using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChasing : MonoBehaviour, BossActions
{

    public float followIntensity = 0.2f;
    public float lifeTime = 3;
    public float timer;


    public void Update()
    {

    }

    void BossActions.Begin(AbstractBoss boss)
    {
        ((BossSerpent)boss).StopMoving(true);
    }

    void BossActions.DeleteAll()
    {

    }

    void BossActions.Finish(AbstractBoss boss)
    {
        ((BossSerpent)boss).StopMoving(false);
    }

    void BossActions.Update(Transform boss, Vector3 playerPosition)
    {

        Vector3 auxDir = playerPosition - this.transform.position;
        this.transform.forward = Vector3.RotateTowards(transform.forward, auxDir, followIntensity, 0.0f);
        this.transform.forward = new Vector3(this.transform.forward.x, 0f, this.transform.forward.z);
        //  this.transform.LookAt(player);

    }

    void BossActions.Upgrade()
    {

    }
}
