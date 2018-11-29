using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : AbstractBoss,IHittable
{

    public BossExpansiveWave bossExpansiveWave;
    public BossExpansiveWave2 bossExpansiveWave2;
    public BossShoot bossShoot;
    public BossShoot bossShootUpgrade;
    public bossMissiles bossMissile;
    public bossMissiles bossMissileUpgrade;
    public BossJump bossJump;
    public BossJump bossJump1;
    public BossFollow bossFollow;

    public BossMisileAndShoot bossMisileAndShoot;
    public BossCharge bossCharge;
    public BossCharge bossChargeUpgrade;



    private BossActions actualAction;

    bool inmortal=false;
    public SimpleHealthBar helthBar;


    //public ParticleSystem DeadParticle;
    public LayerMask layerThatDontAffectCharge;

    public BoxCollider col;
    public GameObject shield1;
    public GameObject shield2;
    public GameObject columna;
    public Transform ShootPos1;
    public Transform ShootPos2;
    public Transform ShootPos3;

    void Awake()
    {
        SetActions();
        Config();
        numberBoss = 0;
        shield1= GameObject.Find("ShieldBoss1");
        shield2=GameObject.Find("ShieldBoss2");


        col = this.GetComponent<BoxCollider>();
        UpdateBossLife();
        meshRends = GetComponentsInChildren<Renderer>();
        ChangeShaderValue("_SegundaFase", 0);

        transform.LookAt(player.transform.position);
    }

    private void SetActions()
    {

        stageActions.Add(new List<BossActions>());
        stageActions.Add(new List<BossActions>());
        stageActions.Add(new List<BossActions>());

        stageActions[0].Add(bossExpansiveWave);
        //      stageActions[0].Add(bossMissile); duraba 7
        stageActions[0].Add(bossCharge);
        stageActions[0].Add(bossCharge);
        stageActions[0].Add(bossCharge);
        stageActions[0].Add(bossShoot);
        stageActions[0].Add(bossMissile);
        stageActions[0].Add(bossJump);

        stageActions[1].Add(bossJump);
        stageActions[1].Add(bossExpansiveWave2);
        stageActions[1].Add(bossJump);
        stageActions[1].Add(bossExpansiveWave2);
        stageActions[1].Add(bossMissileUpgrade);
        stageActions[1].Add(bossShootUpgrade);
        stageActions[1].Add(bossChargeUpgrade);
        UIManager.instance.ActivateBar(true, 0);
    }


    private void UpgradeAction()
    {
        foreach (var action in actions)
        {
            action.Upgrade();
        }
    }


    //IEnumerator Dead() {
    //    EventManager.instance.ExecuteEvent(Constants.CAMERA_STATIONARY, new object[] { transform.position });
    //    yield return new WaitForSeconds(2);

    //    while (_paused)
    //        yield return null;

    //    EventManager.instance.ExecuteEvent(Constants.PARTICLE_SET, new object[] { Constants.PARTICLE_BOSS_EXPLOTION_NAME, transform.position });
    //    EventManager.instance.ExecuteEvent(Constants.BOSS_DESTROYED , new object[] { bossCameraShakeTime });
    //    Destroy(this.gameObject);
    //}

    bool isDead() {
        return life <= 0;
    }

    public void SetInmortal(bool v) {
        inmortal = v;
    }
    public void SpawnEnemies(string nameParent, EnemiesManager.TypeOfEnemy type = EnemiesManager.TypeOfEnemy.Normal) {
        GameObject spawns = GameObject.Find(nameParent);
        var positions = spawns.GetComponentsInChildren<EnemySpawner>();
        foreach (var p in positions)
        {
            Vector3 position = Utility.SetYInVector3(p.transform.position,1); 
            
            _actualSectionNode.SpawnEnemyAtPointNoCuentaParaTerminarNodoPeroTieneIntegracion(position, type);
        }
    }
    private void OnTriggerEnter(Collider c)
    {
        //if (c.gameObject.layer != 12 && c.gameObject.layer != 13 && c.gameObject.layer != 0) {//enemy //powerup//ddefault
        if (layerThatDontAffectCharge != (layerThatDontAffectCharge | (1 << c.gameObject.layer)))
        {
            EventManager.instance.ExecuteEvent(Constants.CHARGER_CRUSH);
            Player p = c.gameObject.GetComponent<Player>();
            if (p != null)
            {
                p.OnHit(1);
            }
            //print("me choque");
        }

    }

    void IHittable.OnHit(int damage)
    {
        if (!inmortal)
        {
            AbstractOnHitWhiteAction();
            GetDamage(damage);

        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        //if (c.gameObject.layer != 12 && c.gameObject.layer != 13 && c.gameObject.layer != 0) {//enemy //powerup//ddefault
        if (layerThatDontAffectCharge != (layerThatDontAffectCharge | (1 << collision.gameObject.layer)))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            if (p != null)
            {
                p.OnHit(1);
            }
            //print("me choque");
            EventManager.instance.ExecuteEvent(Constants.CHARGER_CRUSH);

        }
        
    }
}
