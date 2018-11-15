using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractBoss : AbstractEnemy
{

    public float introTime;

    public List<BossActions> actions = new List<BossActions>();
    public List<List<BossActions>> stageActions = new List<List<BossActions>>();
    Animator an;
    public int lifeToChangeEasy = 150;
    public int lifeToChangeMedium = 250;
    public int lifeToChangeHard = 350;
    public int lifeEasy = 300;
    public int lifeMedium = 400;
    public int lifeHard = 500;
    public List<float> maxLifeToChangeStage = new List<float>();
    public int stage = 0;
    public int index = 0;
    public List<float> timerActionsStage1 = new List<float>();
    public List<float> timerActionsStage2 = new List<float>();
    public List<float> timerActionsStage3 = new List<float>();
    List<float> timerActions = new List<float>();
    Timer timerIntro;
    Timer timer;
    protected BossActions actualAction;
    public int life = 50;
    public float bossCameraShakeTime = 2;
    public GameObject player;
    int maxLife = 50;
    bool introFinished = false;
    public Renderer[] meshRends;
    protected bool shouldChangeStage = true;

    public void Config()
    {

        player = ((Player)FindObjectOfType(typeof(Player))).gameObject;
        index = 0;
        stage = 0;
        SetLife(0);
        maxLife = life;
        actions = stageActions[stage];
        an = GetComponent<Animator>();
        timerIntro = new Timer(introTime, FinishiIntro);
        timerActions = timerActionsStage1;
        actualAction = actions[0];
        //TODO OJOOOO VER Q NO ROMPA EL  BOSS1. 
        //actualAction.Begin(this);
    }

    private void SetLife(int stage)
    {
        if (Configuration.instance.dificulty == Configuration.Dificulty.Easy)
        {
            life = lifeEasy;
            maxLifeToChangeStage[stage] = lifeToChangeEasy;
        }
        else if (Configuration.instance.dificulty == Configuration.Dificulty.Medium)
        {
            life = lifeMedium;
            maxLifeToChangeStage[stage] = lifeToChangeMedium;
        }
        else if (Configuration.instance.dificulty == Configuration.Dificulty.Hard)
        {
            life = lifeToChangeHard;
            maxLifeToChangeStage[stage] = lifeToChangeHard;
        }
    }

    protected void Evolve() {
        SetLife(1);
        life *= 2;
        maxLife = life;
        shouldChangeStage = true;

        stage=1;
        index = 0;
        actions = stageActions[stage];
        timerActions = timerActionsStage2;


        actualAction = actions[index];
        actualAction.Begin(this);
        timer.Reset(timerActions[index]);

    }

    bool isDead()
    {
        return life <= 0;
    }

    public void GetDamage(int damage)
    {
        life -= damage;
        UpdateBossLife();
        if (isDead())
        {
            actualAction.Finish(this);
            SetAnimation("Die", true);
            StartCoroutine(Dead());
        }
    }

    IEnumerator Dead()
    {
        EventManager.instance.ExecuteEvent(Constants.CAMERA_STATIONARY, new object[] { transform.position });
        yield return new WaitForSeconds(2);

        EventManager.instance.ExecuteEvent(Constants.PARTICLE_SET, new object[] { Constants.PARTICLE_BOSS_EXPLOTION_NAME, transform.position });
        EventManager.instance.ExecuteEvent(Constants.BOSS_DESTROYED, new object[] { bossCameraShakeTime });
        Destroy(this.gameObject);
    }

    public void DeleteAll()
    {
        actualAction.DeleteAll();
    }

    private void FinishiIntro()
    {

        timer = new Timer(timerActions[index], ChangeAction);
        introFinished = true;
        actualAction.Begin(this);
    }

    internal void SetAnimation(string name, bool value)
    {
        if (an == null) return;
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

    public void ChangeShaderValue(string name, float value)
    {
        foreach (var item in meshRends)
        {
            item.material.SetFloat(name, value);
        }
    }

    public void UpdateBossLife()
    {

        object[] container = new object[2];
        container[0] = life;
        container[1] = maxLife;

        EventManager.instance.ExecuteEvent(Constants.UPDATE_BOSS_LIFE, container);
    }

    protected void ChangeStageIfNeeded()
    {
        if (life < maxLifeToChangeStage[stage]&& life>0 && shouldChangeStage)
        {
            stage++;
            actions = stageActions[stage];
            index = 0;
            if (stage == 1)
            {
                timerActions = timerActionsStage2;
                ChangeShaderValue("_SegundaFase", 1);
            }
            else if (stage == 2)
            {
                timerActions = timerActionsStage3;
            }
        }
        if (life <= 0&& this.gameObject!=null)
        {
            this.gameObject.SetActive(false);

        }
    }

    private void Upgrade()
    {
        stage++;
        UpgradeAction();
    }


    protected void Update()
    {
        if (!introFinished)
        {
            timerIntro.CheckAndRun();
        }
        if (!isDead() && introFinished)
        {
            timer.CheckAndRun();
            actualAction.Update(this.transform, player.transform.position);
        }
    }

    private void UpgradeAction()
    {
        foreach (var action in actions)
        {
            action.Upgrade();
        }
    }


}
