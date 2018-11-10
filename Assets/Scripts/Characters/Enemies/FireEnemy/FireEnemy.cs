using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Flocking))]
public class FireEnemy : AbstractEnemy, IHittable {

    public float timeDefault = 3;
    public int hitsCanTake = 10;
    public Transform target;
    public float speed;
    public LayerMask maskThatBlockVisionToPlayer;
    public Transform body;
    DamagePath damagePath; 
    Timer timer;

    int _hitsRemaining;

    private void Start() {
        timer = new Timer(timeDefault, Shoot);
        damagePath=GetComponent<DamagePath>();
    }

    private void Shoot() {  
        Vector3 direct = target.position - this.transform.position;
        if (Physics.Raycast(transform.position, direct, direct.magnitude, maskThatBlockVisionToPlayer))
            return;
        direct.y = 0;
        damagePath.SpawnDirection(this.transform.position + new Vector3(0f,1,0f), direct.normalized, speed);
    }

    private void Update() {
        if (timer.CheckAndRun()) timer.Reset();
        if (_eIntegration != null && !_eIntegration.LoadingNotComplete) {

        }
        body.LookAt(EnemiesManager.instance.player.transform.position);
        body.Rotate(90, 0, 0);
    }

    void FixedUpdate() {

    } 

    public void OnHit(int damage) {
        _hitsRemaining -= damage;
        AbstractOnHitWhiteAction();
        if (_hitsRemaining < 0) {
            EnemiesManager.instance.ReturnFireEnemyToPool(this);
            Stop();
            StopAllCoroutines();
            gameObject.SetActive(false);
            EventManager.instance.ExecuteEvent(Constants.ENEMY_DEAD, new object[] { _actualWave, _actualSectionNode, this, false, hasToDestroyThisToUnlockSomething });
        }
    }

    public FireEnemy SetPosition(Vector3 pos) {
        _hitsRemaining = hitsCanTake;
        transform.position = pos;
        if (timer != null) {
            timer.Reset();
        }
        return this;
    }

    public FireEnemy SetTarget(Transform player) {
        target = player;
        return this;
    } 

    private void OnTriggerEnter(Collider c) {
        /*        if (c.gameObject.layer != 12 && c.gameObject.layer != 13 && c.gameObject.layer != 14 && c.gameObject.layer != 0 && _flocking != null)
                {//enemy //powerup // enemybullet
                 //   _flocking.resetVelocity();
                }*/
    }

    private void OnDisable() {
        if (damagePath != null) {
            damagePath.DeleteAll();
        }
    }

    public void Stop() {
        damagePath.DeleteAll();
    }
}
