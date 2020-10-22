using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionNode : MonoBehaviour, IPauseable
{
    public int id;
    public SectionNode next;

    public float timeBetweenWaves = 2f;
    public float timeBetweenSpawns = 1f;

    public Transform playerSpawnPoint;

    public GameObject[] triggerSpawners;

    public bool wavesStartAtTrigger = false;

    public LayerMask objectToDetectConnectingNodes;

    public bool isBossNode;

    public GameObject bossNodeWarningParticle;
    public Transform constantPointToLookAtOnBoss;
    public Transform cameraPosInLookAtOnBoos;

    public float radiusToSetPowerUpChaser = 7f;

    public bool waveDelayedSpawn = false;
    public SectionManager.WaveNumber waveNumber;
    public float timeBetweenDelayedSpawn = 0.4f;
    protected float _currentTimeBetweenDelayedSpawn = 0f;

    protected Vector3 onGizmosSafeZoneChaser = new Vector3(1000f, 1000f, 1000f);

    protected EnemySpawner[] _allSpawns;
    protected EnemySpawnerAtBeginningOfNode[] _allSpawnsThatNotAffectEndOfNode;
    protected MultiEnemySpawner[] _allMultiSpawns;
    protected PropsBehaviour[] _allProps;

    protected MapNode[] _allMapNodes;

    protected Dictionary<SectionManager.WaveNumber, List<EnemySpawner>> _dicSpawn = new Dictionary<SectionManager.WaveNumber, List<EnemySpawner>>();
    protected Dictionary<SectionManager.WaveNumber, int> _dicQuantityInWave = new Dictionary<SectionManager.WaveNumber, int>();

    protected List<NormalEnemyBehaviour> _allNormalActives = new List<NormalEnemyBehaviour>();
    protected List<ChargerEnemyBehaviour> _allChargerActives = new List<ChargerEnemyBehaviour>();
    protected List<EnemyTurretBehaviour> _allTurretsActives = new List<EnemyTurretBehaviour>();
    protected List<PowerUpChaserEnemy> _allChasersActives = new List<PowerUpChaserEnemy>();
    protected List<MiniBossBehaviour> _allMiniBoss = new List<MiniBossBehaviour>();
    protected List<CubeEnemyBehaviour> _allCubeActives = new List<CubeEnemyBehaviour>();
    protected List<MisilEnemy> _allMisilEnemiesActive = new List<MisilEnemy>();
    protected List<FireEnemy> _allFireEnemiesActive = new List<FireEnemy>();

    protected WaitForSeconds _waitBetweenWaves;
    protected WaitForSeconds _waitBetweenSpawns;

    protected Dictionary<int, int> _quantityPerIDDictionary = new Dictionary<int, int>();
    protected WallThatDestroysWhenPlayerKillsEnemiesBehaviour[] _allWallsThatHaveToUnlockDestroying;

    protected int _enemiesRemaining = 0;
    protected int _allQuantityInMultiEnemySpawners = 0;
    protected bool _bossAlive = true;

    protected int timesDead = 0;
    public bool spawnGreenEnemiesInBossNode = true;


    protected Boss bossOnScreen;
    protected BossSerpent bossSerpentLeftOnScreen;
    BossSerpent bossSerpentRightOnScreen;
    public int timesDeadTillPowerHelp = 3;

    public bool BossAlive { get { return _bossAlive; } }

    protected bool _paused;
    public void OnPauseChange(bool v)
    {
        _paused = v;
    }

    public void SetBoss()
    {
        if (Configuration.instance.lvl == 1)
        {
            StartCoroutine(BossRoutine());
        }
        else if (Configuration.instance.lvl == 2)
        {
            StartCoroutine(BossSerpentsRoutine());
        }
    }

    protected IEnumerator BossRoutine()
    {

        yield return _waitBetweenWaves;
        while (_paused)
            yield return null;

        bossOnScreen = Instantiate(EnemiesManager.instance.bossPrefab, _allSpawns[0].transform.position, _allSpawns[0].transform.rotation).GetComponent<Boss>();
        bossOnScreen.GetComponent<AbstractEnemy>().SetTimeAndRenderer().SetActualNode(this);
        bossOnScreen.gameObject.SetActive(true);
    }


    protected IEnumerator BossSerpentsRoutine()
    {

        yield return _waitBetweenWaves;
        while (_paused)
            yield return null;

        bossSerpentLeftOnScreen = Instantiate(EnemiesManager.instance.bossSerpentLeftPrefab, _allSpawns[0].transform.position, _allSpawns[0].transform.rotation).GetComponent<BossSerpent>();
        bossSerpentLeftOnScreen.GetComponent<AbstractEnemy>().SetTimeAndRenderer().SetActualNode(this);
        bossSerpentLeftOnScreen.gameObject.SetActive(true);

        bossSerpentRightOnScreen = Instantiate(EnemiesManager.instance.bossSerpentRightPrefab, _allSpawns[1].transform.position, _allSpawns[1].transform.rotation).GetComponent<BossSerpent>();
        bossSerpentRightOnScreen.GetComponent<AbstractEnemy>().SetTimeAndRenderer().SetActualNode(this);
        bossSerpentRightOnScreen.gameObject.SetActive(true);
    }

    public virtual bool SectionCleared
    {
        get
        {
            if (_enemiesRemaining <= 0)
            {
                StopAllEnemies();
                return true;
            }
            return false;
        }
    }
    void Start()
    {
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

    protected void OnBossDestroyed(params object[] param)
    {
        _bossAlive = false;
        StopAllEnemies();
    }

    //Spawnea en el en todos los spawns que no afecten en el final del nodo y setea la cantidad de enemigos para desbloquear algo
    public void SpawnEnemyAtStart()
    {
        if (Configuration.instance.mode == Configuration.Mode.RogueLike) return;
        foreach (var sP in _allSpawnsThatNotAffectEndOfNode)
        {
            SpawnEnemyAtPointAtStartNoCuentaParaTerminarNodo(sP);
        }
    }

    protected void SetQuantityToDestroyToUnlockSomething()
    {
        if (_allWallsThatHaveToUnlockDestroying.Length == 0)
            return;

        List<int> keys = new List<int>(_quantityPerIDDictionary.Keys);

        foreach (var i in keys)
        {
            _quantityPerIDDictionary[i] = 0;
        }

        foreach (var sP in _allSpawnsThatNotAffectEndOfNode)
        {
            if (sP.hasToDestroyToUnlockSomething)
            {
                if (!_quantityPerIDDictionary.ContainsKey(sP.idOfWall))
                {
                    _quantityPerIDDictionary.Add(sP.idOfWall, 0);
                }
                _quantityPerIDDictionary[sP.idOfWall]++;
            }
        }

        foreach (var sP2 in _allSpawns)
        {
            if (sP2.hasToDestroyToUnlockSomething)
            {
                if (!_quantityPerIDDictionary.ContainsKey(sP2.idOfWall))
                {
                    _quantityPerIDDictionary.Add(sP2.idOfWall, 0);
                }
                _quantityPerIDDictionary[sP2.idOfWall]++;
            }
        }

        foreach (var sP3 in _allMultiSpawns)
        {
            if (sP3.hasToDestroyToUnlockSomething)
            {
                if (!_quantityPerIDDictionary.ContainsKey(sP3.idOfWall))
                {
                    _quantityPerIDDictionary.Add(sP3.idOfWall, 0);
                }
                _quantityPerIDDictionary[sP3.idOfWall] += sP3.quantityOfEnemies;
            }
        }

        foreach (var i in _quantityPerIDDictionary)
        {
            if (_quantityPerIDDictionary[i.Key] > 0)
            {
                foreach (var w in _allWallsThatHaveToUnlockDestroying)
                {
                    if (w.id == i.Key)
                    {
                        w.Show(true);
                        //  w.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    protected void OnEnemyDead(params object[] param)
    {
        if ((SectionNode)param[1] == this)
        {
            if ((SectionManager.WaveNumber)param[0] != SectionManager.WaveNumber.NoCuentaParaTerminarNodo)
            {
                if ((SectionManager.WaveNumber)param[0] != SectionManager.WaveNumber.Trigger)
                    _dicQuantityInWave[(SectionManager.WaveNumber)param[0]]--;

                _enemiesRemaining--;

                if (Utility.CalcPercentage(_enemiesRemaining, _allSpawns.Length) < SectionManager.instance.percentageOfEnemysToBerserk)
                {
                    EventManager.instance.ExecuteEvent(Constants.BERSERK);
                }
            }

            if (param[2] is NormalEnemyBehaviour)
                Utility.RemoveFromListGeneric(_allNormalActives, (NormalEnemyBehaviour)param[2]);

            else if (param[2] is ChargerEnemyBehaviour)
                Utility.RemoveFromListGeneric(_allChargerActives, (ChargerEnemyBehaviour)param[2]);

            else if (param[2] is EnemyTurretBehaviour)
                Utility.RemoveFromListGeneric(_allTurretsActives, (EnemyTurretBehaviour)param[2]);

            else if (param[2] is PowerUpChaserEnemy)
            {
                Utility.RemoveFromListGeneric(_allChasersActives, (PowerUpChaserEnemy)param[2]);
                if (TutorialBehaviour.instance != null && TutorialBehaviour.instance.IsTutorialNode)
                {
                    TutorialBehaviour.instance.GreenKilled = true;
                }
            }

            else if (param[2] is MiniBossBehaviour)
            {
                var e = (MiniBossBehaviour)param[2];
                Utility.RemoveFromListGeneric(_allMiniBoss, e);
                Destroy(e.gameObject);
            }
            else if (param[2] is CubeEnemyBehaviour)
            {
                Utility.RemoveFromListGeneric(_allCubeActives, (CubeEnemyBehaviour)param[2]);
            }
            else if (param[2] is MisilEnemy)
            {
                Utility.RemoveFromListGeneric(_allMisilEnemiesActive, (MisilEnemy)param[2]);
            }
            else if (param[2] is FireEnemy)
            {
                Utility.RemoveFromListGeneric(_allFireEnemiesActive, (FireEnemy)param[2]);
            }

            var absE = (AbstractEnemy)param[2];
            absE.UnSubscribeToIndicator();

            if ((bool)param[3] == false)
            { //el verde no deberia hacer particulas si no le pega el player
                EventManager.instance.ExecuteEvent(Constants.PARTICLE_SET, new object[] { Constants.PARTICLE_ENEMY_EXPLOTION_NAME, absE.transform.position });
            }

            if ((bool)param[4] == true)
            { // esto es para saber si se tiene que romper una pared o algo
                _quantityPerIDDictionary[(int)param[5]]--; //param 5 es el id

                List<int> keys = new List<int>(_quantityPerIDDictionary.Keys);

                foreach (var k in keys)
                {
                    if (_quantityPerIDDictionary[k] <= 0)
                    {
                        foreach (var w in _allWallsThatHaveToUnlockDestroying)
                        {
                            if (w.id == k)
                            {
                                w.Show(false);
                                //             w.gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }

    //la idea es vaciar todo cuando se reinicia y se termina una seccion, asi no le pega nada cuaando la gana
    protected void StopAllEnemies()
    {
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

        if (isBossNode)
        {
            if (Configuration.instance.lvl == 1)
            {
                bossOnScreen.DeleteAll();
                Destroy(bossOnScreen.gameObject);
                var foundMisiles = FindObjectsOfType<Missile>();
                foreach (var misil in foundMisiles)
                {
                    misil.DestroyMissile();
                }
            }
            else if (Configuration.instance.lvl == 2)
            {
                if (bossSerpentLeftOnScreen != null)
                {
                    bossSerpentLeftOnScreen.Destroy();
                }
                if (bossSerpentRightOnScreen != null)
                {
                    bossSerpentRightOnScreen.Destroy();
                }
            }
        }
    }

    public void RestartSection()
    {
        StopAllEnemies();
        SpawnPlayerInSpawnPoint(EnemiesManager.instance.player);

        if (Configuration.instance != null)
        {
            if (Configuration.instance.Multiplayer())
            {
                SpawnPlayerInSpawnPoint(EnemiesManager.instance.player2);
            }
        }
        object[] conteiner = new object[2]; // container
        conteiner[0] = "in"; 
        conteiner[1] = this;
        EventManager.instance.ExecuteEvent(Constants.START_SECTION, conteiner);

        if (isBossNode)
        {

            SetBoss();
            //return;
        }

        foreach (var trigger in triggerSpawners)
        {
            if (trigger != null)
                trigger.SetActive(true);
        }

        if (_allProps.Length > 0)
        {
            foreach (var prop in _allProps)
            {
                prop.RestartProp();
            }
        }

        LootTableManager.instance.DestroyAllPowerUps();

        var damagesPath = FindObjectsOfType<DamagePath>();

        foreach (var item in damagesPath)
        {
            item.DeleteAll();
        }

        var followBullets = FindObjectsOfType<FollowBullets>();

        foreach (var item in followBullets)
        {
            Destroy(item.gameObject);
        }


        GameObject fire = GameObject.Find("Virtual_Fire(Clone)");
        while (fire != null)
        {
            fire.SetActive(false);
            Destroy(fire);
            fire = GameObject.Find("Virtual_Fire(Clone)");
        }

        GameObject p = EnemiesManager.instance.player;
        DropIfNeededPowerUpHelp(p.transform.position);

        SpawnEnemyAtStart();
        SetQuantityToDestroyToUnlockSomething();

        if (!wavesStartAtTrigger && !isBossNode)
        {
            StartCoroutine(WavesNodeRoutine());
        }

        if (TutorialBehaviour.instance != null && TutorialBehaviour.instance.IsTutorialNode)
        {
            TutorialBehaviour.instance.RestartTutorial();
        }
        _currentTimeBetweenDelayedSpawn = 0;
    }

    protected void DropIfNeededPowerUpHelp(Vector3 position)
    {

        Vector3 spawnPosition = position + new Vector3(0, 0, 4f);
        timesDead++;
        if (timesDead >= timesDeadTillPowerHelp && Configuration.instance.dificulty == Configuration.Dificulty.Easy)
        {
            LootTableManager.instance.DropPowerUp(spawnPosition, false);
        }
    }

    public void SpawnPlayerInSpawnPoint(GameObject player)
    {
        player.transform.position = playerSpawnPoint.position;
        player.gameObject.SetActive(true);
    }

    public void StartTriggerMultipleSpawnRoutine(int quantity, MultiEnemySpawner sp)
    {
        StartCoroutine(MultipleSpawnTriggerRoutine(quantity, sp));
    }

    protected IEnumerator MultipleSpawnTriggerRoutine(int quantity, MultiEnemySpawner sp)
    {
        yield return _waitBetweenWaves;
        while (_paused)
            yield return null;
        for (int i = 0; i < quantity; i++)
        {
            var n = EnemiesManager.instance.giveMeNormalEnemy().SetActualNode(this).SetActualWave(SectionManager.WaveNumber.Trigger).SetIntegration(timeBetweenSpawns).SubscribeToIndicator().SetIffHasToDestroyToOpenSomething(sp.hasToDestroyToUnlockSomething, sp.idOfWall) as NormalEnemyBehaviour;
            n.SetTarget(EnemiesManager.instance.player.transform).SetPosition(sp.GetPositionWithOffset).gameObject.SetActive(true);
            _allNormalActives.Add(n);
            yield return _waitBetweenSpawns;
            while (_paused)
                yield return null;
        }
    }

    public void StartNodeRoutine()
    {
        StartCoroutine(WavesNodeRoutine());
    }

    public void TriggerSpawn(EnemySpawner[] allTriggerSpawners)
    {
        StartCoroutine(TriggerRoutine(allTriggerSpawners));
    }

    protected IEnumerator TriggerRoutine(EnemySpawner[] allTriggerSpawners)
    {
        yield return _waitBetweenWaves;
        while (_paused)
            yield return null;

        foreach (var sP in allTriggerSpawners)
        {
            yield return new WaitForSeconds(sP.extraWaitToSpawn);
            while (_paused)
                yield return null;
            StartCoroutine(SpawnEnemy(sP, SectionManager.WaveNumber.Trigger));
        }
    }

    protected virtual IEnumerator WavesNodeRoutine()
    {
        if (TutorialBehaviour.instance != null && TutorialBehaviour.instance.IsTutorialNode)
        {
            LootTableManager.instance.SetTutoProbability();
            EventManager.instance.ExecuteEvent(Constants.UI_TUTORIAL_CHANGE, UIManager.TUTORIAL_SHOOT);
        }
        _currentTimeBetweenDelayedSpawn = 0;
        SetWaves(SectionManager.WaveNumber.First);
        yield return _waitBetweenWaves;

        while (_paused)
            yield return null;

        while (_dicQuantityInWave[SectionManager.WaveNumber.First] > 0)
            yield return null;

        if (TutorialBehaviour.instance != null && TutorialBehaviour.instance.IsTutorialNode)
        {
            TutorialBehaviour.instance.FirstEnemyKIlled();

            while (!TutorialBehaviour.instance.GreenKilled)
            {
                yield return null;
            }
        }
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

        if (TutorialBehaviour.instance != null && TutorialBehaviour.instance.IsTutorialNode)
        {
            EventManager.instance.ExecuteEvent(Constants.UI_TUTORIAL_DEACTIVATED);
        }
    }

    protected void SetWaves(SectionManager.WaveNumber wave)
    {
        SoundManager.instance.PlaySpawnEnemy();
        if (!_dicSpawn.ContainsKey(wave))
            return;

        foreach (var spawnPoint in _dicSpawn[wave])
        {
            if (spawnPoint.waveOfSpawn != wave || spawnPoint.triggerSpawn)
                return;
            _dicQuantityInWave[wave]++;

            StartCoroutine(SpawnEnemy(spawnPoint, wave));
        }
    }

    protected IEnumerator SpawnEnemy(EnemySpawner sP, SectionManager.WaveNumber wave)
    {
        if (waveDelayedSpawn && wave == waveNumber)
        {
            _currentTimeBetweenDelayedSpawn += timeBetweenDelayedSpawn;
        }
        yield return new WaitForSeconds(_currentTimeBetweenDelayedSpawn);

        while (_paused)
            yield return null;

        yield return new WaitForSeconds(sP.extraWaitToSpawn);

        while (_paused)
            yield return null;

        switch (sP.typeOfEnemy)
        {
            case EnemiesManager.TypeOfEnemy.Normal:
                var n = EnemiesManager.instance.giveMeNormalEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves).SubscribeToIndicator().SetIffHasToDestroyToOpenSomething(sP.hasToDestroyToUnlockSomething, sP.idOfWall) as NormalEnemyBehaviour;
                n.SetTarget(EnemiesManager.instance.player.transform).SetPosition(sP.transform.position).gameObject.SetActive(true);
                _allNormalActives.Add(n);
                break;

            case EnemiesManager.TypeOfEnemy.Charger:
                var c = EnemiesManager.instance.giveMeChargerEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves).SetTimeAndRenderer().SubscribeToIndicator().SetIffHasToDestroyToOpenSomething(sP.hasToDestroyToUnlockSomething, sP.idOfWall) as ChargerEnemyBehaviour;
                c.SetTarget(EnemiesManager.instance.player.transform).SetPosition(sP.transform.position).gameObject.SetActive(true);
                _allChargerActives.Add(c);
                break;

            case EnemiesManager.TypeOfEnemy.TurretBurst:
                var t = EnemiesManager.instance.giveMeTurretEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves).SetTimeAndRenderer().SubscribeToIndicator().SetIffHasToDestroyToOpenSomething(sP.hasToDestroyToUnlockSomething, sP.idOfWall) as EnemyTurretBehaviour;
                t.SetPosition(sP.transform.position).SetType(EnemiesManager.TypeOfEnemy.TurretBurst).gameObject.SetActive(true);
                _allTurretsActives.Add(t);
                break;

            case EnemiesManager.TypeOfEnemy.TurretLaser:
                var tL = EnemiesManager.instance.giveMeTurretEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves).SubscribeToIndicator().SetIffHasToDestroyToOpenSomething(sP.hasToDestroyToUnlockSomething, sP.idOfWall) as EnemyTurretBehaviour;
                tL.SetPosition(sP.transform.position).SetForward(sP.transform.forward).SetType(EnemiesManager.TypeOfEnemy.TurretLaser).gameObject.SetActive(true);
                _allTurretsActives.Add(tL);
                break;

            case EnemiesManager.TypeOfEnemy.Cube:
                var cu = EnemiesManager.instance.GiveMeCubeEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves).SetTimeAndRenderer().SubscribeToIndicator().SetIffHasToDestroyToOpenSomething(sP.hasToDestroyToUnlockSomething, sP.idOfWall) as CubeEnemyBehaviour;
                cu.SetTarget(EnemiesManager.instance.player.transform).SetPosition(sP.transform.position).gameObject.SetActive(true);
                _allCubeActives.Add(cu);
                break;
            case EnemiesManager.TypeOfEnemy.MisilEnemy:
                var m = EnemiesManager.instance.GiveMeMisilEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves).SetTimeAndRenderer().SubscribeToIndicator().SetIffHasToDestroyToOpenSomething(sP.hasToDestroyToUnlockSomething, sP.idOfWall) as MisilEnemy;

                m.SetPosition(sP.transform.position).SetTarget(EnemiesManager.instance.player.transform).gameObject.SetActive(true);
                _allMisilEnemiesActive.Add(m);
                break;
            case EnemiesManager.TypeOfEnemy.FireEnemy:
                var f = EnemiesManager.instance.GiveMeFireEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves).SetTimeAndRenderer().SubscribeToIndicator().SetIffHasToDestroyToOpenSomething(sP.hasToDestroyToUnlockSomething, sP.idOfWall) as FireEnemy;
                f.SetPosition(sP.transform.position).SetTarget(EnemiesManager.instance.player.transform).gameObject.SetActive(true);
                _allFireEnemiesActive.Add(f);
                break;
        }
    }

    public void SpawnEnemyAtPointAtStartNoCuentaParaTerminarNodo(EnemySpawnerAtBeginningOfNode sp)
    {
        var wave = SectionManager.WaveNumber.NoCuentaParaTerminarNodo;
        switch (sp.typeOfEnemy)
        {
            case EnemiesManager.TypeOfEnemy.Normal:
                var n = EnemiesManager.instance.giveMeNormalEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(0f).SetIffHasToDestroyToOpenSomething(sp.hasToDestroyToUnlockSomething, sp.idOfWall) as NormalEnemyBehaviour;
                n.SetTarget(EnemiesManager.instance.player.transform).SetPosition(sp.transform.position).gameObject.SetActive(true);
                if (sp.parent != null)
                {
                    n.transform.SetParent(sp.parent);
                }
                _allNormalActives.Add(n);
                break;

            case EnemiesManager.TypeOfEnemy.Charger:
                var c = EnemiesManager.instance.giveMeChargerEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(0f).SetTimeAndRenderer().SetIffHasToDestroyToOpenSomething(sp.hasToDestroyToUnlockSomething, sp.idOfWall) as ChargerEnemyBehaviour;
                c.SetTarget(EnemiesManager.instance.player.transform).SetPosition(sp.transform.position).gameObject.SetActive(true);
                if (sp.parent != null)
                {
                    c.transform.SetParent(sp.parent);
                }
                _allChargerActives.Add(c);
                break;
            case EnemiesManager.TypeOfEnemy.TurretBurst:
                var t = EnemiesManager.instance.giveMeTurretEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(0f).SetTimeAndRenderer().SetIffHasToDestroyToOpenSomething(sp.hasToDestroyToUnlockSomething, sp.idOfWall) as EnemyTurretBehaviour;
                t.SetPosition(sp.transform.position).SetType(EnemiesManager.TypeOfEnemy.TurretBurst).gameObject.SetActive(true);
                if (sp.parent != null)
                {
                    t.transform.SetParent(sp.parent);
                }
                _allTurretsActives.Add(t);
                break;

            case EnemiesManager.TypeOfEnemy.TurretLaser:
                var tL = EnemiesManager.instance.giveMeTurretEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(0f).SetIffHasToDestroyToOpenSomething(sp.hasToDestroyToUnlockSomething, sp.idOfWall) as EnemyTurretBehaviour;
                tL.SetPosition(sp.transform.position).SetForward(sp.transform.forward).SetType(EnemiesManager.TypeOfEnemy.TurretLaser).gameObject.SetActive(true);
                _allTurretsActives.Add(tL);
                if (sp.parent != null)
                {
                    tL.transform.SetParent(sp.parent);
                }
                break;

            case EnemiesManager.TypeOfEnemy.MovingTurretLaser:
                var MTL = EnemiesManager.instance.giveMeTurretEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(0f).SetTimeAndRenderer().SetIffHasToDestroyToOpenSomething(sp.hasToDestroyToUnlockSomething, sp.idOfWall) as EnemyTurretBehaviour;
                MTL.SetPosition(sp.transform.position).SetForward(sp.transform.forward).SetType(EnemiesManager.TypeOfEnemy.MovingTurretLaser, sp.startTurretNode, sp.hasToHaveShield).gameObject.SetActive(true);
                _allTurretsActives.Add(MTL);
                if (sp.parent != null)
                {
                    MTL.transform.SetParent(sp.parent);
                }
                break;

            case EnemiesManager.TypeOfEnemy.Cube:
                var cu = EnemiesManager.instance.GiveMeCubeEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(0f).SetTimeAndRenderer().SetIffHasToDestroyToOpenSomething(sp.hasToDestroyToUnlockSomething, sp.idOfWall) as CubeEnemyBehaviour;
                cu.SetTarget(EnemiesManager.instance.player.transform).SetPosition(sp.transform.position).gameObject.SetActive(true);
                _allCubeActives.Add(cu);
                if (sp.parent != null)
                {
                    cu.transform.SetParent(sp.parent);
                }
                break;
            case EnemiesManager.TypeOfEnemy.MisilEnemy:
                var m = EnemiesManager.instance.GiveMeMisilEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(0f).SetTimeAndRenderer().SetIffHasToDestroyToOpenSomething(sp.hasToDestroyToUnlockSomething, sp.idOfWall) as MisilEnemy;

                m.SetPosition(sp.transform.position).SetTarget(EnemiesManager.instance.player.transform).SubscribeToEvents().gameObject.SetActive(true);
                _allMisilEnemiesActive.Add(m);
                break;
        }
    }

    public void SpawnEnemyAtPointNoCuentaParaTerminarNodoPeroTieneIntegracion(Vector3 spawnPoint, EnemiesManager.TypeOfEnemy type)
    {
        var wave = SectionManager.WaveNumber.NoCuentaParaTerminarNodo;
        switch (type)
        {
            case EnemiesManager.TypeOfEnemy.Normal:
                var n = EnemiesManager.instance.giveMeNormalEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves).SubscribeToIndicator().SetIffHasToDestroyToOpenSomething(false, 0) as NormalEnemyBehaviour;
                n.SetTarget(EnemiesManager.instance.player.transform).SetPosition(spawnPoint).gameObject.SetActive(true);
                _allNormalActives.Add(n);
                break;

            case EnemiesManager.TypeOfEnemy.Charger:
                var c = EnemiesManager.instance.giveMeChargerEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves).SetTimeAndRenderer().SubscribeToIndicator().SetIffHasToDestroyToOpenSomething(false, 0) as ChargerEnemyBehaviour;
                c.SetTarget(EnemiesManager.instance.player.transform).SetPosition(spawnPoint).gameObject.SetActive(true);
                _allChargerActives.Add(c);
                break;

            case EnemiesManager.TypeOfEnemy.MisilEnemy:
                var m = EnemiesManager.instance.GiveMeMisilEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves).SetTimeAndRenderer().SubscribeToIndicator().SetIffHasToDestroyToOpenSomething(false, 0) as MisilEnemy;
                m.SetTarget(EnemiesManager.instance.player.transform).SetPosition(spawnPoint).gameObject.SetActive(true);
                _allMisilEnemiesActive.Add(m);
                break;
        }
    }


    protected void OnPowerUpDropped(params object[] paramss)
    {
        if (id == 0) LootTableManager.instance.SetDefaultProbavility();

        var go = (GameObject)paramss[1];

        if ((SectionNode)paramss[0] != this || isBossNode && !spawnGreenEnemiesInBossNode)
            return;

        var pos = Utility.RandomVector3InRadiusCountingBoundariesInRectDirection(go.transform.position, radiusToSetPowerUpChaser, objectToDetectConnectingNodes);

        onGizmosSafeZoneChaser = pos;

        _enemiesRemaining++;

        var pUC = EnemiesManager.instance.GiveMeChaserEnemy().SetActualNode(this).SetActualWave(SectionManager.WaveNumber.Trigger).SetIntegration(timeBetweenWaves).SetTimeAndRenderer() as PowerUpChaserEnemy;
        pUC.SetStart().SetPosition(pos).gameObject.SetActive(true);

        _allChasersActives.Add(pUC);
    }

    public void SpawnMiniBoss(Vector3 pos, SectionManager.WaveNumber wave)
    {
        _enemiesRemaining++;

        var mB = Instantiate(EnemiesManager.instance.miniBossPrefab, new Vector3(pos.x,pos.y - 1, pos.z), Quaternion.identity)
                                     .GetComponent<AbstractEnemy>();
        mB.SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves).SetTimeAndRenderer();//deberia ser trigger la wave
        mB.transform.forward = Utility.SetYInVector3(EnemiesManager.instance.player.transform.position, pos.y) - pos;
        _allMiniBoss.Add(mB as MiniBossBehaviour);
    }

    public void SetEnemiesRemaining()
    {
        _enemiesRemaining = _allSpawns.Length;
        _enemiesRemaining += _allQuantityInMultiEnemySpawners;
    }

    public MapNode GetClosestMapNode(Vector3 pos)
    {
        MapNode t = null;
        float returnMNDistance = float.MaxValue;
        foreach (var mN in _allMapNodes)
        {
            var distanceFromActualMN = Vector3.Distance(mN.transform.position, pos);

            if (distanceFromActualMN < returnMNDistance)
            {
                t = mN;
                returnMNDistance = distanceFromActualMN;
            }
        }
        if (t == null)
            return null;

        return t;
    }

    public bool sectionHasMapNodes { get { return _allMapNodes.Length > 0; } }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(onGizmosSafeZoneChaser, radiusToSetPowerUpChaser);
    }
}