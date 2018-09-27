using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShoot: MonoBehaviour ,BossActions {
    public float time;
    private float _timer = 0;
    private float _timerShoot = 0;
    public float radius;
    public float numberShoot;
    public GameObject bullet;
    public float timeToShoot;
    public float rotationSpeed;
    public float currentAngle=0;
    //EnemyBulletManager bulletManager;
    public Vector3 offsetShoot;
    public float reduceTimeToShoot;
    public float extraRotationSpeed;
    public bool shouldUpgrade;
    Boss boss;
    public float movementSpeed=2;

    private void Start()
    {
        if (shouldUpgrade)
        {
            Upgrade();
        }
        //bulletManager = EnemyBulletManager.instance;
    }


    void BossActions.Begin(Boss bosss)
    {
        boss = bosss;
        _timer = 0;
        _timerShoot = 0;
        boss.SetAnimation("Spin", true);
        
}

    void BossActions.Finish(Boss boss)
    {
        _timer = 0;
        _timerShoot = 0;
        boss.SetAnimation("Spin", false);
    }



    void BossActions.Update(Transform bossTransform,Vector3 playerPosition)
    {
        _timer += Time.deltaTime;
        _timerShoot += Time.deltaTime;
        Shoot(bossTransform.position + offsetShoot);
        //boss.LookAt(playerPosition); //TODO hacelro con lerp
        if (_timerShoot > timeToShoot) {
            Shoot(bossTransform.position+ offsetShoot);
            _timerShoot = 0;
        }
        Vector3 target = new Vector3(boss.player.transform.position.x, boss.transform.position.y, boss.player.transform.position.z);
        boss.transform.LookAt(target);
        boss.transform.position += boss.transform.forward * movementSpeed * Time.deltaTime;

    }

    private void Shoot(Vector3 position)
    {
        print("lala");
        currentAngle += rotationSpeed + UnityEngine.Random.Range(-1,1);

        float shootPositionX = position.x + (float)Math.Cos(currentAngle);
        float shootPositionz = position.z + (float)Math.Sin(currentAngle);
        Vector3 shootPosition = new Vector3(shootPositionX, position.y, shootPositionz);
        Vector3 rotation = shootPosition - position;


        var s = EnemyBulletManager.instance.giveMeEnemyBullet();
        s.SetPos(shootPosition).SetDir(Quaternion.LookRotation(rotation)).gameObject.SetActive(true);

    }

    public void Upgrade()
    {

        rotationSpeed += extraRotationSpeed;
        timeToShoot = timeToShoot- reduceTimeToShoot;
    }
}
