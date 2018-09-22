using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCharge : MonoBehaviour,BossActions {
    bool _moving = false;
    public float distanceToCharge = 5f;
    public float timeToStartCharging = 1f;
    public float timeToStartMovingAgain = 1f;
    public float speedOfCharge = 3f;
    private Boss boss;
    public bool upgrade;
    public float ExtraSpeedOfCharge=5;
    public DamagePath damagePath;
    public LineRenderer line;

    void BossActions.Begin(Boss boss)
    {
        EventManager.instance.SubscribeEvent(Constants.CHARGER_CRUSH,StopCharging);
        this.boss = boss;
        StartCoroutine(Wait());
        boss.SetAnimation("Shield", true);
        if (upgrade) {
            Upgrade();
        }
    }
    void BossActions.Finish(Boss boss)
    {
        boss.SetAnimation("Shield", false);
        EventManager.instance.UnsubscribeEvent(Constants.CHARGER_CRUSH, StopCharging);
    }


    private void StopCharging(object[] parameterContainer)
    {
        _moving = false;
        damagePath.Stop();
        boss.SetAnimation("Shield", false);
    }
    IEnumerator Wait()
    {
        
        line.SetPosition(0, boss.transform.position);
        Vector3 targetPosition = new Vector3(boss.player.transform.position.x, boss.transform.position.y, boss.player.transform.position.z);
        boss.transform.LookAt(targetPosition);
        Vector3 direction = (boss.player.transform.position - boss.transform.position).normalized;

        line.SetPosition(1, boss.transform.position+direction *100);
        line.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeToStartCharging);
        line.gameObject.SetActive(false);
        StartCoroutine(ChargeMethod());
    }
    IEnumerator ChargeMethod()
    {
        _moving = true;
        //Vector3 target = new Vector3(boss.player.transform.position.x, boss.player.transform.position.y, boss.player.transform.position.z);
        Vector3 targetPosition = new Vector3(boss.player.transform.position.x, boss.transform.position.y, boss.player.transform.position.z);

        damagePath.SpawnDirection(boss.transform.position, this.transform.forward);
        while (_moving)
        {
            boss.transform.position += boss.transform.forward * speedOfCharge * Time.deltaTime * SectionManager.instance.EnemiesMultiplicator;
            yield return null;
        }
        yield return new WaitForSeconds(timeToStartMovingAgain);
    }

    void BossActions.Update(Transform boss, Vector3 playerPosition)
    {


    }

    public void Upgrade()
    {

        speedOfCharge += ExtraSpeedOfCharge;
        boss.SpawnEnemies("ChargeUpgrade",EnemiesManager.TypeOfEnemy.Charger);
    }
}
