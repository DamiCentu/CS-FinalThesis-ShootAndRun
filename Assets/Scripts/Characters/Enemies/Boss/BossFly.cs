using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFly : MonoBehaviour, BossActions
{
     Vector3 startPos;

    public float height = 20;
    public float speed = 10;

    void BossActions.Begin(AbstractBoss boss)
    {
        print("empiezo a volar");
        ((BossSerpent)boss).StopMoving(true);
        startPos = this.transform.position;
    }

    void BossActions.DeleteAll()
    {

    }

    void BossActions.Finish(AbstractBoss boss)
    {
        print("paro de volar");
        boss.transform.position = startPos + new Vector3(0, height, 0);
        ((BossSerpent)boss).StopMoving(false);
        boss.transform.forward = -Vector3.forward;
    }


    void BossActions.Update(Transform boss, Vector3 playerPosition)
    {
        boss.transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
    }

    void BossActions.Upgrade()
    {

    }
}
