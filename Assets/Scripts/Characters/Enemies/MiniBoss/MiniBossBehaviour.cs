using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSMFUNCTIONAL;

public class MiniBossBehaviour : AbstractEnemy,IHittable { 
    public float openColliderHeight = 5.7f;
    public float ClosedColliderHeight = 0f;
    public float radiusToStartLaser = 10f;
    public float timeToStartAttack = 1f;
    public float timeToEndAttack = 1f;

    public int hitsCanTake = 10;
    public Transform transformToRotate;
    public LayerMask layersToHitOnAttack;
    public float maxDistanceOfAttack = 10f;
    public float speedOfAttack = 7f;
    public float speedOfEndAttack = 10f;
    public Transform spawnLaserFront;
    public Transform spawnLaserBack; 
    public float speedRotation = 5f;
    public LayerMask blockEnemyViewToTarget;

    LineRenderer[] _lineRendereders; 

    int _hitsTaken; 
    float _currentDistanceOfAttack = 0f;
    bool _canReciveDamage = false; 
    float _time = 0f;

    Animator _anim;
    Flocking _flocking;
    CapsuleCollider _capCol;
    FollowPathBehaviour _followPathBehaviour;

    WaitForSeconds _waitToStartAttack;
    WaitForSeconds _waitToEndAttack;

    EventFSM<MiniBossInputs> _myFsm;

    enum MiniBossInputs {
        InRange,
        NotInRange,
        FinishedClosing,
        FinishedOpening,
        FinishedIntegration
        //ResetFSM
    }

    void Start () {
        _hitsTaken = hitsCanTake;

        _anim = GetComponentInChildren<Animator>();
        _flocking = GetComponent<Flocking>();
        _capCol = GetComponent<CapsuleCollider>();
        _lineRendereders = GetComponentsInChildren<LineRenderer>(); 
        _followPathBehaviour = new FollowPathBehaviour(this, blockEnemyViewToTarget, _flocking);

        _flocking.target = EnemiesManager.instance.player.transform; 
        _flocking.resetVelocity();
        _anim.SetBool("Moving", true);
        _capCol.height = ClosedColliderHeight;
        _flocking.flockingEnabled = false;

        _waitToStartAttack = new WaitForSeconds(timeToStartAttack);

        _waitToEndAttack = new WaitForSeconds(timeToEndAttack);

        int count = 0;
        foreach (var l in _lineRendereders) { 
            l.SetPosition(count ,transform.position);

            count++;
            if(l.positionCount == count) {
                count = 0;
            }
            l.enabled = false;
        } 

        _followPathBehaviour.SetActualSectionNode(_actualSectionNode);

        SetFsm();
    }

    const string OPENING = "Opening";
    const string CLOSING = "Closing";
    const string SHOOTING_LASER = "ShootingLaser";
    const string FOLLOW_USER = "FollowPlayer";
    const string INTEGRATION = "Integration"; 

    void SetFsm() {  
        var closing = new State<MiniBossInputs>(CLOSING);
        var integration = new State<MiniBossInputs>(INTEGRATION);
        var followUser = new State<MiniBossInputs>(FOLLOW_USER);
        var shootingLaser = new State<MiniBossInputs>(SHOOTING_LASER);
        var opening = new State<MiniBossInputs>(OPENING);


        StateConfigurer.Create(integration)
            .SetTransition(MiniBossInputs.FinishedIntegration, followUser)
            .Done();

        StateConfigurer.Create(followUser)
            .SetTransition(MiniBossInputs.InRange, opening)
            //.SetTransition(MiniBossInputs.ResetFSM, integration)
            .Done();

        StateConfigurer.Create(opening)
            .SetTransition(MiniBossInputs.FinishedOpening, shootingLaser)
            .SetTransition(MiniBossInputs.NotInRange, closing)
            //.SetTransition(MiniBossInputs.ResetFSM, integration)
            .Done();

        StateConfigurer.Create(shootingLaser)
            .SetTransition(MiniBossInputs.NotInRange, closing)
            //.SetTransition(MiniBossInputs.ResetFSM, integration)
            .Done();

        StateConfigurer.Create(closing)
            .SetTransition(MiniBossInputs.InRange, opening)
            .SetTransition(MiniBossInputs.FinishedClosing, followUser)
            //.SetTransition(MiniBossInputs.ResetFSM, integration)
            .Done();

        SetFollowUserFuncs(followUser);
        SetOpeningFuncs(opening);
        SetClosingFuncs(closing);
        SetShootingLaserFuncs(shootingLaser);
        SetIntegrationFuncs(integration);

        _myFsm = new EventFSM<MiniBossInputs>(integration);
    }

    //---------------------------------------------------------------------INTEGRATION

    void SetIntegrationFuncs(State<MiniBossInputs> integration) {
        integration.OnEnter += x => { 
            _anim.speed = 0f;
        };

        integration.OnUpdate += () => {
            if (!_eIntegration.NotFinishedLoading) {
                SendInputToFSM(MiniBossInputs.FinishedIntegration);
            } 
        }; 
    }

    //---------------------------------------------------------------------FOLLOW PLAYER

