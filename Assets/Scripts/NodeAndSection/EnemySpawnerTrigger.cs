using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerTrigger : MonoBehaviour {
    public SectionNode nodeOfTriggering;
    public bool spawnWaves = false;
    public GameObject allTriggerSpawnersContainer;
    public TypeOfTrigger typeOfTrigger;

    EnemySpawner[] _allTriggerSpawners;

    public enum TypeOfTrigger {
        SpawnWaves,
        SpawnEnemiesPreSetted,
        SpawnEnemiesInPortal
    }

    void Start() {
        if (typeOfTrigger == TypeOfTrigger.SpawnEnemiesPreSetted) {
         //   if (!spawnWaves) { 
            _allTriggerSpawners = allTriggerSpawnersContainer.GetComponentsInChildren<EnemySpawner>();
        }
    }

    void OnTriggerEnter(Collider c) {
        if(c.gameObject.layer == 8) {

            switch (typeOfTrigger) {

                case TypeOfTrigger.SpawnWaves:
                    nodeOfTriggering.StartNodeRoutine();
                    break;
                case TypeOfTrigger.SpawnEnemiesPreSetted:
                    nodeOfTriggering.TriggerSpawn(_allTriggerSpawners);
                    break;
                case TypeOfTrigger.SpawnEnemiesInPortal:
                    break;

            }

            gameObject.SetActive(false);

            //if (spawnWaves) { 
            //    nodeOfTriggering.StartNodeRoutine();
            //}
            //else { 
            //    nodeOfTriggering.TriggerSpawn(_allTriggerSpawners);
            //}
            //gameObject.SetActive(false);
            
        }
    }
}
