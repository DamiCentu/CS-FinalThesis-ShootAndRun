using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionNodeRoguelike : SectionNode {

    public override bool SectionCleared
    {
        get
        {

            return false;
        }
    }
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


    protected override IEnumerator WavesNodeRoutine()
    {
        while (true) {
            ResetTimeBetweenWaves(SectionManager.WaveNumber.First);
            ResetTimeBetweenWaves(SectionManager.WaveNumber.Second);
            ResetTimeBetweenWaves(SectionManager.WaveNumber.Third);
            _currentTimeBetweenDelayedSpawn = 0;
            SetWaves(SectionManager.WaveNumber.First);
            yield return _waitBetweenWaves;

            while (_paused)
                yield return null;

            while (_dicQuantityInWave[SectionManager.WaveNumber.First] > 0)
                yield return null;
            _currentTimeBetweenDelayedSpawn = 0;
            SetWaves(SectionManager.WaveNumber.Second);
            yield return _waitBetweenWaves;

            while (_paused)
                yield return null;

            while (_dicQuantityInWave[SectionManager.WaveNumber.Second] > 0)
                yield return null;

            if (TutorialBehaviour.instance != null && TutorialBehaviour.instance.IsTutorialNode)
            {
                EventManager.instance.ExecuteEvent(Constants.UI_TUTORIAL_CHANGE, UIManager.TUTORIAL_ULTIMATE);
            }
            _currentTimeBetweenDelayedSpawn = 0;
            SetWaves(SectionManager.WaveNumber.Third);
            yield return _waitBetweenWaves;

            while (_paused)
                yield return null;

            while (_dicQuantityInWave[SectionManager.WaveNumber.Third] > 0)
                yield return null;
            NextStageSection();

        }
      

    }

    private void ResetTimeBetweenWaves(SectionManager.WaveNumber wave )
    {
        foreach (var spawnPoint in _dicSpawn[wave])
        {
            if (spawnPoint.waveOfSpawn != wave || spawnPoint.triggerSpawn)
                return;
            _dicQuantityInWave[wave]=0;

            //StartCoroutine(SpawnEnemy(spawnPoint, wave));
        }
    }

  

   

    //Spawnea en el en todos los spawns que no afecten en el final del nodo y setea la cantidad de enemigos para desbloquear algo

    
    
    public void NextStageSection()
    {

        object[] conteiner = new object[2]; // container
        conteiner[0] = "in"; 
        conteiner[1] = this;
        EventManager.instance.ExecuteEvent(Constants.START_SECTION, conteiner);

        SpawnEnemyAtStart();

        if (!wavesStartAtTrigger && !isBossNode)
        {
            StartCoroutine(WavesNodeRoutine());
        }


        _currentTimeBetweenDelayedSpawn = 0;
    }






    protected void SetWaves(SectionManager.WaveNumber wave)
    {
        SoundManager.instance.PlaySpawnEnemy();
        if (!_dicSpawn.ContainsKey(wave))
            return;
     //   if (_dicQuantityInWave[wave] == 3) _dicQuantityInWave[wave] = 0;

        foreach (var spawnPoint in _dicSpawn[wave])
        {
            if (spawnPoint.waveOfSpawn != wave || spawnPoint.triggerSpawn)
                return;
            _dicQuantityInWave[wave]++;

            StartCoroutine(SpawnEnemy(spawnPoint, wave));
        }
    }



}
