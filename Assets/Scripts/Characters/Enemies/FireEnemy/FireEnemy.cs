using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Flocking))]
public class FireEnemy : AbstractEnemy, IHittable {

    public float timeDefault = 3;
    public int life = 10;
    public Transform target;
    public float speed;
    public LayerMask maskThatBlockVisionToPlayer;

    FollowPathBehaviour _followPathBehaviour;
    DamagePath damagePath; 
    Timer timer;

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
    }

    void FixedUpdate() {

    } 

    public void OnHit(int damage) { 
        life -= damage;
        AbstractOnHitWhiteAction();
        if (life < 0) {
            EnemiesManager.instance.ReturnFireEnemyToPool(this);
            Stop();
            StopAllCoroutines();
            gameObject.SetActive(false);
            EventManager.instance.ExecuteEvent(Constants.ENEMY_DEAD, new object[] { _actualWave, _actualSectionNode, this, false, hasToDestroyThisToUnlockSomething });
        }
    }

    public FireEnemy SetPosition(Vector3 pos) {
        transform.position = pos;
        return this;
    }

    public FireEnemy SetTarget(Transform player) {
        target = player;
        return this;
    }

    void OnDisable() {
        if (_followPathBehaviour != null) {
            _followPathBehaviour.OnDisable();
        }
    } 

    private void OnTriggerEnter(Collider c) {
        /*        if (c.gameObject.layer != 12 && c.gameObject.layer != 13 && c.gameObject.layer != 14 && c.gameObject.layer != 0 && _flocking != null)
                {//enemy //powerup // enemybullet
                 //   _flocking.resetVelocity();
                }*/
    }

    public void Stop() {
        damagePath.DeleteAll();
    }
}
