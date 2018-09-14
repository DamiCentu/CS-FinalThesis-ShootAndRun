using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurretStrategy : ITurret { 

    EnemyTurretBehaviour _parent;
    LineRenderer _line;

    public LaserTurretStrategy(EnemyTurretBehaviour parent, LineRenderer line) { 
        _parent = parent;
        _line = line; 
    }

    public void OnUpdate() {
        RaycastHit rh;

        if (Physics.Raycast(_parent.shotSpawn.position, _parent.shotSpawn.forward, out rh, _parent.laserMaxDistance, _parent.maskToCollide)) {
            if(rh.collider.gameObject.layer == 8) {
                rh.collider.GetComponent<IHittable>().OnHit(0);
            } 
        }
    } 

    public void SetHitsCanTake() { }

    public void SetStartValues() {
        _parent.shieldGO.SetActive(true);
        
        _parent.transformToRotate.rotation = _parent.transform.rotation * Quaternion.Euler(new Vector3(0f, -180f, -90f));

        //si no choca contra algo hay que desactiva, ahora no lo esta haciendo
        _parent.sparksParticleS.gameObject.SetActive(true);
        _line.enabled = true;

        RaycastHit rh; 
        if (Physics.Raycast(_parent.shotSpawn.position,_parent.shotSpawn.forward, out rh, _parent.laserMaxDistance, _parent.maskToCollide)) { 
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
    }

    public bool OnHitReturnIfDestroyed(int damage) { return false; }
}