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

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) { 
            if(!pause) { 
                pause = true;
                _particlesSys = FindObjectsOfType<ParticleSystem>().Where(x => x.gameObject.activeSelf && x.isPlaying).ToArray();
                _lineRends = FindObjectsOfType<LineRenderer>().Where(x => x.gameObject.activeSelf && x.enabled ).ToArray();
                //_trailRends = FindObjectsOfType<TrailRenderer>().Where(x => x.gameObject.activeSelf && x.enabled).ToArray();
                _animators = FindObjectsOfType<Animator>().Where(x => x.gameObject.activeSelf && x.enabled).ToArray();
                _audioSources = FindObjectsOfType<AudioSource>().Where(x => x.gameObject.activeSelf && x.enabled && x.isPlaying).ToArray();
            }
            else {
                pause = false;
            }
            
            Pause(pause, FindObjectsOfType<MonoBehaviour>().Where(x => x.gameObject.activeSelf).OfType<IPauseable>().ToArray());
            Pause(pause, _particlesSys);
            //Pause(pause, _trailRends);
            Pause(pause, _lineRends);
            Pause(pause, _animators);
            Pause(pause, _audioSources);
        } 
    }

    void Pause(bool v, IPauseable[] _allPauseables) {
        foreach (var p in _allPauseables) {
            p.OnPauseChange(v);
        }
    }

     void Pause(bool v, ParticleSystem[] _allParticles) {
        if(v) { 
            foreach (var p in _allParticles) {
                if (p != null)
                    p.Pause();
            }
        }
        else {
            foreach (var p in _allParticles) {
                if(p!=null)
                  p.Play();
            }
        }
    }

    void Pause(bool v, TrailRenderer[] _allTrails) {
        foreach (var p in _allTrails) {
            p.enabled = !v;
        }
    }

    void Pause(bool v, LineRenderer[] _allLines) {
        foreach (var p in _allLines) {
            p.enabled = !v;
        }
    }

    void Pause(bool v, AudioSource[] _allAudioSources) {
        if(v) { 
            foreach (var p in _allAudioSources) {
                p.Pause();
            }
        }
        else {
            foreach (var p in _allAudioSources) {
                p.UnPause();
            }
        }
    }

    void Pause(bool v, Animator[] _allAnimators) {
        foreach (var p in _allAnimators) {
            p.enabled = !v;
        }
    }
}
