using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Flocking))]
public class FireEnemy : AbstractEnemy, IHittable
{

    //  Flocking _flocking;
    // Animator _anim;

    FollowPathBehaviour _followPathBehaviour;


    public int life = 10;
    public Transform target;


    private void Update()
    {

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
