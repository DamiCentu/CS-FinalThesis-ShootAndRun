using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJump : MonoBehaviour, BossActions
{
    public float timeToStartFall=3f;
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
    public LayerMask layerToDetect;

    void BossActions.Begin(Boss boss)
    {
        boss.SetAnimation("Jump", true);
        timer = new Timer(timeToStartFall,StartFall);
        timerMark = new Timer(TimeToMark, Mark);
        this.boss= boss;
        boss.transform.LookAt(boss.player.transform.position);
        Vector3 position = new Vector3(boss.player.transform.position.x, boss.transform.position.y, boss.player.transform.position.z);
        positionToAim= Utility.RandomVector3InRadiusCountingBoundariesInAnyDirection(position, 5f, layerToDetect);
        boss.col.enabled = false;

    }

    void BossActions.Finish(Boss boss)
    {
        mark.SetActive(false);
        boss.SetAnimation("Jump", false);
        //print("Le pegue");
        Collider[] hitColliders = Physics.OverlapSphere(boss.transform.position, radius, hittableLayer, QueryTriggerInteraction.Collide);
        foreach (Collider item in hitColliders)
        {
            IHittable hittable = item.gameObject.GetComponent<IHittable>();
            if (hittable != null) hittable.OnHit(damage);
        }
        boss.col.enabled = true;
        //  boss.gameObject.GetComponent<Collider>().gameObject.SetActive(true);
    }

    void BossActions.DeleteAll()
    {
        if(mark!=null)   mark.SetActive(false);
        boss.SetAnimation("Jump", false);
        boss.col.enabled = true;
    }

    private void Mark()
    {
        boss.transform.position = positionToAim;
        mark= Instantiate(markPrefab, positionToAim, Quaternion.LookRotation(Vector3.up));
        timerMark = null;
    }


    void BossActions.Update(Transform boss, Vector3 playerPosition)
    {
        if(timer!=null)
            timer.CheckAndRun();
        if (timerMark != null)
            timerMark.CheckAndRun();

    }

    void StartFall() {
        boss.SetAnimation("Jump", false);
        timer = null;
    }

    void BossActions.Upgrade()
    {

    }
}
