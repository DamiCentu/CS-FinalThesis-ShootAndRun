using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerTrigger : MonoBehaviour {
    public SectionNode nodeOfTriggering;
    public bool spawnWaves = false;
    public GameObject allTriggerSpawnersContainer;

    EnemySpawner[] _allTriggerSpawners;

    void Start() {
        if (!spawnWaves)
            _allTriggerSpawners = allTriggerSpawnersContainer.GetComponentsInChildren<EnemySpawner>();
    }

    void OnTriggerEnter(Collider c) {
        if(c.gameObject.layer == 8) {
            if (nodeOfTriggering != null) {
                if(spawnWaves)
                    nodeOfTriggering.StartNodeRoutine();
                else
                    nodeOfTriggering.TriggerSpawn(_allTriggerSpawners);
                gameObject.SetActive(false);
            }
        }
    }
}
