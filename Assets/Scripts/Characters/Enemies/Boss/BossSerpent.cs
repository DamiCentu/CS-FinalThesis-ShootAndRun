using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSerpent : AbstractBoss {
    public float speed=1;
    public BossThrowFire actionThrowFire;
    public BossShootGun actionShootGun;
    public BossLaser actionLaser;
    public enum Type {Left, Right };
    public Type type;
    MovingPlatform moving;
    public Transform shootPosition;

    void Awake()
    {
        SetActions();
        Config();
        moving= GetComponent<MovingPlatform>();
    }

    private void SetActions()
    {

        stageActions.Add(new List<BossActions>());
        if (type == Type.Left) {
            stageActions[0].Add(actionThrowFire);
        }
        if (type == Type.Right)
        {
            stageActions[0].Add(actionShootGun);
            stageActions[0].Add(actionLaser);
        }
    }

    new void Update()
    {
        base.Update();

        this.transform.position += this.transform.right * speed * Time.deltaTime;
    }

    internal void StopMoving(bool v)
    {
        moving.Move(!v);
    }
}
