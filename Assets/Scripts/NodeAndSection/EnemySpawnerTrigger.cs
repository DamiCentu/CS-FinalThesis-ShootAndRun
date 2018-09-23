using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerTrigger : MonoBehaviour {
    [Header("TheNodeContainerOfTHIS")]
    public SectionNode nodeOfTriggering;

    [Header("ThisOnlyWorkWithSpawnEnemiesPreSetted")]
    public GameObject allTriggerSpawnersContainer;

    [Header("ThisOnlyWorkWithSpawnEnemiesInPortal")]
    public MultiEnemySpawner multiSpawner;

    [Header("TypeOfTrigger")]
    public TypeOfTrigger typeOfTrigger;

    EnemySpawner[] _allTriggerSpawners;

    public enum TypeOfTrigger {
        SpawnWaves,
        SpawnEnemiesPreSetted,
        SpawnEnemiesInPortal
    }

    void Start() {
        if (typeOfTrigger == TypeOfTrigger.SpawnEnemiesPreSetted) {
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
                    nodeOfTriggering.StartTriggerMultipleSpawnRoutine(multiSpawner.GetRespectiveQuantityOfEnemyPerTrigger, multiSpawner.GetPositionWithOffset);
                    break;

            } 
            gameObject.SetActive(false);
        }
    }
}
