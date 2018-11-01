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
    public LayerMask CollisionLayer;

    void BossActions.Begin(AbstractBoss boss)
    {
        EventManager.instance.SubscribeEvent(Constants.CHARGER_CRUSH,StopCharging);
        this.boss = boss as Boss;
        StartCoroutine(Wait());
        this.boss.SetAnimation("Shield", true);
        if (upgrade) {
            Upgrade();
        }
        this.boss.shield1.GetComponent<BoxCollider>().enabled=true ;
        this.boss.shield2.GetComponent<BoxCollider>().enabled = true;
        this.boss.ChangeShaderValue("_Shield", 1);
    }
    void BossActions.Finish(AbstractBoss boss)
    {
        this.boss.SetAnimation("Shield", false);
        this.boss.ChangeShaderValue("_Shield", 0);
        EventManager.instance.UnsubscribeEvent(Constants.CHARGER_CRUSH, StopCharging);
        this.boss.shield1.GetComponent<BoxCollider>().enabled = false;
        this.boss.shield2.GetComponent<BoxCollider>().enabled = false;
    }
    void BossActions.DeleteAll()
    {
        boss.ChangeShaderValue("_Shield", 0);
        boss.shield1.GetComponent<BoxCollider>().enabled = false;
        boss.shield2.GetComponent<BoxCollider>().enabled = false;
        EventManager.instance.UnsubscribeEvent(Constants.CHARGER_CRUSH, StopCharging);
        StopCharging(new object[0]);
        damagePath.DeleteAll();
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
        Vector3 direction = (targetPosition - boss.transform.position).normalized;

        //print(direction);

        RaycastHit info;
        Physics.Raycast(boss.transform.position, direction, out info, CollisionLayer);


        line.SetPosition(1, info.point);
        line.gameObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        line.gameObject.SetActive(true);
        //print("length");
        //print(Vector3.Distance(boss.transform.position, targetPosition));
        yield return new WaitForSeconds(timeToStartCharging);
        line.gameObject.SetActive(false);
        StartCoroutine(ChargeMethod());
    }
    IEnumerator ChargeMethod()
    {
        _moving = true;
        //Vector3 target = new Vector3(boss.player.transform.position.x, boss.player.transform.position.y, boss.player.transform.position.z);
        Vector3 targetPosition = new Vector3(boss.player.transform.position.x, boss.transform.position.y, boss.player.transform.position.z);

        damagePath.SpawnDirection(boss.transform.position, this.transform.forward,this.speedOfCharge);
        while (_moving)
        {
            boss.transform.position += boss.transform.forward * speedOfCharge * Time.deltaTime;
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
