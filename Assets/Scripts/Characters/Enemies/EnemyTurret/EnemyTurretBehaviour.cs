using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretBehaviour : AbstractEnemy, IHittable, IPauseable { 

    [Header("LaserTurret")]
    public int hitsCanTakeLaser = 20;
    public float laserMaxDistance = 100f;
    public LayerMask maskToCollide;
    public ParticleSystem sparksParticleS;

    [Header("MovingLaserTurret")]
    public int hitsCanTakeMovingLaser = 10;
    public float movingLaserMaxDistance = 100f;
    public TurretWaypoint startWaypointForMovingLaser;
    public float speedOfLerp = 3f;
    public float timeToWaitToInteract = 3f;

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

    public SectionNode CurrentNode { get { return _actualSectionNode; } }

    internal bool Paused { get { return paused; } }

    //bool _paused;

    public void OnPauseChange(bool v) {
        paused = v;
        //_anim.enabled = !v;
    }

    void Update() {
        if (paused)
            return;

        if (_eIntegration != null && !_eIntegration.LoadingNotComplete || _actualWave == SectionManager.WaveNumber.NoCuentaParaTerminarNodo)
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

    public EnemyTurretBehaviour DeactivateEverything() {
        GetComponent<LineRenderer>().enabled = false;
        shieldGO.SetActive(false);
        sparksParticleS.gameObject.SetActive(false);
        return this;
    }

    public EnemyTurretBehaviour SetType(EnemiesManager.TypeOfEnemy type, TurretWaypoint starter = null, bool hasToHaveShield = false) {
        if(type != EnemiesManager.TypeOfEnemy.TurretBurst && type != EnemiesManager.TypeOfEnemy.TurretLaser && type != EnemiesManager.TypeOfEnemy.MovingTurretLaser)
            throw new System.Exception("No es de tipo turret");

        if(!_turretTypes.ContainsKey(type)) {
            switch(type) {
                case EnemiesManager.TypeOfEnemy.TurretBurst:
                    _turretTypes.Add(type, new BurstTurretStrategy(this)); 
                    break;
                case EnemiesManager.TypeOfEnemy.TurretLaser:
                    _turretTypes.Add(type, new LaserTurretStrategy(this, GetComponent<LineRenderer>()));
                    break;
                case EnemiesManager.TypeOfEnemy.MovingTurretLaser:
                    _turretTypes.Add(type, new MovingLaserTurretStrategy(this, GetComponent<LineRenderer>()));
                    break; 
            }  
        }

        _currentTypeOfTurret = _turretTypes[type];

        _currentTypeOfTurret.SetStartValues(hasToHaveShield , starter);

        _currentTypeOfTurret.SetHitsCanTake();
        return this;
    }

    public void OnHit(int damage) {
        if(_currentTypeOfTurret.OnHitReturnIfDestroyed(damage)) { 
            EventManager.instance.ExecuteEvent("EnemyDead", new object[] { _actualWave, _actualSectionNode, this, false, hasToDestroyThisToUnlockSomething, wallToUnlockID , false});  // si es true el ultimo parametro significa que el elemento no debe spawnear un power up
        }
    }

    public void StartHitRoutine() {
        AbstractOnHitWhiteAction();
    }

    private void OnDrawGizmos() {
        if(_currentTypeOfTurret is BurstTurretStrategy) { 
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, distanceToShoot);
        }
    }
}
