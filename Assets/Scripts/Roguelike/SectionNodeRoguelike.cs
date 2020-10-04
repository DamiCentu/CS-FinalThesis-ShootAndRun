using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionNodeRoguelike : SectionNode {

	// Use this for initialization
	void Start () {
        print("section node start");
        _dicSpawn.Add(SectionManager.WaveNumber.First, new List<EnemySpawner>());
        _dicSpawn.Add(SectionManager.WaveNumber.Second, new List<EnemySpawner>());
        _dicSpawn.Add(SectionManager.WaveNumber.Third, new List<EnemySpawner>());

        _dicQuantityInWave.Add(SectionManager.WaveNumber.First, 0);
        _dicQuantityInWave.Add(SectionManager.WaveNumber.Second, 0);
        _dicQuantityInWave.Add(SectionManager.WaveNumber.Third, 0);

        _allSpawns = transform.GetComponentsInChildren<EnemySpawner>();

        _allSpawnsThatNotAffectEndOfNode = transform.GetComponentsInChildren<EnemySpawnerAtBeginningOfNode>();

        _allMultiSpawns = transform.GetComponentsInChildren<MultiEnemySpawner>();

        _allProps = transform.GetComponentsInChildren<PropsBehaviour>();

        _allMapNodes = transform.GetComponentsInChildren<MapNode>();


        foreach (var m in _allMultiSpawns)
        {
            _allQuantityInMultiEnemySpawners += m.quantityOfEnemies;
        }

        Utility.ConnectMapNodes(_allMapNodes, objectToDetectConnectingNodes);

        foreach (var spawn in _allSpawns)
        {
            if (_dicSpawn.ContainsKey(spawn.waveOfSpawn))
            {
                if (spawn.triggerSpawn)
                    continue;
                else
                    _dicSpawn[spawn.waveOfSpawn].Add(spawn);
            }
        }

        _waitBetweenWaves = new WaitForSeconds(timeBetweenWaves);
        _waitBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);

        EventManager.instance.SubscribeEvent(Constants.ENEMY_DEAD, OnEnemyDead);
        EventManager.instance.SubscribeEvent(Constants.POWER_UP_DROPED, OnPowerUpDropped);
        EventManager.instance.SubscribeEvent(Constants.BOSS_DESTROYED, OnBossDestroyed);

        _allWallsThatHaveToUnlockDestroying = transform.GetComponentsInChildren<WallThatDestroysWhenPlayerKillsEnemiesBehaviour>();
        SetQuantityToDestroyToUnlockSomething();
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
