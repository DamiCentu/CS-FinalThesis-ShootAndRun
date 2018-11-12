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
    public GameObject cañon;

    private Vector3 defaultRot = new Vector3(0, -90, -90);

   // Animator _anim;

    private LayerMask misileEnemyLayerMask;

    public int hitsCanTake = 10;
    public Transform target;
    public  Transform spawnMissilesPosition;
    int _hitsRemaining;

    void FixedUpdate() {
        if (_eIntegration != null && _eIntegration.LoadingNotComplete) 
            return;

        _timer += Time.deltaTime;
        if (_timer > timeBetweenMissiles)
        {
            //Vector3 dir = target.position- this.transform.position;
            float angle = Vector3.SignedAngle(cañon.transform.position, target.position, Vector3.right);
        cañon.transform.rotation = Quaternion.Euler(0, 90, -90);
       //     cañon.transform.rotation= Quaternion.AngleAxis(angle, Vector3.right);

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

     //   Missile mis2 = Instantiate(Missile, this.transform.position+ Vector3.up, Quaternion.LookRotation(Vector3.up, Vector3.forward));


        mis.Set(destination, timeToBoom);
    }

    public void OnHit(int damage)
    {
        _hitsRemaining -= damage;
        AbstractOnHitWhiteAction();
        if (_hitsRemaining <= 0) {
        AbstractOnHitWhiteAction();
            EnemiesManager.instance.ReturnMisilEnemyToPool(this);
            StopAllCoroutines();
            gameObject.SetActive(false);
            EventManager.instance.ExecuteEvent(Constants.ENEMY_DEAD, new object[] { _actualWave, _actualSectionNode, this, false, hasToDestroyThisToUnlockSomething, wallToUnlockID });
        }
    }

    public MisilEnemy SetPosition(Vector3 pos)
    {
        _hitsRemaining = hitsCanTake;
        transform.position = pos;
        cañon.transform.rotation = Quaternion.Euler(0, 90, -90);
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
