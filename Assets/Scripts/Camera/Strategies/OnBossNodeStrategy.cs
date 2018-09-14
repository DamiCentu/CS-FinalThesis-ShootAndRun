using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBossNodeStrategy : ICameraStrategy  {

    CameraBehaviour _parent;

    public OnBossNodeStrategy(CameraBehaviour parent) {
        _parent = parent;
    }

    public void OnFixedUpdate() {
        Vector3 desiredPosition = _parent.target.position + _parent.cameraOffsetOnBoss;
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, _parent.cameraPivotOnBoss.x - _parent.clampMovementX, _parent.cameraPivotOnBoss.x + _parent.clampMovementX);
        desiredPosition.z = Mathf.Clamp(desiredPosition.z, _parent.cameraPivotOnBoss.z - _parent.clampMovementZ, _parent.cameraPivotOnBoss.z + _parent.clampMovementZ);
        Vector3 smoothedPosition = Vector3.Lerp(_parent.transform.position, desiredPosition, _parent.smoothSpeedOnBoss);
        _parent.transform.position = smoothedPosition;
    }
}
