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
    //public AudioSource[] playerShootAudioSources;
    public AudioSource playerShootAudioSource;
    public AudioSource HigherRange;
    public AudioSource shield;
    public AudioSource ExtraDash;
    public AudioSource DoubleShoot;
    public AudioSource enemyExplotion;
    public AudioSource portalFadeIn;
    public AudioSource portalFadeOut;
    public AudioSource portalFadeLoop;

    void Awake()
    {
        instance = this;

    }
    void Start () {
        EventManager.instance.SubscribeEvent(Constants.PLAYER_DEAD, OnPlayerDead);
        EventManager.instance.SubscribeEvent(Constants.ENEMY_DEAD, OnEnemyDead);
        EventManager.instance.SubscribeEvent(Constants.SOUND_FADE_OUT, OnSoundFadeOut);//fade out es cuando reaparece
        EventManager.instance.SubscribeEvent(Constants.SOUND_FADE_IN, OnSoundFadeIn);//fade in es cuando esta ingresando al portal
    }

    void OnSoundFadeOut(object[] parameterContainer) {
        portalFadeLoop.Stop();
        portalFadeOut.Play();
        //StartCoroutine(VolumeEffectRoutine(portalFadeOut, false));
    }

    void OnSoundFadeIn(object[] parameterContainer) {
        portalFadeIn.Play();
        //StartCoroutine(VolumeEffectRoutine(portalFadeIn, true));
        StartCoroutine(PlayLoopSoundRoutine());
    }
    
    IEnumerator VolumeEffectRoutine(AudioSource aS, bool ascending) {
        var tempVol = aS.volume;
        var time = 0f;
        var halfClipTime = aS.clip.length /aS.pitch / 2;
        if (ascending) { 
            while (time < tempVol) {
                if (aS.time < halfClipTime) { 
                    aS.volume = Mathf.Lerp(0, tempVol, time / halfClipTime);
                    time += Time.deltaTime * 1f;
                }
                yield return null;
            }
        }
        else {
            time = tempVol;
            while (time > 0) {
                if(aS.time > halfClipTime) { 
                    aS.volume = Mathf.Lerp( tempVol,0, time / halfClipTime);
                    time -= Time.deltaTime * 1f ;
                }
                yield return null;
            }
        }
        aS.volume = tempVol;
    }

    IEnumerator PlayLoopSoundRoutine() {
        while (portalFadeIn.isPlaying) { 
            yield return null;
        }
        portalFadeLoop.Play(); 
    } 

    void OnEnemyDead(object[] parameterContainer) {
        enemyExplotion.Play();
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

    internal void PlayPlayerShoot() { 
        playerShootAudioSource.Play();
    }

    internal void StopPlayerShoot()
    {
        playerShootAudioSource.Stop();
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
