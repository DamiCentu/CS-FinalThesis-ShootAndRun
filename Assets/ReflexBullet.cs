using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflexBullet : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        other.transform.forward = new Vector3(-other.transform.forward.x, other.transform.forward.z, -other.transform.forward.z);
    }
    private void OnCollisionEnter(Collision other)
    {
        Quaternion rot = other.transform.rotation;
        rot.eulerAngles = new Vector3(rot.eulerAngles.x, -rot.eulerAngles.y, rot.eulerAngles.z);
        other.transform.rotation = rot;
    }
}
