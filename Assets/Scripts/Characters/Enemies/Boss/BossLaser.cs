using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour, BossActions {
    public int laserMaxDistance=20;
    public LayerMask maskToCollide;
    public BossSerpent boss;
    public LineRenderer line;
    private bool upgrade;
    public float speed=0.01f;
    Vector3 laserDir;
    private float timer;
    public GameObject mark1;
    public GameObject mark2;
    public GameObject mark3;

    void BossActions.Begin(AbstractBoss boss)
    {
        print("entre");
        this.boss = (BossSerpent)boss;
        line.gameObject.SetActive(false);
     //   boss.transform.forward = -Vector3.forward;
        laserDir = -Vector3.forward;
        MarkActive(true);
        timer = 1f;
    }

    void BossActions.DeleteAll()
    {
        if (line != null && line.gameObject != null) {
            line.gameObject.SetActive(false);
        }
        MarkActive(false);
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
            if (!upgrade)
            {
                Laser(Vector3.left);
            }
            else
            {

                Vector3 auxDir = playerPosition - boss.transform.position;
                auxDir.y = 0;
                Vector3 auxDir2 = Vector3.RotateTowards(laserDir, auxDir, speed * Time.deltaTime, 0.0f);
                laserDir = new Vector3(auxDir2.x, 0f, auxDir2.z);

                Laser(laserDir);
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
