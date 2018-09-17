using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SoundManager : MonoBehaviour {
    public AudioMixerSnapshot amsDefault;
    public AudioMixerSnapshot amsOnPlayerDead;
    public AudioSource asPlayerDead;
    public enum Snapshot { Default, OnPlayerDead }
    public Snapshot currentSnapshot;
    public float timeToTransition=2;
    internal static SoundManager instance;
    public AudioSource spawnSound;
    public AudioSource playerShootSound;
    public AudioSource HigherRange;
    public AudioSource shield;
    public AudioSource ExtraDash;
    public AudioSource DoubleShoot;

    void Awake()
    {
        instance = this;

    }
    void Start () {
        EventManager.instance.SubscribeEvent(Constants.PLAYER_DEAD, OnPlayerDead);

    }

    private void Default(object[] parameterContainer)
    {
        print("paso a default");
        if (currentSnapshot != Snapshot.Default) {
            amsDefault.TransitionTo(timeToTransition);
            currentSnapshot = Snapshot.Default;
        }
    }

    private void OnPlayerDead(object[] parameterContainer)
    {
        if (currentSnapshot != Snapshot.OnPlayerDead)
        {
            amsOnPlayerDead.TransitionTo(timeToTransition);
            currentSnapshot = Snapshot.OnPlayerDead;
            StartCoroutine("ReturnDefault");
            asPlayerDead.Play();
        }

    }

    IEnumerator ReturnDefault()
    {
        yield return new WaitForSeconds(2.5f);
        if (currentSnapshot != Snapshot.Default)
        {
            amsDefault.TransitionTo(timeToTransition);
            currentSnapshot = Snapshot.Default;
        }
    }

    internal void PlayDoubleShoot()
    {
        DoubleShoot.Play();
    }

    internal void PlaySpawnEnemy()
    {
        spawnSound.Play();
    }
    internal void PlayPlayerShoot()
    {
        if (!playerShootSound.isPlaying) {

            playerShootSound.Play();
        }

    }
    internal void StopPlayerShoot()
    {
        playerShootSound.Play();
    }

    internal void PlayHigherRange()
    {
        HigherRange.Play();
    }

    internal void PlayShield()
    {
        shield.Play();
    }

    internal void PlayExtraDash()
    {
        ExtraDash.Play();
    }
}
