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
    void BossActions.Begin(AbstractBoss boss)
    {
        print("empiezo a volar");
        print("boss type");
        print(((BossSerpent)boss).type);
        this.boss = (BossSerpent)boss;
        ((BossSerpent)boss).StopMoving(true);
        boss.SetAnimation("fly", true);
        print("llll");
        //  startPos = boss.transform.position;
        StartCoroutine("WaitToGetDown");
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
        boss.transform.forward = -Vector3.forward;
        
    }

    void BossActions.Finish(AbstractBoss boss)
    {
        ((BossSerpent)boss).StopMoving(false);
        

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
