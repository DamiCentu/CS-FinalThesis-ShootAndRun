using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PauseGame : MonoBehaviour {
    bool pause = false;

    LineRenderer[] _lineRends;
    TrailRenderer[] _trailRends;
    ParticleSystem[] _particlesSys;

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) { 
            if(!pause) { 
                pause = true;
                _particlesSys = FindObjectsOfType<ParticleSystem>().Where(x => x.isPlaying).ToArray();
                _lineRends = FindObjectsOfType<LineRenderer>().Where(x => x.enabled).ToArray();
                _trailRends = FindObjectsOfType<TrailRenderer>().Where(x => x.enabled).ToArray();
            }
            else {
                pause = false;
            }
            
            Pause(pause, FindObjectsOfType<MonoBehaviour>().OfType<IPauseable>().ToArray());
            Pause(pause, _particlesSys);
            Pause(pause, _trailRends);
            Pause(pause, _lineRends);
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
                p.Pause();
            }
        }
        else {
            foreach (var p in _allParticles) {
                p.Play();
            }
        }
    }

    void Pause(bool v, TrailRenderer[] _allTrails) {
        if(v) { 
            foreach (var p in _allTrails) {
                p.enabled = false;
            }
        }
        else {
            foreach (var p in _allTrails) {
                p.enabled = true ;
            }
        }
    }

    void Pause(bool v, LineRenderer[] _allLines) {
        if(v) { 
            foreach (var p in _allLines) {
                p.enabled = false;
            }
        }
        else {
            foreach (var p in _allLines) {
                p.enabled = true ;
            }
        }
    }
}
