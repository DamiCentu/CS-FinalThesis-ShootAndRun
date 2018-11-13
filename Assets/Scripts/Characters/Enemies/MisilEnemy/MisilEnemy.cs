using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Flocking))]
public class MisilEnemy : AbstractEnemy, IHittable {

    public LayerMask blockEnemyViewToPlayer;
    public Missile Missile;
    public float timeBetweenMissiles;
    public float timeToBoom;
    public float maxOffset;
    public float _timer=-20;
    public GameObject cañon;

    private Vector3 defaultRot = new Vector3(0, -90, -90);

    Animator _anim;

    private LayerMask misileEnemyLayerMask;

    public int hitsCanTake = 10;
    public Transform target;
    public  Transform spawnMissilesPosition;
    int _hitsRemaining;
    bool _canInteract = true;

    void FixedUpdate() {
        if (_eIntegration != null && _eIntegration.LoadingNotComplete || !_canInteract) {
            if(_anim != null) {
                _anim.speed = 0;
            }
            return;
        }

        _anim.speed = SectionManager.instance.EnemiesMultiplicator;
        _timer += Time.deltaTime;
        if (_timer > timeBetweenMissiles) {
            float angle = Vector3.SignedAngle(cañon.transform.position, target.position, Vector3.right);
            cañon.transform.rotation = Quaternion.Euler(0, 90, -90); 
            DropMissile(target.position); 
            _timer = 0;
        }
    }

    private void DropMissile(Vector3 playerPosition) {
        float xPosition = playerPosition.x + UnityEngine.Random.Range(-maxOffset, maxOffset);
        float zPosition = playerPosition.z + UnityEngine.Random.Range(-maxOffset, maxOffset);
        Vector3 destination = new Vector3(xPosition, playerPosition.y + 0.3f, zPosition);

        Missile mis = Instantiate(Missile, spawnMissilesPosition.position, Quaternion.FromToRotation(spawnMissilesPosition.position, destination));

        mis.Set(destination, timeToBoom);
    }

    public void OnHit(int damage) {
        _hitsRemaining -= damage;
        AbstractOnHitWhiteAction();
        if (_hitsRemaining <= 0) {
        AbstractOnHitWhiteAction();
            EnemiesManager.instance.ReturnMisilEnemyToPool(this);
            StopAllCoroutines();
            gameObject.SetActive(false);
            UnsubscribeToEvents();
            EventManager.instance.ExecuteEvent(Constants.ENEMY_DEAD, new object[] { _actualWave, _actualSectionNode, this, false, hasToDestroyThisToUnlockSomething, wallToUnlockID });
        }
    }

    public MisilEnemy SetPosition(Vector3 pos) {
        _hitsRemaining = hitsCanTake;
        if (_anim == null)
            _anim = GetComponentInChildren<Animator>();
        _canInteract = true;
        _anim.speed = 0;
        transform.position = pos;
        cañon.transform.rotation = Quaternion.Euler(0, 90, -90);
        _timer = 0; 
        return this;
    } 

    public MisilEnemy SetTarget(Transform player)
    {
        target = player;
        return this;
    }

    void OnDisable()
    {

    } 

    private void OnTriggerEnter(Collider c)
    {

    }

    public MisilEnemy SubscribeToEvents() {
        _canInteract = false;
        EventManager.instance.SubscribeEvent(Constants.PLAYER_CAN_MOVE, OnPlayerCanMove);
        EventManager.instance.SubscribeEvent(Constants.PLAYER_DEAD, OnPlayerDead);
        return this;
    }

    public MisilEnemy UnsubscribeToEvents() {
        EventManager.instance.UnsubscribeEvent(Constants.PLAYER_CAN_MOVE, OnPlayerCanMove);
        EventManager.instance.UnsubscribeEvent(Constants.PLAYER_DEAD, OnPlayerDead);
        return this;
    }

    private void OnPlayerCanMove(object[] parameterContainer) {
        if (SectionManager.instance.actualNode != _actualSectionNode || !isActiveAndEnabled) {
            return;
        }
        _canInteract = true;
    }

    private void OnPlayerDead(object[] parameterContainer) { 
        _canInteract = false;
    }
}
