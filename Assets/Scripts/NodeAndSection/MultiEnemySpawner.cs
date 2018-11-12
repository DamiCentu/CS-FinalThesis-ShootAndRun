using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiEnemySpawner : MonoBehaviour {

    public int quantityOfEnemies = 10;
    public int quantityOfTriggersThatAffectThisSpawner;
    [Header("----------------------------")]
    public int idOfWall = 0;
    [Header("----------------------------")]
    public float offsetY = 1f;
    public bool hasToDestroyToUnlockSomething = false;

    public Vector3 GetPositionWithOffset { get { return Utility.SetYInVector3(transform.position, offsetY); } }
    public int GetRespectiveQuantityOfEnemyPerTrigger { get { return quantityOfEnemies / quantityOfTriggersThatAffectThisSpawner; } }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 1));
    }
}
