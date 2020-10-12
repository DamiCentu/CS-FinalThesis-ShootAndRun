using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionNodeRoguelike : SectionNode {

    public List<GameObject> phases;
    public int currentPhases=0;
    public override bool SectionCleared
    {
        get
        {

            return false;
        }
    }
    void Start () {
        print("section node start");
        AddSpawns();

        Utility.ConnectMapNodes(_allMapNodes, objectToDetectConnectingNodes);
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
            //ResetBetweenPhases();
            yield return _waitBetweenWaves;
            NextStageSection();


        }
      

    }

    private void ResetBetweenPhases() {

        OffScreenIndicatorManager.instance.CleanIndicators();
        StopAllCoroutines();

        EventManager.instance.ExecuteEvent(Constants.STOP_BERSERK);

        EnemyBulletManager.instance.ReturnAllBullets();

        _dicQuantityInWave[SectionManager.WaveNumber.First] = 0;
        _dicQuantityInWave[SectionManager.WaveNumber.Second] = 0;
        _dicQuantityInWave[SectionManager.WaveNumber.Third] = 0;

        foreach (var e in _allFireEnemiesActive)
        {
            e.Stop();
        }

        Utility.DeactivateList(_allNormalActives);
        Utility.DeactivateList(_allChargerActives);
        Utility.DeactivateList(_allTurretsActives);
        Utility.DeactivateList(_allChasersActives);
        Utility.DeactivateList(_allCubeActives);
        Utility.DeactivateList(_allMisilEnemiesActive);
        Utility.DeactivateList(_allFireEnemiesActive);

        _allNormalActives.Clear();
        _allChargerActives.Clear();
        _allTurretsActives.Clear();
        _allChasersActives.Clear();
        _allCubeActives.Clear();
        _allMisilEnemiesActive.Clear();
        _allFireEnemiesActive.Clear();

        Utility.DestroyAllInAndClearList(_allMiniBoss);
    }



  

   

    //Spawnea en el en todos los spawns que no afecten en el final del nodo y setea la cantidad de enemigos para desbloquear algo

    
    
    public void NextStageSection()
    {
        Utility.DeactivateList(_allNormalActives);
        Utility.DeactivateList(_allChargerActives);
        Utility.DeactivateList(_allTurretsActives);
        Utility.DeactivateList(_allChasersActives);
        Utility.DeactivateList(_allCubeActives);
        Utility.DeactivateList(_allMisilEnemiesActive);
        Utility.DeactivateList(_allFireEnemiesActive);

        _allNormalActives.Clear();
        _allChargerActives.Clear();
        _allTurretsActives.Clear();
        _allChasersActives.Clear();
        _allCubeActives.Clear();
        _allMisilEnemiesActive.Clear();
        _allFireEnemiesActive.Clear();
        StopAllCoroutines();


        currentPhases++;
        AddSpawns();
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


    void AddSpawns() {

        _dicSpawn = new Dictionary<SectionManager.WaveNumber, List<EnemySpawner>>();
        _dicQuantityInWave = new Dictionary<SectionManager.WaveNumber, int>();
        _dicSpawn.Add(SectionManager.WaveNumber.First, new List<EnemySpawner>());
        _dicSpawn.Add(SectionManager.WaveNumber.Second, new List<EnemySpawner>());
        _dicSpawn.Add(SectionManager.WaveNumber.Third, new List<EnemySpawner>());

        _dicQuantityInWave.Add(SectionManager.WaveNumber.First, 0);
        _dicQuantityInWave.Add(SectionManager.WaveNumber.Second, 0);
        _dicQuantityInWave.Add(SectionManager.WaveNumber.Third, 0);


        GameObject curPhaseGO = phases[currentPhases];
        _allSpawns = curPhaseGO.transform.GetComponentsInChildren<EnemySpawner>();
        _allSpawnsThatNotAffectEndOfNode = curPhaseGO.transform.GetComponentsInChildren<EnemySpawnerAtBeginningOfNode>();
        _allMultiSpawns = curPhaseGO.transform.GetComponentsInChildren<MultiEnemySpawner>();
        _allProps = curPhaseGO.transform.GetComponentsInChildren<PropsBehaviour>();
        _allMapNodes = curPhaseGO.transform.GetComponentsInChildren<MapNode>();


        foreach (var m in _allMultiSpawns)
        {
            _allQuantityInMultiEnemySpawners += m.quantityOfEnemies;
        }



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
    }





}
