using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSerpent : AbstractBoss,IHittable {
    public float speed=1;
    public BossThrowFire actionThrowFire;
    public BossShootGun actionShootGun;
    public BossLaser actionLaser;
    public BossLaser actionLaserUpgraded;
    public BossDirectedMisil actionDirectedMisil;
    public BossFly actionBossFly;
    public enum Type {Left, Right,Up };
    public Type type;
    public MovingPlatform moving;
    public Transform shootPosition;
    private bool dead=false;
    public bool inmortal=false;

    void Awake()
    {
        SetActions();
        Config();
        moving= GetComponent<MovingPlatform>();

        shouldChangeStage = false;

        this.transform.Rotate(0, 90, 0);
        SetAnimation("fly", true);
    }


    private void Start()
    {
        EventManager.instance.SubscribeEvent("EvolveBoss2", SetEvolve);
        moving.Move(false);
        UIManager.instance.bossLifeBar0.SetActive(false);
    }

    private void SetEvolve(object[] parameterContainer)
    {
        shouldEvolve = true;
        shouldChangeStage = true;
        UIManager.instance.ActivateBar(false, numberBoss);
        inmortal = true;
    }

    private void SetActions()
    {

        stageActions.Add(new List<BossActions>());
        stageActions.Add(new List<BossActions>());
        stageActions.Add(new List<BossActions>());
        if (type == Type.Left) {
            stageActions[0].Add(actionThrowFire);
            stageActions[0].Add(actionDirectedMisil);
            numberBoss = 1;
            UIManager.instance.ActivateBar(true, 1);
        }
        if (type == Type.Right)
        {
            stageActions[0].Add(actionShootGun);
            stageActions[0].Add(actionLaser);
            numberBoss = 2;
            UIManager.instance.ActivateBar(true, 2);
        }
        if (type == Type.Up)
        {
            stageActions[1].Add(actionBossFly);

            BossShootGun auxshootGun = actionShootGun;
            actionShootGun.Upgrade();
            stageActions[1].Add(auxshootGun);
             BossThrowFire auxfire = actionThrowFire;
            auxfire.Upgrade();
            stageActions[1].Add(auxfire);
            stageActions[1].Add(actionLaserUpgraded);

            numberBoss = 0;
        }
    }


    internal void StopMoving(bool v)
    {
        moving.Move(!v);
        if (v) {
            SetAnimation("left", false);
            SetAnimation("right", false);
        }
        else if (moving.speed > 0) {
            SetAnimation("left", true);
        }
        else if (moving.speed < 0)
        {
            SetAnimation("right", true);
        }
    }
    void IHittable.OnHit(int damage)
    {
        if (inmortal) return;
        AbstractOnHitWhiteAction();
        if (type == Type.Up)
        {
            GetDamage(damage);
        }
        else
        {
            life -= damage;
            UpdateBossLife();
            if (life <= 0 && !dead) {
                dead = true;
                actualAction.Finish(this);
                Destroy();
                //this.gameObject.SetActive(false);
                EventManager.instance.ExecuteEvent("EvolveBoss2");

            }
        }
    }

    public void SpawnEnemies(string nameParent, EnemiesManager.TypeOfEnemy type = EnemiesManager.TypeOfEnemy.Normal)
    {
        GameObject spawns = GameObject.Find(nameParent);
        var positions = spawns.GetComponentsInChildren<EnemySpawner>();
        foreach (var p in positions)
        {
            Vector3 position = Utility.SetYInVector3(p.transform.position, 1);

            _actualSectionNode.SpawnEnemyAtPointNoCuentaParaTerminarNodoPeroTieneIntegracion(position, type);
        }
    }
    protected override void Evolve()
    {
        if (!dead) {
            print(type);
            numberBoss = 0;
            print("evolveee");
            type = Type.Up;
            stageActions.Clear();
            SetActions();
            base.Evolve();

        }
    }

    internal void Destroy()
    {
        this.ResetBossLife();
        this.DeleteAll();
        EventManager.instance.UnsubscribeEvent("EvolveBoss2", SetEvolve);
        Destroy(this.gameObject);
    }

    protected override void FinishiIntro()
    {
        base.FinishiIntro();
        SetAnimation("fly", false);
    }
}
