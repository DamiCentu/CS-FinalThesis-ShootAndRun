using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEnemy : MonoBehaviour, IOffScreen {
    protected SectionManager.WaveNumber _actualWave;
    protected SectionNode _actualSectionNode;
    //public float whiteTimeOnHit = .2f;
    //public Material mainMat;

    //public Material whiteMat;

    //Material mainMat;
    //WaitForSeconds _waitWhite;
    //SkinnedMeshRenderer[] _skinnedRnds;
    //MeshRenderer[] _meshRnds;

    //bool _hitRoutineRunning = false; 
    //IEnumerator _onHitRoutine;

    OnHitWhiteFeedback _onHitWhiteFeedback;

    protected EnemiesIntegrationBehaviour _eIntegration;

    public SectionNode GetCurrentSectionNode { get { return _actualSectionNode; } }

    public Vector3 GetPosition { get { return transform.position; } }

    public AbstractEnemy SetActualWave(SectionManager.WaveNumber wave) {
        _actualWave = wave;
        return this;
    }

    public AbstractEnemy SetActualNode(SectionNode node) {
        _actualSectionNode = node;
        return this;
    }

    public AbstractEnemy SetIntegration(float timeToReintergrate) {
        if (_eIntegration == null) { 
            _eIntegration = GetComponent<EnemiesIntegrationBehaviour>();
        }

        _eIntegration.SetReintergration(timeToReintergrate); 
        return this; 
    }

    public AbstractEnemy SubscribeToIndicator() {
        OffScreenIndicatorManager.instance.SubscribeIOffScreen(this);
        return this;
    }

    public AbstractEnemy UnSubscribeToIndicator() {
        OffScreenIndicatorManager.instance.UnsubscribeIOffScreen(this);
        return this;
    }

    //settea el ponerse blanco cuando le pegan
    public AbstractEnemy SetTimeAndRenderer() {
        //_waitWhite = new WaitForSeconds(whiteTimeOnHit);
        if(_onHitWhiteFeedback == null) {
            _onHitWhiteFeedback = GetComponent<OnHitWhiteFeedback>();
        }

        _onHitWhiteFeedback.SetFeedback();

        //if(_skinnedRnds == null)
        //    _skinnedRnds = GetComponentsInChildren<SkinnedMeshRenderer>();

        //if (_meshRnds == null)
        //    _meshRnds = GetComponentsInChildren<MeshRenderer>();

        //if (_skinnedRnds.Length > 0) { 
        //    if(mainMat == null) { 
        //        mainMat = _skinnedRnds[0].material;
        //    }
        //    foreach (var mt in _skinnedRnds) { 
        //        mt.material = mainMat;
        //    }
        //}

        //if (_meshRnds.Length > 0) {
        //    if (mainMat == null)
        //        mainMat = _meshRnds[0].material;
        //    foreach (var mt in _meshRnds) 
        //        mt.material = mainMat;
        //}

        return this;
    } 

    protected void AbstractOnHitWhiteAction() {
        //if(_hitRoutineRunning) {
        //    StopCoroutine(_onHitRoutine);
        //    ResetMat();
        //}

        //_onHitRoutine = OnHitRoutine();

        //StartCoroutine(_onHitRoutine); 

        _onHitWhiteFeedback.OnHit();
    }

    //protected void ResetMat() {
    //    if (_skinnedRnds != null) { 
    //        foreach (var mt in _skinnedRnds) {
    //            if (mt == null)
    //                continue;
    //            mt.material = mainMat;
    //        }
    //    }

    //    if (_meshRnds != null) { 
    //        foreach (var mt in _meshRnds) {
    //            if (mt == null)
    //                continue;
    //            mt.material = mainMat;
    //        }
    //    }
    //}

    //void OnDisable() {
    //    ResetMat();
    //}

    //IEnumerator OnHitRoutine() {
    //    _hitRoutineRunning = true;

    //    if (_skinnedRnds != null) { 
    //        foreach (var mt in _skinnedRnds) {
    //            if (mt == null)
    //                continue;
    //            mt.material = whiteMat;
    //        }
    //    }

    //    if (_meshRnds != null) { 
    //        foreach (var mt in _meshRnds) {
    //            if (mt == null)
    //                continue;
    //            mt.material = whiteMat;
    //        }
    //    }

    //    yield return _waitWhite;

    //    if (_skinnedRnds != null) { 
    //        foreach (var mt in _skinnedRnds) {
    //            if (mt == null)
    //                continue;
    //            mt.material = mainMat;
    //        }
    //    }

    //    if (_meshRnds != null) { 
    //        foreach (var mt in _meshRnds) {
    //            if (mt == null)
    //                continue;
    //            mt.material = mainMat;
    //        }
    //    }
    //    _hitRoutineRunning = false;
    //}
}
