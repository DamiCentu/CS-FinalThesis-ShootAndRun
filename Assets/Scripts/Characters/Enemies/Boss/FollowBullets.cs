using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBullets : EnemyBulletBehaviour
{
    public float followIntensity=0.2f;
    public float lifeTime = 3;
    public float timer;

    public void Awake()
    {
        timer = lifeTime;
    }

    public override void OnTriggerEnter(Collider c)
    {

    }
    public override void Update()
    {
        if (_paused)
            return;

        timer -= Time.deltaTime;
        base.Update();
        Transform player = EnemiesManager.instance.player.transform;


        Vector3 auxDir = player.position - this.transform.position;
        this.transform.forward = Vector3.RotateTowards(transform.forward, auxDir, followIntensity, 0.0f);
        this.transform.forward = new Vector3(this.transform.forward.x, 0f, this.transform.forward.z);
        //  this.transform.LookAt(player);
        if (timer <= 0) {
            Destroy(this.gameObject);
        }
    }

    internal void ExtraSpeed(float extraSpeed)
    {
        bulletSpeed += extraSpeed;
    }
}
