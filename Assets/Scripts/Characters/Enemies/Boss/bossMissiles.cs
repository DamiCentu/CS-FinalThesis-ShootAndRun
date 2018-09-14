using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossMissiles : MonoBehaviour, BossActions{

    public Missile Missile;
    public float timeBetweenMissiles;
    public float timeToBoom;
    public float maxOffset;
    public float _timer;
    public Transform spawnMissilesPosition;
    public float reduceTimeBetweenMissiles;
    public float reduceTimeToBoom;
    public bool shouldUpgrade;
    Boss boss;

    void BossActions.Begin(Boss boss1)
    {
        boss = boss1;
        if (shouldUpgrade) {
            Upgrade();
        }
        _timer = 0;
        boss.SetAnimation("ShootAir", true);
    }

    void BossActions.Finish(Boss boss)
    {
        boss.SetAnimation("ShootAir", false);
    }

    void BossActions.Update(Transform boss, Vector3 playerPosition)
    {
        _timer += Time.deltaTime;
        if (_timer > timeBetweenMissiles)
        {
            DropMissile(playerPosition);
            _timer = 0;
        }
    }

    private void DropMissile(Vector3 playerPosition)
    {
        float xPosition = playerPosition.x + UnityEngine.Random.Range(-maxOffset, maxOffset);
        float zPosition = playerPosition.z + UnityEngine.Random.Range(-maxOffset, maxOffset);
        Vector3 destination = new Vector3(xPosition, playerPosition.y+0.3f, zPosition);
        //Missile mis= new Missile(destination, timeToBoom)
        Missile mis=  Instantiate(Missile, spawnMissilesPosition.position, Quaternion.FromToRotation(spawnMissilesPosition.position,destination));
        mis.Set(destination, timeToBoom);
    }

    public void Upgrade()
    {
        timeBetweenMissiles -= reduceTimeBetweenMissiles;
        timeToBoom -= reduceTimeToBoom;
        boss.SpawnEnemies("MissileUpgrade");
    }
}
