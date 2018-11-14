using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitWhiteFeedback : MonoBehaviour {

    public float whiteTimeOnHit = 0.1f; 
    public Material whiteMat;

    Material mainMat; 
    SkinnedMeshRenderer[] _skinnedRnds;
    MeshRenderer[] _meshRnds;

    bool _hitted;
    float _time;

    bool _paused;
    public void OnPauseChange(bool v) {
        _paused = v;
    }

    void Update () {
        if (_paused)
            return;

        if (_hitted) {
            OnHitCheck();
            _time += Time.deltaTime;
        }
	}

    //esto se llama cuando el enemigo es golpeado por algo
    public void OnHit() {
        EventManager.instance.ExecuteEvent(Constants.SOUND_BULLET_HIT);
        _time = 0;
        _hitted = true;
        _alreadyChangedMat = false;
    }

    //esto se llama cuando el enemigo es seteado en la escena
    public void SetFeedback() {
        if(_skinnedRnds == null) { 
            _skinnedRnds = GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        if (_meshRnds == null) { 
            _meshRnds = GetComponentsInChildren<MeshRenderer>();
        }

        if (_skinnedRnds.Length > 0) { 
            if(mainMat == null) { 
                mainMat = _skinnedRnds[0].material;
            }
            foreach (var mt in _skinnedRnds) { 
                mt.material = mainMat;
            }
        }

        if (_meshRnds.Length > 0) {
            if (mainMat == null) { 
                mainMat = _meshRnds[0].material;
            }
            foreach (var mt in _meshRnds) { 
                mt.material = mainMat;
            }
        }

        ResetMat();
    } 

    //esto se deberia llamar para volver a poner el material original 
    void ResetMat() {
        _hitted = false;
        _alreadyChangedMat = false;
        _time = 0;

        if (_skinnedRnds.Length > 0) { 
            foreach (var mt in _skinnedRnds) {
                if (mt == null)
                    continue;
                mt.material = mainMat;
            }
        }

        if (_meshRnds.Length > 0) { 
            foreach (var mt in _meshRnds) {
                if (mt == null)
                    continue;
                mt.material = mainMat;
            }
        }
    }

    bool _alreadyChangedMat;

    //este metodo chequea el tiempo transcurrido para volver a poner el material correspondiente
    void OnHitCheck() {  
        if(_time < whiteTimeOnHit) {
            if (_alreadyChangedMat) {
                return;
            }

            if (_skinnedRnds.Length > 0) { 
                foreach (var mt in _skinnedRnds) {
                    if (mt == null)
                        continue;
                    mt.material = whiteMat;
                }
            }

            if (_meshRnds.Length > 0) { 
                foreach (var mt in _meshRnds) {
                    if (mt == null)
                        continue;
                    mt.material = whiteMat;
                }
            }
            _alreadyChangedMat = true;
        }
        else { //---------------------------------------------
            ResetMat();
            _alreadyChangedMat = false;
            _hitted = false;
            _time = 0;
        }
    }
}
