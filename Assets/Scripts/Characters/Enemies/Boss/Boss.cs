using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : AbstractEnemy, IHittable{
    public float introTime;
    public BossExpansiveWave bossExpansiveWave;
    public BossExpansiveWave2 bossExpansiveWave2;
    public BossShoot bossShoot;
    public BossShoot bossShootUpgrade;
    public bossMissiles bossMissile;
    public bossMissiles bossMissileUpgrade;
    public BossJump bossJump;
    public BossJump bossJump1;
    public BossFollow bossFollow;
    // public bossMissiles bossMissile1;
    public BossMisileAndShoot bossMisileAndShoot;
    public BossCharge bossCharge;
    public BossCharge bossChargeUpgrade;
    private List<BossActions> actions= new List<BossActions>();
    private List<List<BossActions>> stageActions = new List<List<BossActions>>();
    Animator an;
    public int life = 50;
    int maxLife = 50;
    public List<float> timerActionsStage1 = new List<float>();
    public List<float> timerActionsStage2 = new List<float>();
    public List<float> timerActionsStage3 = new List<float>();
     List<float> timerActions= new List<float>();
    public List<float> maxLifeToChangeStage = new List<float>();
    int stage=0;
    public GameObject player;
    private BossActions actualAction;
    Timer timer;
    Timer timerIntro;
    int index;
    bool inmortal=false;
    public SimpleHealthBar helthBar;
    bool introFinished=false;
    public BoxCollider col;
    //public ParticleSystem DeadParticle;
    public LayerMask layerThatDontAffectCharge;
    private Renderer[] _meshRends;
    public float bossCameraShakeTime = 2;
    public GameObject shield1;
    public GameObject shield2;
    public GameObject columna;
    public Transform ShootPos1;
    public Transform ShootPos2;
    public Transform ShootPos3;
    public void DeleteAll() {
        actualAction.DeleteAll();
    }

    void Awake()
    {
        shield1= GameObject.Find("ShieldBoss1");
        shield2=GameObject.Find("ShieldBoss2");
        columna = GameObject.Find("Columna");
        maxLife = life;
        SetActions();
        actions = stageActions[stage];
        an = GetComponent<Animator>();
        index = 0;
        stage = 0;
        timerIntro = new Timer(introTime, FinishiIntro);
        actualAction = actions[0];
        actualAction.Begin(this);
        player = ((Player)FindObjectOfType(typeof(Player))).gameObject;
        transform.LookAt(player.transform.position);

        col = this.GetComponent<BoxCollider>();
        //DeadParticle =GameObject.Find("ParticleExplosion").GetComponent<ParticleSystem>();
        UpdateBossLife();
        _meshRends = GetComponentsInChildren<Renderer>();
        ChangeShaderValue("_SegundaFase", 0);
    }

    private void SetActions()
    {

        stageActions.Add(new List<BossActions>());
        stageActions.Add(new List<BossActions>());
        stageActions.Add(new List<BossActions>());

        stageActions[0].Add(bossExpansiveWave);
        //      stageActions[0].Add(bossMissile); duraba 7
        stageActions[0].Add(bossShoot);
        stageActions[0].Add(bossCharge);
        stageActions[0].Add(bossCharge);
        stageActions[0].Add(bossCharge);
        stageActions[0].Add(bossMissile);
        stageActions[0].Add(bossJump);

        stageActions[1].Add(bossJump);
        stageActions[1].Add(bossExpansiveWave2);
        stageActions[1].Add(bossJump);
        stageActions[1].Add(bossExpansiveWave2);
        stageActions[1].Add(bossMissileUpgrade);
        stageActions[1].Add(bossShootUpgrade);
        stageActions[1].Add(bossChargeUpgrade);


        timerActions = timerActionsStage1;
    }

    private void FinishiIntro()
    {
        
        timer = new Timer(timerActions[index], ChangeAction);
        introFinished = true;
    }

    private void UpdateBossLife()
    {

        object[] container = new object[2];
        container[0] = life;
        container[1] = maxLife;

        EventManager.instance.ExecuteEvent(Constants.UPDATE_BOSS_LIFE, container);
    }

    internal void SetAnimation(string name, bool value)
    {
        if (an==null) return;
        an.SetBool(name, value);
       
    }

    private void ChangeAction()
    {
        index++;
        if (index >= timerActions.Count)
        {
            index = 0;
            //Upgrade();
        }
        actualAction.Finish(this);

        ChangeStageIfNeeded();
        actualAction = actions[index]; 
        actualAction.Begin(this);
        timer.Reset(timerActions[index]);
    }

    private void ChangeStageIfNeeded()
    {
        if (life < maxLifeToChangeStage[stage ]) {
            stage++;
            actions = stageActions[stage];
            index = 0;
            if (stage == 1)
            {
                timerActions = timerActionsStage2;
                ChangeShaderValue("_SegundaFase", 1);
            }
            else if (stage == 2) {
                timerActions = timerActionsStage3;
            }
        }
    }

    private void Upgrade()
    {
        stage++;
        UpgradeAction();
    }



    private void UpgradeAction()
    {
        foreach (var action in actions)
        {
            action.Upgrade();
        }
    }

    void Update () {
        if (!introFinished) {
            timerIntro.CheckAndRun();
        }
        if (!isDead() && introFinished) {
            timer.CheckAndRun();
            actualAction.Update(this.transform, player.transform.position);
	    }
   }

    void IHittable.OnHit(int damage) {
        if (!inmortal) {
            life -= damage;
            AbstractOnHitWhiteAction();
            UpdateBossLife();
            if (isDead()) {
                actualAction.Finish(this);
                SetAnimation("Die", true);
                StartCoroutine(Dead());
            }
        }
    }



    IEnumerator Dead() {
        EventManager.instance.ExecuteEvent(Constants.CAMERA_STATIONARY, new object[] { transform.position });
        yield return new WaitForSeconds(2);
        //DeadParticle.transform.position = this.transform.position;
        //DeadParticle.gameObject.SetActive(true);
        //DeadParticle.Play();
        EventManager.instance.ExecuteEvent(Constants.PARTICLE_SET, new object[] { Constants.PARTICLE_BOSS_EXPLOTION_NAME, transform.position });
        EventManager.instance.ExecuteEvent(Constants.BOSS_DESTROYED , new object[] { bossCameraShakeTime });
        Destroy(this.gameObject);
    }

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
            
            _actualSectionNode.SpawnEnemyAtPoint(position, type);
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
            print("me choque");
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
            print("me choque");
            EventManager.instance.ExecuteEvent(Constants.CHARGER_CRUSH);

        }
        
    }
    public void ChangeShaderValue(string name, float value)
    {
        foreach (var item in _meshRends)
        {
            item.material.SetFloat(name, value);
        }
    }

}
