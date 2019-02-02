using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerEnemyBehaviour : AbstractEnemy, IHittable, IPauseable {

    public float distanceToCharge = 5f;
    public float timeToStartCharging = 1f;
    public float timeToStartMovingAgain = 1f;
    public float speedOfCharge = 3f;
    public int hitsCanTake = 5;
    public LayerMask layerThatDontAffectCharge;
    public LayerMask blockEnemyViewToPlayer;

    int _hitsRemaining = 0;
    bool _charging = false;
    bool _moving = false;
    Flocking _flocking;

    FollowPathBehaviour _followPathBehaviour;

    //bool _paused;

    public void OnPauseChange(bool v) {
        paused = v;
        //_anim.enabled = !v;
    }

    public void OnHit(int damage) {
        _hitsRemaining -= damage;
        
        AbstractOnHitWhiteAction();

        if (_hitsRemaining <= 0) {
            _charging = false;
            EnemiesManager.instance.ReturnChargerEnemyToPool(this);
            gameObject.SetActive(false);
            EventManager.instance.ExecuteEvent(Constants.ENEMY_DEAD, new object[] { _actualWave, _actualSectionNode,this, false, hasToDestroyThisToUnlockSomething,wallToUnlockID });
        }
    }

    void Update() {
        if (paused)
            return;

        if (_eIntegration == null || _eIntegration.LoadingNotComplete)
            return;

        if (!_charging)
            _followPathBehaviour.OnUpdate();
    }

    void FixedUpdate () {
        if (paused)
            return;

        if (_eIntegration == null || _eIntegration.LoadingNotComplete)
            return;

        if (!_charging && _flocking != null) {
            RaycastHit rh;
            if (Utility.InRange(transform.position, _flocking.target.position, distanceToCharge) && Physics.Raycast(transform.position, _flocking.target.position - transform.position, out rh, distanceToCharge) && rh.collider.gameObject.layer == 8) {
                //Debug.DrawRay(transform.position, _flocking.target.position - transform.position, Color.blue , Time.fixedDeltaTime);
                StartCoroutine(ChargeMethod()); 
            }
            else { 
                _flocking.OnFixedUpdate();
            }
        }
    }

    IEnumerator ChargeMethod() {
        _charging = true;
        yield return new WaitForSeconds(timeToStartCharging);
        _moving = true;

        Vector3 target = new Vector3(_flocking.target.position.x, transform.position.y, _flocking.target.position.z);

        transform.forward = target - transform.position;
        while (_moving) {
            transform.position += transform.forward * speedOfCharge * Time.deltaTime * SectionManager.instance.EnemiesMultiplicator;
            while (paused) {
                yield return null;
            }
            yield return null;
        } 
        yield return new WaitForSeconds(timeToStartMovingAgain);
        _charging = false;
    }

    public ChargerEnemyBehaviour SetPosition(Vector3 pos) {
        transform.position = pos;
        _hitsRemaining = hitsCanTake;
        _flocking.resetVelocity(false);
        return this;
    }

    public ChargerEnemyBehaviour SetTarget(Transform target) {
        if(_flocking == null)
            _flocking = GetComponent<Flocking>();

        if (_followPathBehaviour == null)
            _followPathBehaviour = new FollowPathBehaviour(this, blockEnemyViewToPlayer, _flocking/*,true*/);

        _followPathBehaviour.SetActualSectionNode(_actualSectionNode);

        _flocking.target = target;
        _charging = false;
        return this;
    } 

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, distanceToCharge);
    }

    private void OnTriggerEnter(Collider c) {
        //if (c.gameObject.layer != 12 && c.gameObject.layer != 13 && c.gameObject.layer != 0) {//enemy //powerup//ddefault
        if(layerThatDontAffectCharge != (layerThatDontAffectCharge | (1 << c.gameObject.layer))) { 
            _moving = false;
            _flocking.resetVelocity(false);
        } 
    } 
}
