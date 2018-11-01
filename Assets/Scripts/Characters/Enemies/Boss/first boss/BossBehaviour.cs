using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour {
    Boss boss;
    public int lifeToChangeEasy = 150;
    public int lifeToChangeMedium = 250;
    public int lifeToChangeHard = 350;
    public int lifeEasy = 300;
    public int lifeMedium = 400;
    public int lifeHard = 500;
    public List<float> maxLifeToChangeStage = new List<float>();

    /*
    public BossBehaviour(Boss boss) {
        if (Configuration.instance.dificulty == Configuration.Dificulty.Easy)
        {
            boss.life = lifeEasy;
            maxLifeToChangeStage[0] = lifeToChangeEasy;
        }
        else if (Configuration.instance.dificulty == Configuration.Dificulty.Medium)
        {
            boss.life = lifeMedium;
            maxLifeToChangeStage[0] = lifeToChangeMedium;
        }
        else if (Configuration.instance.dificulty == Configuration.Dificulty.Hard)
        {
            boss.life = lifeToChangeHard;
            maxLifeToChangeStage[0] = lifeToChangeHard;
        }
        this.boss = boss;
        boss.SetActions();
        actions = stageActions[stage];
        an = GetComponent<Animator>();
        index = 0;
        stage = 0;
        timerIntro = new Timer(introTime, FinishiIntro);
        actualAction = actions[0];
        actualAction.Begin(this);


    }

    internal void SetAnimation(string name, bool value)
    {
        if (boss.an == null) return;
        boss.an.SetBool(name, value);

    }

    private void ChangeAction()
    {
        boss.index++;
        if (boss.index >= timerActions.Count)
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
        if (life < maxLifeToChangeStage[stage])
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
    */
}
