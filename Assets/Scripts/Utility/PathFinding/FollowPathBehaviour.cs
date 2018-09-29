using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathBehaviour {
     
    MonoBehaviour _parentMono;
    LayerMask _blockEnemyViewToTarget;
    Flocking _flocking;
    MapNode _closestMNToTarget;
    bool _coroutineRunning;
    bool _hasToFollowPlayer = true;

    SectionNode _actualSectionNode;

    Transform _powerUpToChase;

    public bool HasToFollowPlayer { get { return _hasToFollowPlayer; } set { _hasToFollowPlayer = value; } }

    public FollowPathBehaviour(MonoBehaviour parent, LayerMask blockEnemyViewToTarget, Flocking flocking/*, bool hasToFollowPlayer*/) {
        _parentMono = parent;
        _blockEnemyViewToTarget = blockEnemyViewToTarget;
        _flocking = flocking;
        //HasToFollowPlayer = hasToFollowPlayer;
    }

    public FollowPathBehaviour SetActualSectionNode(SectionNode actual) {
        _actualSectionNode = actual;
        return this;
    }

    public FollowPathBehaviour SetPowerUpToChase(Transform powerUp) { 
        _powerUpToChase = powerUp;
        return this;
    }

    public void OnUpdate() {
        if (_actualSectionNode.sectionHasMapNodes) {
            if (HasToFollowPlayer) { 
                Decision(EnemiesManager.instance.player.transform);
            }
            else { 
                Decision(_powerUpToChase);
            }
        }
    }

    void Decision(Transform target) {
        if (target == null)
            return;
        
        var dir = target.position - _parentMono.transform.position; 

        if (Physics.Raycast(_parentMono.transform.position, dir, dir.magnitude,_blockEnemyViewToTarget)) {

            if (!EnemiesManager.instance.showFollowPathGizmos) { 
                Debug.DrawLine(_parentMono.transform.position, _parentMono.transform.position + dir.normalized * dir.magnitude , Color.red, Time.deltaTime);
            }
            var c = _actualSectionNode.GetClosestMapNode(target.position);

            if (_closestMNToTarget != c) {
                _parentMono.StopAllCoroutines();
                _coroutineRunning = false;
                _closestMNToTarget = c;
            } 

            if (!_coroutineRunning)
                _parentMono.StartCoroutine(FollowPathRoutine(ThetaStar.Run(_actualSectionNode.GetClosestMapNode(_parentMono.transform.position), _closestMNToTarget, _blockEnemyViewToTarget)));
        }
        else {
            if (!EnemiesManager.instance.showFollowPathGizmos) {
                Debug.DrawLine(_parentMono.transform.position, _parentMono.transform.position + dir.normalized * dir.magnitude , Color.green, Time.deltaTime);
            }
            _parentMono.StopAllCoroutines();
            _coroutineRunning = false;
            _flocking.target = target;
        }
    }

    public void OnDisable() {
        _parentMono.StopAllCoroutines();
        _coroutineRunning = false;
    }

    IEnumerator FollowPathRoutine(Stack<MapNode> path) {
        _coroutineRunning = true;
        while (path.Count > 0) {
            var temp = path.Pop(); 
            _flocking.target = temp.transform;
            while(!Utility.InRangeSquared(temp.transform.position, _parentMono.transform.position, temp.radius)) { 
                yield return null;
            }
        }
        _coroutineRunning = false;
    }
}
