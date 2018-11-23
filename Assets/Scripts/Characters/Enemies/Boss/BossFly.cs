using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFly : MonoBehaviour, BossActions
{
     Vector3 startPos;

    public float height = 20;
    public float speed = 10;
    public float time = 2f;
    BossSerpent boss;
    private bool stop=false;

    void BossActions.Begin(AbstractBoss boss)
    {
        if (!stop) {

            this.boss = (BossSerpent)boss;
            ((BossSerpent)boss).StopMoving(true);
            boss.SetAnimation("fly", true);
            StartCoroutine("WaitToGetDown");

        }
    }
    IEnumerator WaitToGetDown()
    {
        print("lalala");
        yield return new WaitForSeconds(time);
        GetDown();
    }

    private void GetDown()
    {
        print("lololo");
        boss.SetAnimation("fly", false);
        var newPos = GameObject.Find("BossEvolveSpaenPoint").transform.position;
        boss.moving.ChangeStartPosition(newPos);
        boss.transform.position = newPos;
        boss.moving.direction = MovingPlatform.Direction.Right;
        boss.moving.width = 10;
        boss.moving.speed = 0.75f;
        boss.transform.forward = -Vector3.forward;
        UIManager.instance.bossLifeBar.SetActive(true);

    }

    void BossActions.Finish(AbstractBoss boss)
    {
        if (!stop)
        {
            ((BossSerpent)boss).StopMoving(false);
            boss.StopFlying();
            stop = true;
        }

    }

    void BossActions.Upgrade()
    {

    }

    void BossActions.DeleteAll()
    {

    }

    void BossActions.Update(Transform boss, Vector3 playerPosition)
    {

    }
}
