using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExpansiveWave2 : MonoBehaviour, BossActions
{
    public float width;
    public float minRadius;
    private float _timer;
    public float growSpeed;
    public float extraGrowSpeed;
    float internalRadius = 0;
    float externalRadius = 0;
    Vector3 position = Vector3.zero;
    public LineRenderer line;
    Boss boss;
    Timer t;
    private bool _shouldGrow;
    private bool _shouldStay;
    public float sparkleFactor = 20;
    private float _stayTimer = 0;
    MakeACircle circleMaker;

    void BossActions.Begin(AbstractBoss bosss)
    {
        _timer = 0;
        line.gameObject.SetActive(true);
        boss = bosss as Boss;
        _shouldGrow = true;
        t = new Timer(2, StopGrowing);
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        boss.SpawnEnemies("FirstSpawnBoss");
    }

    private void StopGrowing()
    {
        _shouldGrow = false;
        _shouldStay = true;
        t = new Timer(1.5f, StopStaying);
    }

    void BossActions.Finish(AbstractBoss boss)
    {
        internalRadius = 0;
        externalRadius = 0;
        line.gameObject.SetActive(false);
    }

    void BossActions.DeleteAll()
    {
        ((BossActions)this).Finish(boss);
    }
    void BossActions.Update(Transform bossTransform, Vector3 playerPosition)
    {
        if (t != null) {
            t.CheckAndRun();
        }
        if (_shouldGrow)
        {
            Grow(bossTransform, playerPosition, true);
        }
        else if(!_shouldGrow&& !_shouldStay) 
        {
            Grow(bossTransform, playerPosition, false);
        }
        else if (_shouldStay) {
            Stay();
        }
        float distance = Vector3.Distance(bossTransform.position, playerPosition);
        if (distance > internalRadius && distance < externalRadius)
        {
            boss.player.GetComponent<IHittable>().OnHit(1);
        }

    }

    private void Stay()
    {
     
        _stayTimer += Time.deltaTime;
        var sparkleSin = Math.Sin(_stayTimer * sparkleFactor);
        if (sparkleSin > 0f)
        {
            circleMaker.Show(line, false);
        }
        else {
            circleMaker.Show(line, true);
        }

    }

    private void StopStaying()
    {
        _shouldStay = false;
        _shouldGrow = false;
        circleMaker.Show(line, true);
        t = null;
    }

    void BossActions.Upgrade()
    {
        growSpeed += extraGrowSpeed;
    }
    void Grow(Transform bossTransform, Vector3 playerPosition, bool shouldGrow)
    {
        position = bossTransform.position;
        _timer += Time.deltaTime;
        if (shouldGrow)
        {
            internalRadius = internalRadius + Time.deltaTime * growSpeed;
        }
        else {
            internalRadius = internalRadius - Time.deltaTime * growSpeed;
        }
        externalRadius = internalRadius + width;

         circleMaker = new MakeACircle();
        circleMaker.Make(line, position.x, position.y, position.z, externalRadius, internalRadius, 50);
        circleMaker.Make(line, position.x, position.y, position.z, externalRadius, internalRadius, 50);
    }
}
