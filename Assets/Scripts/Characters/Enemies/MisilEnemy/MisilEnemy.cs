using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Flocking))]
public class MisilEnemy : AbstractEnemy, IHittable {

    public LayerMask blockEnemyViewToPlayer;
    public Missile Missile;
    public float timeBetweenMissiles;
    public float timeToBoom;
    public float maxOffset;
    public float _timer=-20;

   // Animator _anim;

    private LayerMask misileEnemyLayerMask;

    public int life = 10;
    public Transform target;
    public  Transform spawnMissilesPosition; 

    void FixedUpdate() {
        if (_eIntegration != null && _eIntegration.LoadingNotComplete) 
            return;

        _timer += Time.deltaTime;
        if (_timer > timeBetweenMissiles)
        {
            DropMissile(target.position);
            _timer = 0;
        }
    }

    private void DropMissile(Vector3 playerPosition)
    {
        float xPosition = playerPosition.x + UnityEngine.Random.Range(-maxOffset, maxOffset);
        float zPosition = playerPosition.z + UnityEngine.Random.Range(-maxOffset, maxOffset);
        Vector3 destination = new Vector3(xPosition, playerPosition.y + 0.3f, zPosition);
        //Missile mis= new Missile(destination, timeToBoom)
        Missile mis = Instantiate(Missile, spawnMissilesPosition.position, Quaternion.FromToRotation(spawnMissilesPosition.position, destination));
        mis.Set(destination, timeToBoom);
    }

    public void OnHit(int damage)
    {
        life -= damage;
        AbstractOnHitWhiteAction();
        if (life <= 0) {
        AbstractOnHitWhiteAction();
            EnemiesManager.instance.ReturnMisilEnemyToPool(this);
            StopAllCoroutines();
            gameObject.SetActive(false);
            EventManager.instance.ExecuteEvent(Constants.ENEMY_DEAD, new object[] { _actualWave, _actualSectionNode, this, false, hasToDestroyThisToUnlockSomething });
        }
    }

    public MisilEnemy SetPosition(Vector3 pos)
    {
        transform.position = pos;
        _timer = 0;
        return this;
    }

    public MisilEnemy SetTarget(Transform player)
    {
        target = player;
        return this;
    }

    void OnDisable()
    {

    } 

    private void OnTriggerEnter(Collider c)
    {

    }
}
