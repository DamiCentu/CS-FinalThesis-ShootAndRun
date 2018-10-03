using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryPosStrategy : ICameraStrategy {

	CameraBehaviour _parent;
    Vector3 _pos;

    public StationaryPosStrategy(CameraBehaviour parent) {
        _parent = parent;
    }

    public void OnFixedUpdate() { 
        Vector3 desiredPosition = _pos + _parent.offsetStationary;
        Vector3 smoothedPosition = Vector3.Lerp(_parent.transform.position, desiredPosition, _parent.followSmoothSpeedStationary);
        _parent.transform.position = smoothedPosition;
    }

    public void SetStationaryPos(Vector3 pos) {
        _pos = pos;
    }
}
