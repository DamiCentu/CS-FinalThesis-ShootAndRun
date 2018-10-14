using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FSMFUNCTIONAL;

public class CubeEnemyBehaviour : AbstractEnemy, IHittable {

    public LayerMask blockEnemyViewToPlayer; 
    public int hitsCanTake = 5;
    public float stoppedTime = 1f;
    public float radiusToAttack = 7f;
    public Transform shotPos;
    public int quantityOfShots = 5;
    public float angleBetweenShots = 20f;
    public float minRadiusOfEvade = 5f;
    public float maxRadiusOfEvade = 7f;
    public float radiusOfTranformToFollowOnEvade = 2.5f;
    public Canvas canv;
    public Image imageTargetOnEvade;
    public float speedToRotateImage = 60f;
    public float tiltingTime = 0.05f;

    float _tiltingTimer = 0;
    int _hitsRemaining = 0;

    //Animator _anim; 
    MakeUILine _uiLine;
    Flocking _flocking;
    FollowPathBehaviour _followPathBehaviour;
    EventFSM<CubeInputs> _myFsm;

    GameObject _transformToFollowOnEvade ; 

    enum CubeInputs {
        InRadius, 
        NotInRadius,
        FinishedEvade
    }

    void Start() {
        _transformToFollowOnEvade = new GameObject("TransformToFollowOnCubeEvade");

        canv.transform.parent = _transformToFollowOnEvade.transform;
        canv.transform.localPosition = Vector3.zero;
        imageTargetOnEvade.color = Color.yellow;
        imageTargetOnEvade.enabled = false;

        _uiLine = GetComponent<MakeUILine>();
        SetFsm();
    }

    void SetFsm() {  
        var flocking = new State<CubeInputs>("Flocking");
        var stopped = new State<CubeInputs>("Stopped");
        var shoot = new State<CubeInputs>("Shoot");
        var evade = new State<CubeInputs>("Evade");


        StateConfigurer.Create(flocking)
            .SetTransition(CubeInputs.InRadius, stopped)
            //.SetTransition(ChaserInputs.ResetFSM, integration)
            .Done();

        StateConfigurer.Create(stopped)
            .SetTransition(CubeInputs.InRadius, shoot)
            .SetTransition(CubeInputs.NotInRadius, flocking)
            //.SetTransition(ChaserInputs.ResetFSM, integration)
            .Done();
         
        StateConfigurer.Create(shoot)
            .SetTransition(CubeInputs.InRadius, evade)
            .SetTransition(CubeInputs.NotInRadius, flocking)
            //.SetTransition(ChaserInputs.ResetFSM, integration)
            .Done();

        StateConfigurer.Create(evade)
            .SetTransition(CubeInputs.InRadius, stopped)
            .SetTransition(CubeInputs.NotInRadius, flocking)
            .SetTransition(CubeInputs.FinishedEvade, flocking)
            //.SetTransition(ChaserInputs.ResetFSM, integration)
            .Done();

        SetFlokingFuncs(flocking);
        SetStoppedFuncs(stopped);
        SetShootFuncs(shoot);
        SetEvadeFuncs(evade);
        _myFsm = new EventFSM<CubeInputs>(flocking);
    }

    void SetFlokingFuncs(State<CubeInputs> flocking) {
        flocking.OnUpdate += () => { 
            _followPathBehaviour.OnUpdate();

            if(_flocking.target.gameObject.layer != 8) {//player
                return;
            }

            if (Utility.InRangeSquared(_flocking.target.position, transform.position, radiusToAttack)) { 
                SendInputToFSM(CubeInputs.InRadius);
            }
        };

        flocking.OnFixedUpdate += () => {
            _flocking.OnFixedUpdate();
        };
    }

    void SetStoppedFuncs(State<CubeInputs> stopped) {
        stopped.OnEnter += x => {
            StartCoroutine(StoppedRoutine());
        };

        stopped.OnUpdate += () => {
            transform.forward = (Utility.SetYInVector3(_flocking.target.position, transform.position.y) - transform.position).normalized;
        };
    }

    IEnumerator StoppedRoutine() {
        yield return new WaitForSeconds(stoppedTime);
        if(_flocking.target.gameObject.layer == 8) {//player 
            if (Utility.InRangeSquared(_flocking.target.position, transform.position, radiusToAttack)) {
                SendInputToFSM(CubeInputs.InRadius);
            }
            else {
                SendInputToFSM(CubeInputs.NotInRadius);
            }
        }
    }

    void SetShootFuncs(State<CubeInputs> shoot) {
        shoot.OnEnter += x => {
            StartCoroutine(ShootRoutine());
        };
    }

