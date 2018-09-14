using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerAtBeginningOfNode : MonoBehaviour {
    public Transform parent;
    public EnemiesManager.TypeOfEnemy typeOfEnemy;

    public float radiusOfDebugSphere = 1f;
    public bool showGizmos = true;

    void OnDrawGizmos() {
        if (!showGizmos)
            return;

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, radiusOfDebugSphere);
    }


}
