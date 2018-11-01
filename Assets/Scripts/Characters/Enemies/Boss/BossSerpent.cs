using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSerpent : AbstractBoss {
    public float speed=1;
    public BossThrowFire actionThrowFire;
    void Awake()
    {
        SetActions();
        Config();



        transform.LookAt(player.transform.position);
    }

    private void SetActions()
    {

        stageActions.Add(new List<BossActions>());
        stageActions[0].Add(actionThrowFire);

    }

    new void Update()
    {
        base.Update();
        transform.Rotate(Vector3.down, 95.5f);
        this.transform.position += this.transform.right * speed * Time.deltaTime;
    }

}
