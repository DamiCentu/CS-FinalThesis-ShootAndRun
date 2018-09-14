using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExpansiveWave : MonoBehaviour, BossActions {
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

    void BossActions.Begin(Boss bosss)
    {
        _timer = 0;
        line.gameObject.SetActive(true);
        boss = bosss;
    }

    void BossActions.Finish(Boss boss)
    {
        internalRadius = 0;
        externalRadius = 0;
        line.gameObject.SetActive(false);
    }


    void BossActions.Update(Transform bossTransform, Vector3 playerPosition)
    {
        position = bossTransform.position;
        _timer += Time.deltaTime;
         internalRadius= minRadius+ _timer * growSpeed;
         externalRadius = internalRadius + width;
        // Collider[] hitCollidersInternal = Physics.OverlapSphere(boss.position, internalRadius);
        // Collider[] hitCollidersExternal = Physics.OverlapSphere(boss.position, externalRadius);
        float distance = Vector3.Distance(bossTransform.position, playerPosition);

        MakeACircle circleMaker= new MakeACircle();
        circleMaker.Make(line, position.x, position.y, position.z, externalRadius, internalRadius, 50);
        circleMaker.Make(line, position.x, position.y, position.z, externalRadius, internalRadius, 50);
        //circleMaker.Make(line, 0, 0, 0, 100, 100, 40);

        if (distance > internalRadius && distance < externalRadius) {
            //print("le pegue");
            boss.player.GetComponent<IHittable>().OnHit(1);
        }

    }

    void BossActions.Upgrade()
    {
        growSpeed += extraGrowSpeed;
    }
}
