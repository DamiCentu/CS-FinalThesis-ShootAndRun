using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiEnemySpawner : MonoBehaviour {

    public int quantityOfEnemies = 10; 
    public GameObject[] triggers;
    public float offsetY = 1f;

    public Vector3 GetPositionWithOffset { get { return Utility.SetYInVector3(transform.position, offsetY); } }
    public int GetRespectiveQuantityOfEnemyPerTrigger { get { return quantityOfEnemies / triggers.Length; } }
}
