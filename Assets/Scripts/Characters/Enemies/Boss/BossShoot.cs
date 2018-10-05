using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShoot : MonoBehaviour, BossActions
{
    public float time;
    private float _timer = 0;
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
    public bool shouldUpgrade;
    Boss boss;
    public float movementSpeed = 2;
    private Quaternion originalRotation;

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
        originalRotation = boss.columna.transform.localRotation;
        boss.SetAnimation("Spin", true);
    }
    void BossActions.DeleteAll()
    {
        _timer = 0;
        _timerShoot = 0;
        boss.SetAnimation("Spin", false);
    }
    void BossActions.Finish(Boss boss)
    {
        _timer = 0;
        _timerShoot = 0;
        boss.columna.transform.localRotation = originalRotation;
        boss.SetAnimation("Spin", false);
    }



    void BossActions.Update(Transform bossTransform, Vector3 playerPosition)
    {
        _timer += Time.deltaTime;
        _timerShoot += Time.deltaTime;
        currentAngle = (rotationSpeed) * Time.deltaTime  ;
        //print("CurrentAngle" + currentAngle);
        boss.columna.transform.Rotate(Vector3.right, currentAngle);
        //boss.LookAt(playerPosition); //TODO hacelro con lerp
        if (_timerShoot > timeToShoot)
        {
            Shoot(bossTransform.position + offsetShoot, 60);
            Shoot(bossTransform.position + offsetShoot, -60);
            Shoot(bossTransform.position + offsetShoot, 180);
            _timerShoot = 0;
        }
        Vector3 target = new Vector3(boss.player.transform.position.x, boss.transform.position.y, boss.player.transform.position.z);

        Vector3 bossAnglePreLookAt = boss.transform.forward;
        boss.transform.LookAt(target);
        Vector3 bossAnglePostLookAt = boss.transform.forward;

        float bossAngleDeltaDeg = Vector3.SignedAngle(bossAnglePreLookAt, bossAnglePostLookAt, Vector3.up);
        print(bossAngleDeltaDeg);


        boss.columna.transform.Rotate(Vector3.right, currentAngle);
        boss.columna.transform.Rotate(Vector3.right, bossAngleDeltaDeg*1);

        boss.transform.position += boss.transform.forward * movementSpeed * Time.deltaTime;

    }

    private void Shoot(Vector3 position, float angle)
    {

        float shootPositionX = position.x + (float)Math.Cos(Mathf.Deg2Rad * angle + currentAngle);
        float shootPositionz = position.z + (float)Math.Sin(Mathf.Deg2Rad * angle + currentAngle);
        Vector3 shootPosition = new Vector3(shootPositionX, position.y, shootPositionz);
        Vector3 rotation = shootPosition - position;

        var s = EnemyBulletManager.instance.giveMeEnemyBullet();
        s.SetPos(shootPosition).SetDir(Quaternion.LookRotation(rotation)).gameObject.SetActive(true);

        print(s);

    }

    public void Upgrade()
    {

        rotationSpeed += extraRotationSpeed;
        timeToShoot = timeToShoot - reduceTimeToShoot;
    }
}
