using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Flocking))]
public class FireEnemy : AbstractEnemy, IHittable
{

    FollowPathBehaviour _followPathBehaviour;

    public float timeDefault=3;
    private Timer timer;
    public int life = 10;
    public Transform target;
     DamagePath damagePath;
    public float speed;

    private void Start()
    {
        timer = new Timer(timeDefault, Shoot);
        damagePath=GetComponent<DamagePath>();
    }

    private void Shoot()
    {
        print("disparo!");
        print("pos"+this.transform.position+ "this.transform.forward"+ this.transform.forward+ "speed" + speed);
        Vector3 direct = target.position - this.transform.position;
        direct.y = 0;
        damagePath.SpawnDirection(this.transform.position+Vector3.up, direct.normalized, speed);
    }

    private void Update()
    {
            if (timer.CheckAndRun()) timer.Reset();
        if (_eIntegration != null && !_eIntegration.NotFinishedLoading)
        {

        }
    }

    void FixedUpdate()
    {


    }



    public void OnHit(int damage)
    {
        print("auch");
        life -= damage;
        if (life < 0)
        {
            print("mori");
            //   EnemiesManager.instance.ReturnMisilEnemyToPool(this);
            StopAllCoroutines();
            gameObject.SetActive(false);
            EventManager.instance.ExecuteEvent(Constants.ENEMY_DEAD, new object[] { _actualWave, _actualSectionNode, this, true });
        }
    }

    public FireEnemy SetPosition(Vector3 pos)
    {
        transform.position = pos;
        return this;
    }

    public FireEnemy SetTarget(Transform player)
    {
        target = player;
        return this;
    }

    void OnDisable()
    {
        if (_followPathBehaviour != null)
        {
            _followPathBehaviour.OnDisable();
        }
    }



    private void OnTriggerEnter(Collider c)
    {
        /*        if (c.gameObject.layer != 12 && c.gameObject.layer != 13 && c.gameObject.layer != 14 && c.gameObject.layer != 0 && _flocking != null)
                {//enemy //powerup // enemybullet
                 //   _flocking.resetVelocity();
                }*/
    }
}
