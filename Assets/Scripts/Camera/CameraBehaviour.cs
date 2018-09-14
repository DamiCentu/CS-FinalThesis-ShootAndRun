using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraBehaviour : MonoBehaviour {
    [Header("Player")]
    public Transform target;

    [Header("OnFollowPlayer")]
    public float followSmoothSpeed = 0.1f;
    public Vector3 offset;

    [Header("BossCameraActivated?")]
    public bool cameraOnBossActivated;

    [Header("BossCamera")]
    public Vector3 cameraPivotOnBoss;
    public Vector3 cameraOffsetOnBoss;
    public float clampMovementX;
    public float clampMovementZ;
    public float smoothSpeedOnBoss;

    [Header("CameraShake")]
    public float shakeDuration = 1f;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    float _shakeCountdown;   
    ICameraStrategy _strategyBehaviour;

    Dictionary<string, ICameraStrategy> _allStrats = new Dictionary<string, ICameraStrategy>();

    const string ON_FOLLOW = "OnFollow";
    const string ON_BOSS_NODE = "OnBossNode";

    void Start() { 
        _allStrats.Add(ON_FOLLOW, new OnFollowPlayerStrategy(this));
        _allStrats.Add(ON_BOSS_NODE, new OnBossNodeStrategy(this));

        _strategyBehaviour = _allStrats[ON_FOLLOW];

        EventManager.instance.AddEvent(Constants.CAMERA_ON_FOLLOW_PLAYER);
        EventManager.instance.AddEvent(Constants.CAMERA_ON_BOSS );
        EventManager.instance.SubscribeEvent(Constants.CAMERA_ON_FOLLOW_PLAYER, OnFollow);
        EventManager.instance.SubscribeEvent(Constants.CAMERA_ON_BOSS, OnLookAt);

        EventManager.instance.SubscribeEvent(Constants.PLAYER_DEAD, OnPlayerDead);
    } 

    void FixedUpdate() { 
        _strategyBehaviour.OnFixedUpdate();
    } 

    void OnFollow(params object[] param) { 
        _strategyBehaviour = _allStrats[ON_FOLLOW];
    }

    void OnLookAt(params object[] param) {
        if (!cameraOnBossActivated)
            return;
         
        _strategyBehaviour = _allStrats[ON_BOSS_NODE];
    } 

    void OnPlayerDead(params object[] param) {
        _shakeCountdown = shakeDuration;
        StartCoroutine(ShakeRoutine());
    } 

    IEnumerator ShakeRoutine() {
        while (_shakeCountdown > 0)  {
             
            if(_shakeCountdown < shakeDuration/2)
                transform.position = transform.position + UnityEngine.Random.insideUnitSphere * shakeAmount/3;
            else
                transform.position = transform.position + UnityEngine.Random.insideUnitSphere * shakeAmount;

            _shakeCountdown -= Time.deltaTime * decreaseFactor;

            yield return null;
        } 
    } 
}
