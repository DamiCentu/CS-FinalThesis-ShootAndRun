using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeUILine : MonoBehaviour , IPauseable { 
    public GameObject arrowPrefab; 
    public Transform parentOfArrows;
    public float distBetweenArrows = 0.5f;
    public float arrowsScale = 0.05f;
    public float tiltingTime = 0.05f;

    Pool<GameObject> _poolArrows;

    List<GameObject> _allArrows = new List<GameObject>();
    bool _lineActive;
    Transform _target;

    Quaternion _startQuat;
    Vector3 _startPos;

    float _tiltingTimer = 0;
    float yPosOfArrows = 0f;

    bool _paused;

    public void OnPauseChange(bool v) {
        _paused = v; 
    }

    void Awake() {
        _poolArrows = new Pool<GameObject>(15, ArrowFactoryMethod, null, null, true);
    }

    void OnDisable() {
        DeactivateLine();
    }

    void Update() {
        if (_paused)
            return;

        if (!_lineActive || _target == null)
            return;

        parentOfArrows.position = _startPos;
        parentOfArrows.rotation = _startQuat;

        _tiltingTimer += Time.deltaTime;

        UpdateLine();

        if (_tiltingTimer > tiltingTime) {
            TiltingUpdate();
        }
    }

    void TiltingUpdate() {
        foreach (var a in _allArrows) {
            if (a.gameObject.activeSelf) {
                a.gameObject.SetActive(false);
            }
            else {
                a.gameObject.SetActive(true);
            }
        }
        _tiltingTimer = 0f; 
    }

    void UpdateLine() {
        var dir = Utility.SetYInVector3(_target.position, 1f) - Utility.SetYInVector3(transform.position, 1f);
        var dist = dir.magnitude;
        dir.Normalize();

        for (int i = _allArrows.Count - 1; i >= 0  ; i--) {
            var a = _allArrows[i];
            var dirMuliplied = dir * i;

            if (dirMuliplied.magnitude > dist) {
                _allArrows.Remove(a);
                ReturnArrowToPool(a);
                continue;
            }

            a.transform.forward = new Vector3(dir.x, 90, dir.z);
            a.transform.position = Utility.SetYInVector3(transform.position + dir * i, yPosOfArrows);
        } 
    }

    public void ActivateLine(Transform target, Color color) {
        _lineActive = true;
        _target = target;

        var dir = Utility.SetYInVector3(target.position, 1f) - Utility.SetYInVector3(transform.position, 1f);
        var dist = dir.magnitude;
        dir.Normalize();

        RaycastHit rh;

        Physics.Raycast(transform.position, Vector3.down, out rh, 20, Utility.LayerNumberToMask(15)); //floor

        yPosOfArrows = rh.point.y + EnemiesManager.instance.UILineYOffset;

        _startQuat = parentOfArrows.rotation;
        _startPos = parentOfArrows.position; 

        for (int i = 0; i < dist / distBetweenArrows; i++) {
            var dirMuliplied = dir * i;
            if (dirMuliplied.magnitude > dist)
                break;
            var a = GiveMeArrow();
            a.GetComponent<Image>().color = color;
            a.SetActive(true);
            a.transform.localScale = new Vector3(arrowsScale, arrowsScale, 1f);
            a.transform.forward = new Vector3(dir.x,90, dir.z);
            a.transform.position = Utility.SetYInVector3( transform.position + dir * i , yPosOfArrows);
            _allArrows.Add(a);
        }
    } 

    public void DeactivateLine() {
        _lineActive = false;
        foreach (var a in _allArrows) {
            ReturnArrowToPool(a);
        } 
        _allArrows.Clear();
    }

    #region POOL METHODS
    GameObject ArrowFactoryMethod() {
        var a = Instantiate(arrowPrefab, parentOfArrows);
        a.gameObject.SetActive(false);
        return a;
    }

    GameObject GiveMeArrow() { 
        return _poolArrows.GetObjectFromPool();
    }

    void ReturnArrowToPool(GameObject arrow) {
        _poolArrows.DisablePoolObject(arrow);
        arrow.SetActive(false);
    }
    #endregion
}
