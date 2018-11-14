using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSMFUNCTIONAL;

public class MiniBossBehaviour : AbstractEnemy,IHittable , IPauseable {
    public float radiusToStartShooting = 10f;
    public float radiusToEndShooting = 15f;
    public float timeToStartAttack = 1f;
    public float timeToEndAttack = 1f;

    public int hitsCanTake = 10;
    public float timeBetweenBullets = .5f;
    public Transform spawnBulletPosition;
    public float speedRotation = 5f;
    public LayerMask blockEnemyViewToTarget;

    int _hitsTaken;
    bool _canReciveDamage = false; 
    float _time = 0f;
    float _timerToShoot = 0f;

    Animator _anim;
    Flocking _flocking;
    FollowPathBehaviour _followPathBehaviour;

    WaitForSeconds _waitToStartAttack;
    WaitForSeconds _waitToEndAttack;

    EventFSM<MiniBossInputs> _myFsm;

    //bool _paused;

    public void OnPauseChange(bool v) {
        paused = v;
        _anim.enabled = !v;
    }

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
        _followPathBehaviour = new FollowPathBehaviour(this, blockEnemyViewToTarget, _flocking);

        _flocking.target = EnemiesManager.instance.player.transform; 
        _flocking.resetVelocity();
        _anim.SetBool("Moving", true);
        _flocking.flockingEnabled = false;

        _waitToStartAttack = new WaitForSeconds(timeToStartAttack);

        _waitToEndAttack = new WaitForSeconds(timeToEndAttack);

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
        var shooting = new State<MiniBossInputs>(SHOOTING_LASER);
        var opening = new State<MiniBossInputs>(OPENING);


        StateConfigurer.Create(integration)
            .SetTransition(MiniBossInputs.FinishedIntegration, followUser)
            .Done();

        StateConfigurer.Create(followUser)
            .SetTransition(MiniBossInputs.InRange, opening)
            .Done();

        StateConfigurer.Create(opening)
            .SetTransition(MiniBossInputs.FinishedOpening, shooting)
            .SetTransition(MiniBossInputs.NotInRange, closing)
            .Done();

        StateConfigurer.Create(shooting)
            .SetTransition(MiniBossInputs.NotInRange, closing)
            .Done();

        StateConfigurer.Create(closing)
            .SetTransition(MiniBossInputs.InRange, opening)
            .SetTransition(MiniBossInputs.FinishedClosing, followUser)
            .Done();

        SetFollowUserFuncs(followUser);
        SetOpeningFuncs(opening);
        SetClosingFuncs(closing);
        SetShootingFuncs(shooting);
        SetIntegrationFuncs(integration);

        _myFsm = new EventFSM<MiniBossInputs>(integration);
    }

    //---------------------------------------------------------------------INTEGRATION

    void SetIntegrationFuncs(State<MiniBossInputs> integration) {
        integration.OnEnter += x => { 
            _anim.speed = 0f;
        };

        integration.OnUpdate += () => {
            if (!_eIntegration.LoadingNotComplete) {
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
            
            if (Utility.InRangeSquared(_flocking.target.transform.position, transform.position, radiusToStartShooting)) {
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
            if (Utility.InRangeSquared(_flocking.target.transform.position, transform.position, radiusToEndShooting)) { 
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
            _anim.SetBool("Moving", true);
            _time = 0f;
        };

        closing.OnUpdate += () => {
            _anim.speed = SectionManager.instance.EnemiesMultiplicator;
            _time += Time.deltaTime;
            if (Utility.InRangeSquared(_flocking.target.transform.position, transform.position, radiusToEndShooting)) { 
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

    void SetShootingFuncs(State<MiniBossInputs> shooting) {
        shooting.OnEnter += x => {
            _timerToShoot = 0;

            _canReciveDamage = true;
        };

        shooting.OnUpdate += () => {
            
            var targetRotation = Quaternion.LookRotation(Utility.SetYInVector3(EnemiesManager.instance.player.transform.position,transform.position.y) - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.deltaTime * SectionManager.instance.EnemiesMultiplicator);

            _anim.speed = SectionManager.instance.EnemiesMultiplicator;

            if (Utility.InRangeSquared(_flocking.target.transform.position, transform.position, radiusToEndShooting)) {

                if(_timerToShoot > timeBetweenBullets) { 
                    var s = EnemyBulletManager.instance.giveMeEnemyBullet();
                    s.SetPos(spawnBulletPosition.position).SetDir(spawnBulletPosition.forward).gameObject.SetActive(true);
                    _timerToShoot = 0;
                }

                _timerToShoot += Time.deltaTime;
            }

            else {
                    SendInputToFSM(MiniBossInputs.NotInRange);
            } 
        };

        shooting.OnExit += x => {
            _timerToShoot = 0;
            _canReciveDamage = false;
        };
    }

    //---------------------------------------------------------------------

    void Update () {
        if (paused)
            return;

        _myFsm.Update();
    }

    void FixedUpdate() {
        if (paused)
            return;

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
             EventManager.instance.ExecuteEvent(Constants.ENEMY_DEAD, new object[] { _actualWave, _actualSectionNode, this,false, hasToDestroyThisToUnlockSomething, wallToUnlockID });
        }
    }

    void SendInputToFSM(MiniBossInputs inp) {
        _myFsm.SendInput(inp);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiusToStartShooting);
    }
}
