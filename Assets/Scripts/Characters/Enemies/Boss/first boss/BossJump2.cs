using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJump2 : MonoBehaviour, BossActions
{
    public float timeToStartFall = 3f;
    public Boss boss;
    Timer timer;
    Timer timerMark;
    public float TimeToMark = 2f;
    Vector3 positionToAim;
    public float radius = 4;
    public LayerMask hittableLayer;
    int damage = 1;
    public GameObject markPrefab;
    public Vector3 offsetMark;
    GameObject mark;
    private BoxCollider bossCollider;

    void BossActions.Begin(AbstractBoss boss)
    {
        boss.SetAnimation("Jump", true);
        timer = new Timer(timeToStartFall, StartFall);
        timerMark = new Timer(TimeToMark, Mark);
        this.boss = (Boss) boss;
        boss.transform.LookAt(boss.player.transform.position);
        positionToAim = new Vector3(boss.player.transform.position.x, boss.transform.position.y, boss.player.transform.position.z);
        this.boss.col.enabled = false;

    }
    void BossActions.DeleteAll()
    {
        mark.SetActive(false);
        boss.SetAnimation("Jump", false);
        boss.col.enabled = true;
    }
    void BossActions.Finish(AbstractBoss boss)
    {
        this.boss = (Boss)boss;
        mark.SetActive(false);
        boss.SetAnimation("Jump", false);
        //print("Le pegue");
        Collider[] hitColliders = Physics.OverlapSphere(boss.transform.position, radius, hittableLayer, QueryTriggerInteraction.Collide);
        foreach (Collider item in hitColliders)
        {
            IHittable hittable = item.gameObject.GetComponent<IHittable>();
            if (hittable != null) hittable.OnHit(damage);
        }
        this.boss.col.enabled = true;
        //  boss.gameObject.GetComponent<Collider>().gameObject.SetActive(true);
    }

    private void Mark()
    {
        boss.transform.position = positionToAim;
        mark = Instantiate(markPrefab, positionToAim, Quaternion.LookRotation(Vector3.up));
        timerMark = null;
    }


    void BossActions.Update(Transform boss, Vector3 playerPosition)
    {
        if (timer != null)
            timer.CheckAndRun();
        if (timerMark != null)
            timerMark.CheckAndRun();

    }

    void StartFall()
    {
        boss.SetAnimation("Jump", false);
        timer = null;
    }

    void BossActions.Upgrade()
    {

    }
}
