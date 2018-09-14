using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMisileAndShoot : MonoBehaviour, BossActions
{
    public float time;
    //private float _timer = 0;
    private float _timerShoot = 0;
    public float radius;
    public float numberShoot;
    public GameObject bullet;
    public float timeToShoot;
    public float rotationSpeed;
    public float currentAngle = 0;
    //EnemyBulletManager bulletManager;
    public Vector3 offsetShoot;
    public float reduceTimeToShoot;
    public float extraRotationSpeed;


    public GameObject missile;
    public float timeBetweenMissiles;
    public float timeToBoom;
    public float maxOffset;
    public float _timer;
    public Transform spawnMissilesPosition;
    public float reduceTimeBetweenMissiles;
    public float reduceTimeToBoom;


    //private void Start()
    //{
        //bulletManager = EnemyBulletManager.instance;
    //}


    void BossActions.Begin(Boss boss)
    {
        //     _timer = 0;
        _timer = 0;
        boss.SetAnimation("ShootAir", true);
        _timerShoot = 0;

    }

    void BossActions.Finish(Boss boss)
    {
     //   _timer = 0;
        _timerShoot = 0;
        boss.SetAnimation("ShootAir", false);
    }



    void BossActions.Update(Transform boss, Vector3 playerPosition)
    {
    //    _timer += Time.deltaTime;
        _timerShoot += Time.deltaTime;

        //boss.LookAt(playerPosition); //TODO hacelro con lerp
        if (_timerShoot > timeToShoot)
        {
            Shoot(boss.position + offsetShoot);
            _timerShoot = 0;
        }
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
        Vector3 destination = new Vector3(xPosition, playerPosition.y + 0.3f, zPosition);
        //Missile mis= new Missile(destination, timeToBoom)
        Missile mis = Instantiate(missile, spawnMissilesPosition.position, Quaternion.FromToRotation(spawnMissilesPosition.position, destination)).GetComponent<Missile>();
        mis.Set(destination, timeToBoom);
    }

    private void Shoot(Vector3 position)
    {

        currentAngle += rotationSpeed + UnityEngine.Random.Range(-1, 1);

        float shootPositionX = position.x + (float)Math.Cos(currentAngle);
        float shootPositionz = position.z + (float)Math.Sin(currentAngle);
        Vector3 shootPosition = new Vector3(shootPositionX, position.y, shootPositionz);
        Vector3 rotation = shootPosition - position;


        var s = EnemyBulletManager.instance.giveMeEnemyBullet();
        s.SetPos(shootPosition).SetDir(Quaternion.LookRotation(rotation)).gameObject.SetActive(true);

    }

    void BossActions.Upgrade()
    {
        rotationSpeed += extraRotationSpeed;
        timeToShoot = timeToShoot - reduceTimeToShoot;
        timeBetweenMissiles -= reduceTimeBetweenMissiles;
        timeToBoom -= reduceTimeToBoom;
    }
}
