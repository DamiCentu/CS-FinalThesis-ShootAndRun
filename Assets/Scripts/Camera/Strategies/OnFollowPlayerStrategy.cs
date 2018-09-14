using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFollowPlayerStrategy : ICameraStrategy  {
    CameraBehaviour _parent;

    public OnFollowPlayerStrategy(CameraBehaviour parent) {
        _parent = parent;
    } 
    
    public void OnFixedUpdate() {
        Vector3 desiredPosition = _parent.target.position + _parent.offset;
        Vector3 smoothedPosition = Vector3.Lerp(_parent.transform.position, desiredPosition, _parent.followSmoothSpeed);
        _parent.transform.position = smoothedPosition;
    }
}