    void SetFollowUserFuncs(State<MiniBossInputs> followUser) {

        followUser.OnUpdate += () => {
            _followPathBehaviour.OnUpdate();

            if(_flocking.target != EnemiesManager.instance.player.transform) {
                return;
            }
            
            if (Utility.InRangeSquared(_flocking.target.transform.position, transform.position, radiusToStartLaser)) {
                SendInputToFSM(MiniBossInputs.InRange);
            }
        };

        followUser.OnFixedUpdate += () => {
            _anim.speed = SectionManager.instance.EnemiesMultiplicator;
            _flocking.OnFixedUpdate();
        };

        followUser.OnExit += x => {
            _flocking.target = EnemiesManager.instance.player.transform;
        };
    }

    //---------------------------------------------------------------------OPENING 

    void SetOpeningFuncs(State<MiniBossInputs> opening) {
        opening.OnEnter += x => {
            
            _anim.SetBool("Moving", false);
            _time = 0f;
        };

        opening.OnUpdate += () => {
            _anim.speed = SectionManager.instance.EnemiesMultiplicator;
            _time += Time.deltaTime;
            if (Utility.InRangeSquared(_flocking.target.transform.position, transform.position, radiusToStartLaser)) { 
                if (0.1f < _time) { // es el largo del clip de open y closing
                    SendInputToFSM(MiniBossInputs.FinishedOpening);
                }
            }
            else {
                SendInputToFSM(MiniBossInputs.NotInRange);
            }
        }; 
    } 

    //---------------------------------------------------------------------CLOSING

    void SetClosingFuncs(State<MiniBossInputs> closing) {
        closing.OnEnter += x => { 
            _capCol.height = ClosedColliderHeight;
            _anim.SetBool("Moving", true);
            _time = 0f;
        };

        closing.OnUpdate += () => {
            _anim.speed = SectionManager.instance.EnemiesMultiplicator;
            _time += Time.deltaTime;
            if (Utility.InRangeSquared(_flocking.target.transform.position, transform.position, radiusToStartLaser)) { 
                SendInputToFSM(MiniBossInputs.InRange);
            }
            else {
                if (0.1f < _time) { // es el largo del clip de open y closing HARCODINGGG HERMOSO
                    SendInputToFSM(MiniBossInputs.FinishedClosing);
                }
            }
        };
    }

    //---------------------------------------------------------------------SHOOTING LASER 

    void SetShootingLaserFuncs(State<MiniBossInputs> shootingLaser) {
        shootingLaser.OnEnter += x =>  { 
            _currentDistanceOfAttack = 0f;
            foreach (var l in _lineRendereders) {
                l.enabled = true;
            }

            _canReciveDamage = true;
            _capCol.height = openColliderHeight;
        };

        shootingLaser.OnUpdate += () => {
            
            var targetRotation = Quaternion.LookRotation(Utility.SetYInVector3(EnemiesManager.instance.player.transform.position,transform.position.y) - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.deltaTime * SectionManager.instance.EnemiesMultiplicator);

            _lineRendereders[0].SetPosition(0, spawnLaserFront.position);
            _lineRendereders[0].SetPosition(1, spawnLaserFront.position + spawnLaserFront.forward * _currentDistanceOfAttack);
            _lineRendereders[1].SetPosition(0, spawnLaserBack.position);
            _lineRendereders[1].SetPosition(1, spawnLaserBack.position + spawnLaserBack.forward * _currentDistanceOfAttack);


            _anim.speed = SectionManager.instance.EnemiesMultiplicator;

            if (Utility.InRangeSquared(_flocking.target.transform.position, transform.position, radiusToStartLaser)) { 
                _currentDistanceOfAttack += Time.deltaTime * speedOfAttack * SectionManager.instance.EnemiesMultiplicator; 
                if (_currentDistanceOfAttack > maxDistanceOfAttack) { 
                    _currentDistanceOfAttack = maxDistanceOfAttack;
                }
            }

            else {
                _currentDistanceOfAttack -= Time.deltaTime * speedOfEndAttack * SectionManager.instance.EnemiesMultiplicator; 
                if (_currentDistanceOfAttack < 0) { 
                    SendInputToFSM(MiniBossInputs.NotInRange);
                }
            } 
        };

        shootingLaser.OnFixedUpdate += () => {
            RaycastHit rh;
            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out rh, _currentDistanceOfAttack, layersToHitOnAttack) 
                                || Physics.Raycast(transform.position + Vector3.up, -transform.forward, out rh, _currentDistanceOfAttack, layersToHitOnAttack)) {
                if (rh.collider.gameObject.layer == 8) {//player
                    rh.collider.gameObject.GetComponent<IHittable>().OnHit(1);
                }
            } 
        };

        shootingLaser.OnExit += x => { 
            _currentDistanceOfAttack = 0;
            foreach (var l in _lineRendereders) {
                l.enabled = false;
            }

            _canReciveDamage = false;
        };
    }

    //---------------------------------------------------------------------

    void Update () {
        _myFsm.Update();
    }

    void FixedUpdate() {
        _myFsm.FixedUpdate();
    }

    void OnDisable() {
        if(_followPathBehaviour != null) { 
            _followPathBehaviour.OnDisable();
        }
    }

    public void OnHit(int damage) {
        if (!_canReciveDamage) { 
            return;
        }

        AbstractOnHitWhiteAction();
        _hitsTaken -= damage;
        if (_hitsTaken <= 0) { 
             EventManager.instance.ExecuteEvent(Constants.ENEMY_DEAD, new object[] { _actualWave, _actualSectionNode, this,false, hasToDestroyThisToUnlockSomething });
        }
    }

    void SendInputToFSM(MiniBossInputs inp) {
        _myFsm.SendInput(inp);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiusToStartLaser);
    }
}
