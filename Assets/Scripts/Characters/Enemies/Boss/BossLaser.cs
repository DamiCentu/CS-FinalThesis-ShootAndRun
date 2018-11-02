using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour, BossActions {
    public int laserMaxDistance=20;
    public LayerMask maskToCollide;
    public BossSerpent boss;
    public LineRenderer line;

    void BossActions.Begin(AbstractBoss boss)
    {
        print("entre");
        this.boss = (BossSerpent)boss;
        line.gameObject.SetActive(false);
    }

    void BossActions.DeleteAll()
    {
        line.gameObject.SetActive(false);
    }

    void BossActions.Finish(AbstractBoss boss)
    {
        line.gameObject.SetActive(false);
    }

    void BossActions.Update(Transform bossi, Vector3 playerPosition)
    {
        line.gameObject.SetActive(true);

        RaycastHit rh;
        print("boss: " + this.boss);
        print("shootPos: "+ this.boss.shootPosition);
        print("shootPos pos: " + this.boss.shootPosition.position);
        if (Physics.Raycast(this.boss.shootPosition.position, Vector3.left, out rh, laserMaxDistance, maskToCollide))
        {
            if (rh.collider.gameObject.layer == 8)
            {
                rh.collider.GetComponent<IHittable>().OnHit(0);
            }

            line.SetPosition(0, this.boss.shootPosition.position);
            line.SetPosition(1, rh.point);


        }
        else
        {
            var a = Vector3.left * laserMaxDistance + this.boss.shootPosition.position;
            line.SetPosition(0, this.boss.shootPosition.position);
            line.SetPosition(1, a);

        }

    }

    void BossActions.Upgrade()
    {

    }



}
