﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerTrigger : MonoBehaviour {
    [Header("TheNodeContainerOfTHIS")]
    public SectionNode nodeOfTriggering;

    [Header("ThisOnlyWorkWithSpawnEnemiesPreSetted")]
    public GameObject allTriggerSpawnersContainer;

    [Header("ThisOnlyWorkWithSpawnEnemiesInPortal")]
    public MultiEnemySpawner[] multiSpawners;

    [Header("TypeOfTrigger")]
    public TypeOfTrigger typeOfTrigger;

    public bool hasToTriggerOnColliderExit = false;

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
        if (hasToTriggerOnColliderExit)
            return;

        if (c.gameObject.layer == 8) { 

            switch (typeOfTrigger) {

                case TypeOfTrigger.SpawnWaves:
                    nodeOfTriggering.StartNodeRoutine();
                    break;
                case TypeOfTrigger.SpawnEnemiesPreSetted:
                    nodeOfTriggering.TriggerSpawn(_allTriggerSpawners);
                    break;
                case TypeOfTrigger.SpawnEnemiesInPortal:
                    foreach (var multiSpawner in multiSpawners) {
                        nodeOfTriggering.StartTriggerMultipleSpawnRoutine(multiSpawner.GetRespectiveQuantityOfEnemyPerTrigger, multiSpawner);
                    }
                    break;

            } 
            gameObject.SetActive(false);
        }
    }

     void OnTriggerExit(Collider c) {
        if (!hasToTriggerOnColliderExit)
            return;

        if (c.gameObject.layer == 8) { 

            switch (typeOfTrigger) {

                case TypeOfTrigger.SpawnWaves:
                    nodeOfTriggering.StartNodeRoutine();
                    break;
                case TypeOfTrigger.SpawnEnemiesPreSetted:
                    nodeOfTriggering.TriggerSpawn(_allTriggerSpawners);
                    break;
                case TypeOfTrigger.SpawnEnemiesInPortal:
                    foreach (var multiSpawner in multiSpawners) {
                        nodeOfTriggering.StartTriggerMultipleSpawnRoutine(multiSpawner.GetRespectiveQuantityOfEnemyPerTrigger, multiSpawner);
                    }
                    break;

            } 
            gameObject.SetActive(false);
        }
    }
}
