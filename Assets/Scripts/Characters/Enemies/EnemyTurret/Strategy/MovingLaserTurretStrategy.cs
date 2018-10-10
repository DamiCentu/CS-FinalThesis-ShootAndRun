using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLaserTurretStrategy : ITurret {  

    EnemyTurretBehaviour _parent;
    LineRenderer _line; 

    int _hitsRemaining = 0; 
    float _time = 0f;

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
            _parent.sparksParticleS.transform.position = rh.point;
            _parent.sparksParticleS.transform.forward = -_parent.shotSpawn.forward;
            _parent.sparksParticleS.Play();
        } 
        else {
            var a = _parent.shotSpawn.forward * _parent.laserMaxDistance + _parent.shotSpawn.position;
            _line.SetPosition(0, _parent.shotSpawn.position);
            _line.SetPosition(1, a);
        }


        if(!WaitToStartMovement) {
            _time += Time.deltaTime;
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

    public void SetStartValues() {
        _parent.shieldGO.SetActive(false);
        
        _parent.transformToRotate.rotation = _parent.transform.rotation * Quaternion.Euler(new Vector3(0f, -180f, -90f));

        //si no choca contra algo hay que desactiva, ahora no lo esta haciendo
        _parent.sparksParticleS.gameObject.SetActive(true);
        _line.enabled = true;

        _parent.transform.position = _parent.startWaypointForMovingLaser.transform.position;
        _time = 0f;
    }

    bool WaitToStartMovement{get { return _time > _parent.timeToWaitToInteract; } }

    public bool OnHitReturnIfDestroyed(int damage) {
        _hitsRemaining -= damage;
        _parent.StartHitRoutine();
        if (_hitsRemaining <= 0) { 
            EnemiesManager.instance.ReturnTurretEnemyToPool(_parent);
            _parent.gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}
