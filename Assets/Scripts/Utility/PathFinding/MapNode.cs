using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapNode : MonoBehaviour {
	public List<MapNode> adjacent = new List<MapNode>();
    public float radius = 3f;

    public bool gizmosOn = true;

    void OnDrawGizmos() {
        if (!gizmosOn) {
            return;
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);

        if (adjacent.Count == 0) return;

        foreach (var adj in adjacent) {  
            Gizmos.color = Color.grey;
            Gizmos.DrawLine(transform.position, adj.transform.position);
        } 
    }
}
