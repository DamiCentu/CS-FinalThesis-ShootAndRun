using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PauseGame : MonoBehaviour {
    bool pause = false;

    LineRenderer[] _lineRends;
    //TrailRenderer[] _trailRends;
    Animator[] _animators;
    ParticleSystem[] _particlesSys;
    AudioSource[] _audioSources;

    void Start()
    {
        EventManager.instance.SubscribeEvent(Constants.PAUSE_OR_UNPAUSE, OnPauseOrUnpause);
    }

    void OnPauseOrUnpause(object[] param)
    {
        if (param.Length > 0 && (bool)param[0])
            AllPauses(true);
        else
            AllPauses();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AllPauses();
        } 
    }

    void AllPauses(bool playerIsAboutToBeDestroyed = false)
    {
        if (!pause)
        {
            pause = true;
            _particlesSys = FindObjectsOfType<ParticleSystem>().Where(x => x.gameObject.activeSelf && x.isPlaying).ToArray();
            _lineRends = FindObjectsOfType<LineRenderer>().Where(x => x.gameObject.activeSelf && x.enabled).ToArray();
            //_trailRends = FindObjectsOfType<TrailRenderer>().Where(x => x.gameObject.activeSelf && x.enabled).ToArray();
            _animators = FindObjectsOfType<Animator>().Where(x => x.gameObject.activeSelf && x.enabled).ToArray();
            _audioSources = FindObjectsOfType<AudioSource>().Where(x => x.gameObject.activeSelf && x.enabled && x.isPlaying).ToArray();
        }
        else
        {
            pause = false;
        }

        if(!playerIsAboutToBeDestroyed)
            Pause(pause, FindObjectsOfType<MonoBehaviour>().Where(x => x.gameObject.activeSelf).OfType<IPauseable>().ToArray());
        else
            Pause(pause, FindObjectsOfType<MonoBehaviour>().Where(x => x.gameObject.activeSelf && !(x is Player)).OfType<IPauseable>().ToArray());

        Pause(pause, _particlesSys);
        //Pause(pause, _trailRends);
        Pause(pause, _lineRends);
        Pause(pause, _animators);
        Pause(pause, _audioSources);
    }

    void Pause(bool v, IPauseable[] _allPauseables) {
        foreach (var p in _allPauseables) {
            if (p != null)
                p.OnPauseChange(v);
        }
    }

     void Pause(bool pause, ParticleSystem[] _allParticles) {
        if(pause) { 
            foreach (var p in _allParticles) {
                if (p)
                    p.Pause();
            }
        }
        else {
            foreach (var p in _allParticles) {
                if(p)
                  p.Play();
            }
        }
    }

    void Pause(bool pause, TrailRenderer[] _allTrails) {
        foreach (var p in _allTrails) {
            if (p)
                p.enabled = !pause;
        }
    }

    void Pause(bool v, LineRenderer[] _allLines) {
        foreach (var p in _allLines) {
            if (p)
                p.enabled = !v;
        }
    }

    void Pause(bool v, AudioSource[] _allAudioSources) {
        if(v) { 
            foreach (var p in _allAudioSources) {
                if (p)
                    p.Pause();
            }
        }
        else {
            foreach (var p in _allAudioSources) {
                if (p)
                    p.UnPause();
            }
        }
    }

    void Pause(bool v, Animator[] _allAnimators) {
        foreach (var p in _allAnimators) {
            if(p)
                p.enabled = !v;
        }
    }
}
