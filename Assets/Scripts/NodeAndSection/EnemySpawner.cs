using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public SectionManager.WaveNumber waveOfSpawn;
    public EnemiesManager.TypeOfEnemy typeOfEnemy;
    public bool triggerSpawn = false;
    public bool hasToDestroyToUnlockSomething = false;

    public float radiusOfDebugSphere = 1f;
    public float sizeDebugCube = 1.5f; 
    public bool showGizmos = true;

    public float extraWaitToSpawn = 0f;

    void OnDrawGizmos() {
        if (!showGizmos)
            return;

        if(triggerSpawn) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(sizeDebugCube, sizeDebugCube, sizeDebugCube));
            return;
        }

        if (waveOfSpawn == SectionManager.WaveNumber.First) { 
            Gizmos.color = Color.green; 
            Gizmos.DrawWireSphere(transform.position, radiusOfDebugSphere - .3f);
        }
        else if(waveOfSpawn == SectionManager.WaveNumber.Second) { 
            Gizmos.color = Color.yellow; 
            Gizmos.DrawWireSphere(transform.position, radiusOfDebugSphere);
        } 
        else if(waveOfSpawn == SectionManager.WaveNumber.Third) { 
            Gizmos.color = Color.red; 
            Gizmos.DrawWireSphere(transform.position, radiusOfDebugSphere + .3f);
        }
    }
}
