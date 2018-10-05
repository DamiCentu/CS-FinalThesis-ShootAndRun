using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretBehaviour : AbstractEnemy, IHittable { 

    [Header("LaserTurret")]
    public int hitsCanTakeLaser = 20;
    public float laserMaxDistance = 100f;
    public LayerMask maskToCollide;
    public ParticleSystem sparksParticleS;

    [Header("BurstTurret")]
    public int hitsCanTakeBurst = 5;
    public float distanceToShoot = 10f;
    public float timeToWaitBetweenShots = 2f;
    public float timeDelayInBurst = 0.2f;
    public int shotsInBurst = 3;
    public Transform transformToRotate;
    public float timeToStartShootingBurst = 1f;

    [Header("BothTurrets")]
    public Transform shotSpawn;
    public GameObject shieldGO;

    public ITurret _currentTypeOfTurret;

    Dictionary<EnemiesManager.TypeOfEnemy, ITurret> _turretTypes = new Dictionary<EnemiesManager.TypeOfEnemy, ITurret>();

    void Update() {
        if (_eIntegration != null && !_eIntegration.NotFinishedLoading || _actualWave == SectionManager.WaveNumber.NoCuentaParaTerminarNodo)
            _currentTypeOfTurret.OnUpdate();
    }

    public EnemyTurretBehaviour SetPosition(Vector3 pos) {
        transform.position = pos; 
        return this;
    }
    
    public EnemyTurretBehaviour SetForward(Vector3 forw) {
        transform.forward = forw;
        return this;
    }

    public EnemyTurretBehaviour DeactivateEverithing() {
        GetComponent<LineRenderer>().enabled = false;
        shieldGO.SetActive(false);
        sparksParticleS.gameObject.SetActive(false);
        return this;
    }

    public EnemyTurretBehaviour SetType(EnemiesManager.TypeOfEnemy type) {
        if(type != EnemiesManager.TypeOfEnemy.TurretBurst && type != EnemiesManager.TypeOfEnemy.TurretLaser)
            throw new System.Exception("No es de tipo turret");

        if(!_turretTypes.ContainsKey(type)) {
            if (type == EnemiesManager.TypeOfEnemy.TurretBurst) { 
                _turretTypes.Add(type, new BurstTurretStrategy(this));
            }
            else { 
                _turretTypes.Add(type, new LaserTurretStrategy(this, GetComponent<LineRenderer>()));
            }

        }
        _currentTypeOfTurret = _turretTypes[type];

        _currentTypeOfTurret.SetStartValues();

        _currentTypeOfTurret.SetHitsCanTake();
        return this;
    }

    public void OnHit(int damage) {
        if(_currentTypeOfTurret.OnHitReturnIfDestroyed(damage)) { 
            EventManager.instance.ExecuteEvent("EnemyDead", new object[] { _actualWave, _actualSectionNode, this, false });
        }
    }

    public void StartHitRoutine() {
        AbstractOnHitWhiteAction();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanceToShoot);
    }
}
