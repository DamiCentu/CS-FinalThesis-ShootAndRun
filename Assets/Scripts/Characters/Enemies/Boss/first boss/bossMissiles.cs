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
    public LayerMask layer;
    public float movementSpeed=5;
    private List<Missile> misiles = new List<Missile>();


    void BossActions.Begin(AbstractBoss boss1)
    {
        misiles = new List<Missile>();
        boss =(Boss) boss1;
        if (shouldUpgrade) {
            Upgrade();
        }
        _timer = 0;
        boss.SetAnimation("ShootAir", true);
    }
    void BossActions.DeleteAll()
    {
        ((BossActions)this).Finish(boss);
        foreach (var item in misiles)
        {
            if (item != null) Destroy(item);
        }
    }
    void BossActions.Finish(AbstractBoss boss)
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
        boss.transform.LookAt(playerPosition);
        boss.transform.position += boss.transform.forward * movementSpeed * Time.deltaTime;
    }

    private void DropMissile(Vector3 playerPosition)
    {
   //     float xPosition = playerPosition.x + UnityEngine.Random.Range(-maxOffset, maxOffset);
     //   float zPosition = playerPosition.z + UnityEngine.Random.Range(-maxOffset, maxOffset);
        Vector3 destination = new Vector3(playerPosition.x, playerPosition.y+0.3f, playerPosition.z);
        //Missile mis= new Missile(destination, timeToBoom)
        Vector3 destinationInBoundries = Utility.RandomVector3InRadiusCountingBoundariesInAnyDirection(destination, 3f,layer);
        Missile mis=  Instantiate(Missile, spawnMissilesPosition.position, Quaternion.FromToRotation(spawnMissilesPosition.position, destinationInBoundries));
        mis.Set(destinationInBoundries, timeToBoom);
        misiles.Add(mis);
    }

    public void Upgrade()
    {
        timeBetweenMissiles -= reduceTimeBetweenMissiles;
        timeToBoom -= reduceTimeToBoom;
        boss.SpawnEnemies("MissileUpgrade");
    }
}
