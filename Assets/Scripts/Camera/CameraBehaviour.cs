using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraBehaviour : MonoBehaviour , IPauseable {
    [Header("Player")]
    public Transform target;

    [Header("OnFollowPlayer")]
    public float followSmoothSpeed = 0.1f;
    public Vector3 offset;

    [Header("OnStationary")]
    public float followSmoothSpeedStationary = 0.001f;
    public Vector3 offsetStationary;

    [Header("BossCameraActivated?")]
    public bool cameraOnBossActivated;

    [Header("BossCamera")]
    public Vector3 cameraPivotOnBoss;
    public Vector3 cameraOffsetOnBoss;
    public float clampMovementX;
    public float clampMovementZ;
    public float smoothSpeedOnBoss;

    [Header("CameraShake")]
    public float playerDeadShakeDuration = 1f;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    float _shakeCountdown;   
    ICameraStrategy _strategyBehaviour;
    StationaryPosStrategy _stationaryStrategy;

    Dictionary<string, ICameraStrategy> _allStrats = new Dictionary<string, ICameraStrategy>();

    const string ON_FOLLOW = "OnFollow";
    const string ON_BOSS_NODE = "OnBossNode";
    const string ON_STATIONARY = "OnStationary";
    private float digitalGlitchIntensity=0.25f;
    private float colorDrift = 0.25f;
    private float scanLine = 0.25f;

    bool _paused;
    public void OnPauseChange(bool v)
    {
        _paused = v;
    }

    void Start() { 
        _allStrats.Add(ON_FOLLOW, new OnFollowPlayerStrategy(this));
        _allStrats.Add(ON_BOSS_NODE, new OnBossNodeStrategy(this));

        _stationaryStrategy = new StationaryPosStrategy(this);
        _allStrats.Add(ON_STATIONARY, _stationaryStrategy);


        _strategyBehaviour = _allStrats[ON_FOLLOW];

        EventManager.instance.SubscribeEvent(Constants.CAMERA_ON_FOLLOW_PLAYER, OnCameraFollowPlayer);
        EventManager.instance.SubscribeEvent(Constants.CAMERA_ON_BOSS, OnCameraBoss);
        EventManager.instance.SubscribeEvent(Constants.CAMERA_STATIONARY, OnCameraStationary);

        EventManager.instance.SubscribeEvent(Constants.PLAYER_DEAD, OnPlayerDead);
        EventManager.instance.SubscribeEvent(Constants.BOSS_DESTROYED, OnBossDestroyed);
    }

    private void OnCameraStationary(object[] parameterContainer) { 
        _stationaryStrategy.SetStationaryPos((Vector3)parameterContainer[0]); 
        _strategyBehaviour = _allStrats[ON_STATIONARY];
    }

    private void OnBossDestroyed(object[] parameterContainer) {
        _shakeCountdown = (float)parameterContainer[0];
        StartCoroutine(ShakeRoutine()); 
    }

    void FixedUpdate() {
        if (_paused)
            return;

        _strategyBehaviour.OnFixedUpdate();
    } 

    void OnCameraFollowPlayer(params object[] param) { 
        _strategyBehaviour = _allStrats[ON_FOLLOW];
    }

    void OnCameraBoss(params object[] param) {
        if (!cameraOnBossActivated)
            return;
         
        _strategyBehaviour = _allStrats[ON_BOSS_NODE];
    } 

    void OnPlayerDead(params object[] param) {
        _shakeCountdown = playerDeadShakeDuration;
        AnalogGlitch aGlitch = this.gameObject.GetComponent<AnalogGlitch>();
        aGlitch.scanLineJitter = scanLine;
        aGlitch.colorDrift = colorDrift;
        DigitalGlitch glitch = this.gameObject.GetComponent<DigitalGlitch>();
        glitch.intensity = digitalGlitchIntensity;
        StartCoroutine(ShakeRoutine());
    } 

    IEnumerator ShakeRoutine() {
        while (_shakeCountdown > 0)  {

            while (_paused)
                yield return null;

            if (_shakeCountdown < playerDeadShakeDuration/2)
                transform.position = transform.position + UnityEngine.Random.insideUnitSphere * shakeAmount/3;
            else
                transform.position = transform.position + UnityEngine.Random.insideUnitSphere * shakeAmount;

            _shakeCountdown -= Time.deltaTime * decreaseFactor;

            yield return null; 
        }
        yield return new WaitForSeconds(1f);

        while (_paused)
            yield return null;

        AnalogGlitch aGlitch = this.gameObject.GetComponent<AnalogGlitch>();
        aGlitch.scanLineJitter = 0;
        aGlitch.colorDrift = 0;
        DigitalGlitch glitch = this.gameObject.GetComponent<DigitalGlitch>();
        glitch.intensity = 0;
    } 
}
