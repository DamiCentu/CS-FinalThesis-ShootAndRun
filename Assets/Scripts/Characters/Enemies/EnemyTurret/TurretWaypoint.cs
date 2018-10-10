using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretWaypoint : MonoBehaviour {
    public TurretWaypoint next;
    public float radius = 1f;
    public bool drawGizmos = true;

    private void OnDrawGizmos() {
        if (!drawGizmos) {
            return;
        }
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
