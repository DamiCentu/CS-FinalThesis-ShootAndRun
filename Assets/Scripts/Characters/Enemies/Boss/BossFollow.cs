using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFollow : MonoBehaviour,BossActions
{

    public LayerMask blockEnemyViewToPlayer;

    Flocking _flocking;
    Animator _anim;

    FollowPathBehaviour _followPathBehaviour;

    void BossActions.Begin(Boss boss)
    {

    }

    void BossActions.Finish(Boss boss)
    {
        
    }

    void BossActions.Update(Transform boss, Vector3 playerPosition)
    {
        boss.transform.LookAt(playerPosition);
        boss.transform.position += boss.transform.forward * 10 * Time.deltaTime;
    }

    void BossActions.Upgrade()
    {

    }
}
