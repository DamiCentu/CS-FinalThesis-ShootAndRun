using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSerpent : AbstractBoss,IHittable {
    public float speed=1;
    public BossThrowFire actionThrowFire;
    public BossShootGun actionShootGun;
    public BossLaser actionLaser;
    public BossDirectedMisil actionDirectedMisil;
    public BossFly actionBossFlyup;
    public BossFly actionBossFlyDown;
    public enum Type {Left, Right,Up };
    public Type type;
    MovingPlatform moving;
    public Transform shootPosition;
    private bool dead=false;

    void Awake()
    {
        SetActions();
        Config();
        moving= GetComponent<MovingPlatform>();
        shouldChangeStage = false;
    }

    private void Start()
    {
        EventManager.instance.SubscribeEvent("EvolveBoss2", Evolve);
    }

    private void SetActions()
    {

        stageActions.Add(new List<BossActions>());
        stageActions.Add(new List<BossActions>());
        stageActions.Add(new List<BossActions>());
        if (type == Type.Left) {
            stageActions[0].Add(actionThrowFire);
            stageActions[0].Add(actionDirectedMisil);
        }
        if (type == Type.Right)
        {
            stageActions[0].Add(actionShootGun);
            stageActions[0].Add(actionLaser);
        }
        if (type == Type.Up)
        {
            stageActions[1].Add(actionBossFlyup);
            stageActions[1].Add(actionBossFlyDown);
            stageActions[1].Add(actionShootGun);
            stageActions[1].Add(actionLaser);
            stageActions[1].Add(actionThrowFire);
            stageActions[1].Add(actionDirectedMisil);
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
    void IHittable.OnHit(int damage)
    {
        AbstractOnHitWhiteAction();
        if (type == Type.Up)
        {
            GetDamage(damage);
        }
        else
        {
            life -= damage;
            if (life <= 0 && !dead) {
                dead = true;
                Destroy(this.gameObject);
                EventManager.instance.ExecuteEvent("EvolveBoss2");

            }
        }
    }


    private void Evolve(object[] parameterContainer)
    {
        if (!dead) {
            print("evolveee");
            type = Type.Up;
            stageActions.Clear();
            SetActions();
            base.Evolve();

            moving.ChangeStartPosition(GameObject.Find("BossEvolveSpaenPoint").transform.position);
            moving.direction = MovingPlatform.Direction.Right;
            moving.width = 10;
        }
    }

}
