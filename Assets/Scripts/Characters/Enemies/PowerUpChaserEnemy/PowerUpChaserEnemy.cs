using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FSMFUNCTIONAL;

[RequireComponent(typeof(Flocking))]
public class PowerUpChaserEnemy : AbstractEnemy, IHittable {

    public int hitsCanTake = 5;
    public float timeSplicingQuote = 0.001f;
    public float timeToWayToTryFindPowerUp = 1f;
    public LayerMask blockEnemyViewToTarget;

    Flocking _flocking;
    Animator _anim;
    FollowPathBehaviour _followPathBehaviour;
    MakeUILine _uiLine;

    EventFSM<ChaserInputs> _myFsm;

    int _hitsRemaining = 0;

    enum ChaserInputs {
        PowerUpDropped,
        PowerUpPickedByPlayer,
        FinishedIntegration,
        ResetFSM
    }

    const string FOLLOW_POWER_UP = "FollowPowerUp";
    const string FOLLOW_PLAYER = "FollowPlayer";
    const string INTEGRATION = "Integration";

    void Start() {
        SetFsm();
    }

    void SetFsm() {  
        var followPowerUp = new State<ChaserInputs>(FOLLOW_POWER_UP);
        var integration = new State<ChaserInputs>(INTEGRATION);
        var followUser = new State<ChaserInputs>(FOLLOW_PLAYER);

        StateConfigurer.Create(followPowerUp)
            .SetTransition(ChaserInputs.PowerUpPickedByPlayer, followUser)
            .SetTransition(ChaserInputs.PowerUpDropped, followPowerUp)
            .SetTransition(ChaserInputs.ResetFSM, integration) 
            .Done();

        StateConfigurer.Create(followUser)
            .SetTransition(ChaserInputs.PowerUpDropped, followPowerUp)
            .SetTransition(ChaserInputs.PowerUpPickedByPlayer, followUser)
            .SetTransition(ChaserInputs.ResetFSM, integration)
            .Done();

        StateConfigurer.Create(integration)
            .SetTransition(ChaserInputs.FinishedIntegration, followPowerUp) 
            .Done();

        SetFollowPlayerFuncs(followUser);
        SetFollowPowerUpFuncs(followPowerUp);
        SetIntegrationFuncs(integration);

        _myFsm = new EventFSM<ChaserInputs>(integration);
    }

    //---------------------------------------------------------------------INTEGRATION

    void SetIntegrationFuncs(State<ChaserInputs> integration) {
        integration.OnEnter += x => { 
            _anim.speed = 0f;
        };

        integration.OnUpdate += () => {
            if (!_eIntegration.LoadingNotComplete) {
                SendInputToFSM(ChaserInputs.FinishedIntegration);
            } 
        }; 
    }

    //---------------------------------------------------------------------FOLLOW PLAYER

    void SetFollowPlayerFuncs(State<ChaserInputs> followUser) {
        followUser.OnEnter += x =>  {
            _flocking.target = EnemiesManager.instance.player.transform;
        };

        followUser.OnUpdate += () => { 
            if(LootTableManager.instance.ExistAPowerUp) {
                SendInputToFSM(ChaserInputs.PowerUpDropped);
            } 
            _followPathBehaviour.OnUpdate(); 
        };

        followUser.OnFixedUpdate += () => {
            _anim.speed = SectionManager.instance.EnemiesMultiplicator;
            _flocking.OnFixedUpdate();
        };
    }

    //---------------------------------------------------------------------FOLLOW POWER UP

    void SetFollowPowerUpFuncs(State<ChaserInputs> followPowerUp) {
        followPowerUp.OnEnter += x => {
            var p = LootTableManager.instance.ClosestPowerUp(transform.position);

            if (p == null) { 
                SendInputToFSM(ChaserInputs.PowerUpPickedByPlayer);
                return;
            }

            _flocking.target = p.transform;
            _followPathBehaviour.SetPowerUpToChase(p.transform);
            _followPathBehaviour.HasToFollowPlayer = false;

            if (_uiLine != null) {
                _uiLine.ActivateLine(p.transform, Color.red);
            }
        };

        followPowerUp.OnUpdate += () => {
            if(_flocking.target == null) {
                SendInputToFSM(ChaserInputs.PowerUpPickedByPlayer);
                _followPathBehaviour.HasToFollowPlayer = false;
            }
            _followPathBehaviour.OnUpdate(); 
        };

        followPowerUp.OnFixedUpdate += () => {
            _anim.speed = SectionManager.instance.EnemiesMultiplicator;
            _flocking.OnFixedUpdate();
        };

        followPowerUp.OnExit += x => {
            _uiLine.DeactivateLine();
        };

    }

    public PowerUpChaserEnemy SetStart() {
        if (_flocking == null)
            _flocking = GetComponent<Flocking>();

        if (_anim == null)
            _anim = GetComponent<Animator>();

        if (_followPathBehaviour == null)
            _followPathBehaviour = new FollowPathBehaviour(this, blockEnemyViewToTarget, _flocking/*, false*/);

        if(_uiLine == null) {
            _uiLine = GetComponent<MakeUILine>();
        }

        _followPathBehaviour.SetActualSectionNode(_actualSectionNode);

        _hitsRemaining = hitsCanTake; 

        _flocking.resetVelocity();
        
        if(_myFsm != null) { 
            SendInputToFSM(ChaserInputs.ResetFSM);
        }

        return this;
    } 

    void OnDisable() {
        if(_uiLine != null) {
            _uiLine.DeactivateLine();
        }

        if(_followPathBehaviour != null) { 
            _followPathBehaviour.OnDisable();
        }
    }

    public PowerUpChaserEnemy SetPosition(Vector3 pos) {
        transform.position = pos;
        return this;
    }

    void Update() {
        _myFsm.Update(); 
    }

    void FixedUpdate() {
        _myFsm.FixedUpdate();
    } 

    public void OnHit(int damage) {
        _hitsRemaining -= damage;

        AbstractOnHitWhiteAction();

        if (_hitsRemaining > 0)
            return; 

        OnDestroyCustom(false); 
    }

    void OnTriggerEnter(Collider c) {
        if (c.gameObject.layer != 12  && c.gameObject.layer != 0 && c.gameObject.layer != 14) {//enemy 
            if(_flocking.target != null && _flocking != null)
                _flocking.resetVelocity(); 
        }

        if (c.gameObject.layer == 13) {//powerup
            _actualSectionNode.SpawnMiniBoss(transform.position,_actualWave);
            OnDestroyCustom(true);
            EventManager.instance.ExecuteEvent(Constants.POWER_UP_PICKED, new object[] { c.gameObject }); 
        }
    }

    void OnDestroyCustom(bool makeParticles) {
        StopAllCoroutines();
        EnemiesManager.instance.ReturnChaserEnemyToPool(this);
        gameObject.SetActive(false);
        EventManager.instance.ExecuteEvent("EnemyDead", new object[] { _actualWave, _actualSectionNode, this, makeParticles, hasToDestroyThisToUnlockSomething });//true es para saber si toco 1 power up para en el destroy en el section node tirar el evento de particle
    }

    void SendInputToFSM(ChaserInputs inp) {
        _myFsm.SendInput(inp);
    }
}