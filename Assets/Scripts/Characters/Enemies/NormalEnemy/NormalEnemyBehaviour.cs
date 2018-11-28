using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Flocking))]
public class NormalEnemyBehaviour : AbstractEnemy, IHittable, IPauseable {

    public LayerMask blockEnemyViewToPlayer; 

    Flocking _flocking;
    Animator _anim;

    FollowPathBehaviour _followPathBehaviour;

    //bool _paused = false;

    public void OnPauseChange(bool v) {
        paused = v;
        _anim.enabled = !v;
    }

    private void Update() {
        if (paused)
            return;

        if(_eIntegration != null && !_eIntegration.LoadingNotComplete)
            _followPathBehaviour.OnUpdate();
    }

    void FixedUpdate () {
        if (paused)
            return;

        if (_flocking == null || _eIntegration == null || _anim == null)
            return;

        if (!_eIntegration.LoadingNotComplete) { 
            _flocking.OnFixedUpdate();
            _anim.speed = SectionManager.instance.EnemiesMultiplicator; 
        }
        else _anim.speed = 0f;
    }

    public void OnHit(int damage) {
        EnemiesManager.instance.ReturnNormalEnemyToPool(this);
        StopAllCoroutines();
        gameObject.SetActive(false);
        EventManager.instance.ExecuteEvent(Constants.ENEMY_DEAD, new object[] { _actualWave, _actualSectionNode, this, false, hasToDestroyThisToUnlockSomething,wallToUnlockID }); 
    } 

    public NormalEnemyBehaviour SetPosition(Vector3 pos) {
        transform.position = pos;
        if (_anim == null)
            _anim = GetComponent<Animator>();

        _flocking.resetVelocity(false);
        return this;
    }

    void OnDisable() {
        if(_followPathBehaviour != null) { 
            _followPathBehaviour.OnDisable();
        }
    }

    public NormalEnemyBehaviour SetTarget(Transform target) {
        if (_flocking == null)
            _flocking = GetComponent<Flocking>();

        if (_followPathBehaviour == null)
            _followPathBehaviour = new FollowPathBehaviour(this, blockEnemyViewToPlayer, _flocking/*, true*/);

        _followPathBehaviour.SetActualSectionNode(_actualSectionNode);

        _flocking.target = target;
        return this;
    }

    private void OnTriggerEnter(Collider c) {
        if (c.gameObject.layer != 12 && c.gameObject.layer != 13 && c.gameObject.layer != 14 && c.gameObject.layer != 0 &&_flocking != null) {//enemy //powerup // enemybullet
            _flocking.resetVelocity(true); 
        }
    }
}