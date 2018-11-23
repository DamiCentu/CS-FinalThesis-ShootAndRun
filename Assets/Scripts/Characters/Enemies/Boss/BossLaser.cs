using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour, BossActions {
    public int laserMaxDistance=20;
    public LayerMask maskToCollide;
    public BossSerpent boss;
    public LineRenderer line;
    public bool upgrade;
    public float speed=0.01f;
    Vector3 laserDir;
    private float timer;
    public GameObject mark1;
    public GameObject mark2;
    public GameObject mark3;
    List<GameObject> misils;
    public int nBullet;
    public float angle;
    private float offset = 1;
    public float offsetXz = 2;
    public GameObject prefabMisil;
    private float stopTime = 2;

    void BossActions.Begin(AbstractBoss boss)
    {
        print("entre");
        this.boss = (BossSerpent)boss;
        line.gameObject.SetActive(false);
     //   boss.transform.forward = -Vector3.forward;
      //  laserDir = -Vector3.forward;
        MarkActive(true);
        timer = 1f;
        if (upgrade) {

            StartCoroutine("WaitShoot");
            misils = new List<GameObject>();
        }
        this.boss.StopMoving(false);
    }

    void BossActions.DeleteAll()
    {
        if (line != null && line.gameObject != null) {
            line.gameObject.SetActive(false);
        }
        MarkActive(false);
        foreach (var item in misils)
        {
            if (item != null) Destroy(item.gameObject);
        }
    }

    IEnumerator WaitShoot()
    {
        yield return new WaitForSeconds(stopTime);
        Shoot();


    }
    private void Shoot()
    {
        Vector3 rotation = boss.player.transform.position - boss.transform.position;
        rotation.y = 0;
        for (int i = -nBullet + 1; i < nBullet; i++)
        {
            Vector3 shootPosition = new Vector3(boss.transform.position.x, boss.player.transform.position.y + offset, boss.transform.position.z + offset * i);
            var curRot = Quaternion.AngleAxis(angle * i, Vector3.up) * rotation;


            var s = Instantiate(prefabMisil, shootPosition, Quaternion.Euler(new Vector3(curRot.x, 0, -curRot.z)));
            s.transform.forward = curRot;
            misils.Add(s);
        }

    }

    void BossActions.Finish(AbstractBoss boss)
    {
        line.gameObject.SetActive(false);
        MarkActive(false);
    }

    private void MarkActive(bool v) {
        mark1.SetActive(v);
        mark2.SetActive(v);
        mark3.SetActive(v);
    }

    void BossActions.Update(Transform bossi, Vector3 playerPosition)
    {
        if (timer < 0)
        {
            MarkActive(false);
            if(!upgrade)
                Laser(Vector3.left);
            else {
                Laser(-Vector3.forward);
            }

        }
        else {
            timer -= Time.deltaTime;
        }
    }

    private void Laser( Vector3 direction)
    {
        line.gameObject.SetActive(true);

        RaycastHit rh;
        print("disparo");
  //      if (Physics.Raycast(this.boss.shootPosition.position+ new Vector3(0,1,0), direction, out rh, laserMaxDistance, maskToCollide))
    //    {
         if (Physics.SphereCast(this.boss.shootPosition.position, 1.5f, direction, out rh, 20f, maskToCollide))
        {
                print("me choque con algo");
            if (rh.collider.gameObject.layer == 8)
            {
                rh.collider.GetComponent<IHittable>().OnHit(1);
            }

            line.SetPosition(0, this.boss.shootPosition.position);
            line.SetPosition(1, rh.point);


        }
        else
        {
            var a = direction * laserMaxDistance + this.boss.shootPosition.position;
            line.SetPosition(0, this.boss.shootPosition.position);
            line.SetPosition(1, a);
        }
    }

    void BossActions.Upgrade()
    {
        upgrade = true;
    }
    public void Upgrade() {
       upgrade = true;
    }


}
