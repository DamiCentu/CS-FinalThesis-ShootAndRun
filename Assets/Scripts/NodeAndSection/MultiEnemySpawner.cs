using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiEnemySpawner : MonoBehaviour {

    public int quantityOfEnemies = 10; 
    [Header("Deprecated")]
    public GameObject[] triggers;
    [Header("---------")]
    public int quantityOfTriggersThatAffectThisSpawner;
    public float offsetY = 1f;

    public Vector3 GetPositionWithOffset { get { return Utility.SetYInVector3(transform.position, offsetY); } }
    public int GetRespectiveQuantityOfEnemyPerTrigger { get { return quantityOfEnemies / quantityOfTriggersThatAffectThisSpawner; } }
}