    IEnumerator ShootRoutine() { 
        int counter = -Mathf.FloorToInt(quantityOfShots/2);

        for (int i = 0; i < quantityOfShots; i++) { 
            var b = EnemyBulletManager.instance.giveMeEnemyBullet();
            b.SetPos(shotPos.position).gameObject.SetActive(true);
            if (counter == 0) {
                b.SetDir(shotPos.forward);
            }
            else {
                b.SetDir(Quaternion.AngleAxis(angleBetweenShots * counter, transform.up) * transform.forward);
            }
            counter++;
        } 

        yield return new WaitForSeconds(stoppedTime);

        
        if(_flocking.target.gameObject.layer == 8) {//player  
            if (Utility.InRangeSquared(_flocking.target.position, transform.position, radiusToAttack)) {
                SendInputToFSM(CubeInputs.InRadius);
            }
            else {
                SendInputToFSM(CubeInputs.NotInRadius);
            }
        }
    }

    void SetEvadeFuncs(State<CubeInputs> evade) {
        evade.OnEnter += x => {
            var r = Random.Range(minRadiusOfEvade, maxRadiusOfEvade);
            //Debug.Log(r);
            //_transformToFollowOnEvade.transform.position = Utility.RandomVector3InRadiusCountingBoundaries(transform.position,r,blockEnemyViewToPlayer);
            _transformToFollowOnEvade.transform.position = Utility.BestVector3InRectDirectionsInRadiusCountingBoundaries(transform.position,_flocking.target.position, r, blockEnemyViewToPlayer);
            _flocking.target = _transformToFollowOnEvade.transform;

            imageTargetOnEvade.enabled = true;

            if(_uiLine != null) { 
                _uiLine.ActivateLine(_transformToFollowOnEvade.transform, Color.yellow);
            }
        };

        evade.OnUpdate += () => {
            if(Utility.InRangeSquared(_flocking.target.position,transform.position, radiusOfTranformToFollowOnEvade)) {
                SendInputToFSM(CubeInputs.FinishedEvade);
            }
            //ImageTilting();
            imageTargetOnEvade.transform.Rotate(Vector3.forward, speedToRotateImage * Time.deltaTime);
        };

        evade.OnFixedUpdate += () => {
            _flocking.OnFixedUpdate();
        };

        evade.OnExit += x => {
            _flocking.target = EnemiesManager.instance.player.transform;
            if(_uiLine != null) { 
                _uiLine.DeactivateLine();
            }
            imageTargetOnEvade.enabled = false;
            _flocking.resetVelocity();
        };
    }

    private void Update() {
        if(_eIntegration != null && !_eIntegration.LoadingNotComplete) {  
            _myFsm.Update();
            //Debug.Log(_flocking.target.name);
        } 
    }

    void FixedUpdate () {
        if (_flocking == null || _eIntegration == null)// || _anim == null)
            return;

        if (!_eIntegration.LoadingNotComplete) { 
            _myFsm.FixedUpdate();
            //_anim.speed = SectionManager.instance.EnemiesMultiplicator; 
        }
        //else _anim.speed = 0f;
    }

    void SendInputToFSM(CubeInputs inp) {
        _myFsm.SendInput(inp);
    }

    public void OnHit(int damage) {
        _hitsRemaining -= damage;
        AbstractOnHitWhiteAction();
        if (_hitsRemaining <= 0) { 
            EnemiesManager.instance.ReturnCubeEnemyToPool(this);
            gameObject.SetActive(false);
            EventManager.instance.ExecuteEvent(Constants.ENEMY_DEAD, new object[] { _actualWave, _actualSectionNode, this, false, hasToDestroyThisToUnlockSomething });
        }
    } 

    public CubeEnemyBehaviour SetPosition(Vector3 pos) {
        transform.position = pos;
        _hitsRemaining = hitsCanTake;
        //if (_anim == null)
        //    _anim = GetComponent<Animator>();

        _flocking.resetVelocity();
        return this;
    } 

    public CubeEnemyBehaviour SetTarget(Transform target) {
        if (_flocking == null)
            _flocking = GetComponent<Flocking>();

        if (_followPathBehaviour == null)
            _followPathBehaviour = new FollowPathBehaviour(this, blockEnemyViewToPlayer, _flocking/*, true*/);

        if(_uiLine == null) {
            _uiLine = GetComponent<MakeUILine>();
        }

        _followPathBehaviour.SetActualSectionNode(_actualSectionNode);

        _flocking.target = target;
        return this;
    }

    private void OnTriggerEnter(Collider c) {
        if (c.gameObject.layer != 12 && c.gameObject.layer != 13 && c.gameObject.layer != 14 && c.gameObject.layer != 0 && _flocking != null) {//enemy //powerup // enemybullet
            _flocking.resetVelocity(); 
            if(_myFsm.Current.Name == "Evade") {
                SendInputToFSM(CubeInputs.FinishedEvade);
            }
        }
    }

    void OnDisable() {
        if(_followPathBehaviour != null) { 
            _followPathBehaviour.OnDisable();
        }

        if(_uiLine != null) {
            _uiLine.DeactivateLine();
        }

        if(imageTargetOnEvade != null) { 
            imageTargetOnEvade.enabled = false;
        }
    }

    void OnDrawGizmos() {
        if (_transformToFollowOnEvade == null)
            return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(_transformToFollowOnEvade.transform.position, radiusOfTranformToFollowOnEvade);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiusToAttack);
    }
}
