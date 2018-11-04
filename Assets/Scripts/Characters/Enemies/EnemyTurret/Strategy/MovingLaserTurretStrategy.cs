using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLaserTurretStrategy : ITurret {  

    EnemyTurretBehaviour _parent;
    LineRenderer _line;
     
    int _hitsRemaining = 0;
    bool _canInteract = false;
    bool _hasShield = false;
    //float _time = 0f;

    public MovingLaserTurretStrategy(EnemyTurretBehaviour parent, LineRenderer line) { 
        _parent = parent;
        _line = line; 
    }

    public void OnUpdate() {
        RaycastHit rh;

        if (Physics.Raycast(_parent.shotSpawn.position, _parent.shotSpawn.forward, out rh, _parent.laserMaxDistance, _parent.maskToCollide)) {
            if(rh.collider.gameObject.layer == 8) {
                rh.collider.GetComponent<IHittable>().OnHit(0);
            } 

            _line.SetPosition(0, _parent.shotSpawn.position);
            _line.SetPosition(1, rh.point);

            _parent.sparksParticleS.gameObject.SetActive(true);
            _parent.sparksParticleS.transform.position = rh.point;
            _parent.sparksParticleS.transform.forward = -_parent.shotSpawn.forward;
            _parent.sparksParticleS.Play();
        } 
        else {
            var a = _parent.shotSpawn.forward * _parent.laserMaxDistance + _parent.shotSpawn.position;
            _line.SetPosition(0, _parent.shotSpawn.position);
            _line.SetPosition(1, a);
            _parent.sparksParticleS.gameObject.SetActive(false);
        }


        //if(!WaitToStartMovement) {
        if (!_canInteract) {
            //_time += Time.deltaTime;
            return;
        } 

        var dir = (_parent.startWaypointForMovingLaser.transform.position - _parent.transform.position).normalized;
        _parent.transform.position += dir * _parent.speedOfLerp * Time.deltaTime;

        if(Utility.InRangeSquared(_parent.startWaypointForMovingLaser.transform.position, _parent.transform.position, _parent.startWaypointForMovingLaser.radius)) {
            _parent.startWaypointForMovingLaser = _parent.startWaypointForMovingLaser.next;
        } 
    }

    public void SetHitsCanTake() {
        _hitsRemaining = _parent.hitsCanTakeBurst;
    }

    public void SetStartValues(bool hasToHaveShield = false, TurretWaypoint start = null) {
        _hasShield = hasToHaveShield;
        _parent.shieldGO.SetActive(hasToHaveShield);

        if(start != null) {
            _parent.startWaypointForMovingLaser = start;
            _parent.transform.position = start.transform.position;
        }

        _parent.transformToRotate.rotation = _parent.transform.rotation * Quaternion.Euler(new Vector3(0f, -180f, -90f));

        //si no choca contra algo hay que desactiva, ahora no lo esta haciendo
        _parent.sparksParticleS.gameObject.SetActive(true);
        _line.enabled = true;

        _parent.transform.position = _parent.startWaypointForMovingLaser.transform.position;
        //_time = 0f;

        EventManager.instance.SubscribeEvent(Constants.PLAYER_CAN_MOVE, OnPlayerCanMove);
        EventManager.instance.SubscribeEvent(Constants.PLAYER_DEAD, OnPlayerDead);
    }

    private void OnPlayerDead(object[] parameterContainer) { 
        _canInteract = false;
    }

    private void OnPlayerCanMove(object[] parameterContainer) {
        if (SectionManager.instance.actualNode != _parent.CurrentNode || !_parent.isActiveAndEnabled) {
            return;
        }
        
        _parent.StartCoroutine(CanMoveRoutine());
    }

    IEnumerator CanMoveRoutine() {
        yield return new WaitForSeconds(1f);
        _canInteract = true;
    }

    //bool WaitToStartMovement{get { return _time > _parent.timeToWaitToInteract; } }

    public bool OnHitReturnIfDestroyed(int damage) {
        if (_hasShield) {
            return false;
        }

        _hitsRemaining -= damage;
        _parent.StartHitRoutine();
        if (_hitsRemaining <= 0) { 
            EnemiesManager.instance.ReturnTurretEnemyToPool(_parent);
            EventManager.instance.UnsubscribeEvent(Constants.PLAYER_CAN_MOVE, OnPlayerCanMove);
            EventManager.instance.UnsubscribeEvent(Constants.PLAYER_DEAD, OnPlayerCanMove);
            _canInteract = false;
            _line.enabled = false;
            _parent.sparksParticleS.gameObject.SetActive(false);
            _parent.gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}
