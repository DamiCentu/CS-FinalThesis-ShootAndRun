using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionNode : MonoBehaviour {
    public int id;
    public SectionNode next;

    public float timeBetweenWaves = 2f;

    public Transform playerSpawnPoint;

    public GameObject[] triggerSpawners;

    public bool wavesStartAtTrigger = false;

    public LayerMask objectToDetectConnectingNodes;

    public bool isBossNode;

    public Transform constantPointToLookAtOnBoss;
    public Transform cameraPosInLookAtOnBoos;

    public float radiusToSetPowerUpChaser = 7f;
    Vector3 onGizmosSafeZoneChaser = new Vector3(1000f, 1000f, 1000f);

    //public BlockingWallsBehaviour[] blockingWalls;

    EnemySpawner[] _allSpawns;
    EnemySpawnerAtBeginningOfNode[] _allSpawnsThatNotAffectEndOfNode;
    PropsBehaviour[] _allProps;

    MapNode[] _allMapNodes;

    Dictionary<SectionManager.WaveNumber, List<EnemySpawner>> _dicSpawn = new Dictionary<SectionManager.WaveNumber, List<EnemySpawner>>();
    Dictionary<SectionManager.WaveNumber, int> _dicQuantityInWave = new Dictionary<SectionManager.WaveNumber, int>(); 
    
    List<NormalEnemyBehaviour> _allNormalActives = new List<NormalEnemyBehaviour>();
    List<ChargerEnemyBehaviour> _allChargerActives = new List<ChargerEnemyBehaviour>();
    List<EnemyTurretBehaviour> _allTurretsActives = new List<EnemyTurretBehaviour>();
    List<PowerUpChaserEnemy> _allChasersActives = new List<PowerUpChaserEnemy>();
    List<MiniBossBehaviour> _allMiniBoss = new List<MiniBossBehaviour>();
    List<CubeEnemyBehaviour> _allCubeActives = new List<CubeEnemyBehaviour>();
     List<MisilEnemy> _allMisilEnemiesActive= new List<MisilEnemy>();

    WaitForSeconds _waitBetweenWaves; 

    int _enemiesRemaining = 0;
    bool _bossAlive = true;

    int timesDead = 0;
    public bool spawnGreenEnemiesInBossNode = true;


    Boss bossOnScreen;
    public int timesDeadTillPowerHelp=3;

    public bool BossAlive { get { return _bossAlive; } }

    public void SetBoss() {
        StartCoroutine(BossRoutine());
    }
    
    IEnumerator BossRoutine() {
        _allSpawns[0].OnSpawn(timeBetweenWaves);
        yield return _waitBetweenWaves;
        bossOnScreen = Instantiate(EnemiesManager.instance.bossPrefab, _allSpawns[0].transform.position, _allSpawns[0].transform.rotation).GetComponent<Boss>();
        bossOnScreen.GetComponent<AbstractEnemy>().SetTimeAndRenderer().SetActualNode(this);
        bossOnScreen.gameObject.SetActive(true);
    }

    public bool SectionCleared {
        get {
            if (_enemiesRemaining <= 0) { 
                StopAllEnemies();
                return true;
            }
            return false; 
        }
    }
    void Start () {
        _dicSpawn.Add(SectionManager.WaveNumber.First, new List<EnemySpawner>());
        _dicSpawn.Add(SectionManager.WaveNumber.Second, new List<EnemySpawner>());
        _dicSpawn.Add(SectionManager.WaveNumber.Third, new List<EnemySpawner>());

        _dicQuantityInWave.Add(SectionManager.WaveNumber.First, 0);
        _dicQuantityInWave.Add(SectionManager.WaveNumber.Second, 0);
        _dicQuantityInWave.Add(SectionManager.WaveNumber.Third, 0); 

        _allSpawns = transform.GetComponentsInChildren<EnemySpawner>();

        _allSpawnsThatNotAffectEndOfNode = transform.GetComponentsInChildren<EnemySpawnerAtBeginningOfNode>();

        _allProps = transform.GetComponentsInChildren<PropsBehaviour>();

        _allMapNodes = transform.GetComponentsInChildren<MapNode>();

        ConnectMapNodes(); 

        foreach (var spawn in _allSpawns) {
            if (_dicSpawn.ContainsKey(spawn.waveOfSpawn)) {
                if (spawn.triggerSpawn)
                    continue; 
                else
                    _dicSpawn[spawn.waveOfSpawn].Add(spawn);
            }
        }

        _waitBetweenWaves = new WaitForSeconds(timeBetweenWaves);
        EventManager.instance.AddEvent(Constants.BOSS_DESTROYED);

        EventManager.instance.SubscribeEvent(Constants.ENEMY_DEAD, OnEnemyDead);
        EventManager.instance.SubscribeEvent(Constants.POWER_UP_DROPED, OnPowerUpDropped);
        EventManager.instance.SubscribeEvent(Constants.BOSS_DESTROYED, OnBossDestroyed);

        SpawnEnemyAtStart();
    }

    void OnBossDestroyed(params object[] param) {
        _bossAlive = false;
        StopAllEnemies();
    }

    //Spawnea en el en todos los spawns que no afecten en el final del nodo
    void SpawnEnemyAtStart() {
       // SoundManager.instance.PlaySpawnEnemy();
        if (_allSpawnsThatNotAffectEndOfNode.Length == 0)
            return;

        foreach (var sP in _allSpawnsThatNotAffectEndOfNode) {
            SpawnEnemyAtPoint(sP.transform, sP.typeOfEnemy,sP.parent);
        }
    }

    void OnEnemyDead(params object[] param) {
        if ((SectionNode)param[1] == this) {
            if((SectionManager.WaveNumber)param[0] != SectionManager.WaveNumber.NoCuentaParaTerminarNodo) {
                if ((SectionManager.WaveNumber)param[0] != SectionManager.WaveNumber.Trigger)
                    _dicQuantityInWave[(SectionManager.WaveNumber)param[0]]--;

                _enemiesRemaining--;

                if (Utility.CalcPercentage(_enemiesRemaining, _allSpawns.Length) < SectionManager.instance.percentageOfEnemysToBerserk) {
                    EventManager.instance.ExecuteEvent(Constants.BERSERK);
                }
            }

            if (param[2] is NormalEnemyBehaviour)
                Utility.RemoveFromListGeneric(_allNormalActives, (NormalEnemyBehaviour)param[2]);

            else if (param[2] is ChargerEnemyBehaviour)
                Utility.RemoveFromListGeneric(_allChargerActives, (ChargerEnemyBehaviour)param[2]);

            else if (param[2] is EnemyTurretBehaviour)
                Utility.RemoveFromListGeneric(_allTurretsActives, (EnemyTurretBehaviour)param[2]);

            else if (param[2] is PowerUpChaserEnemy) { 
                Utility.RemoveFromListGeneric(_allChasersActives, (PowerUpChaserEnemy)param[2]);
                if(TutorialBehaviour.instance != null) {
                    TutorialBehaviour.instance.GreenKilled = true;
                }
            }
            else if (param[2] is MiniBossBehaviour) {
                var e = (MiniBossBehaviour)param[2];
                Utility.RemoveFromListGeneric(_allMiniBoss, e);
                Destroy(e.gameObject);
            }
            else if(param[2] is CubeEnemyBehaviour) {
                Utility.RemoveFromListGeneric(_allCubeActives, (CubeEnemyBehaviour)param[2]);
            }
            else if (param[2] is MisilEnemy)
            {
                Utility.RemoveFromListGeneric(_allMisilEnemiesActive, (MisilEnemy)param[2]);
            }
            var absE = (AbstractEnemy)param[2];
            absE.UnSubscribeToIndicator();  

            if ((bool)param[3] == true)
                return;

            //el verde no deberia hacer particulas si no le pega el player

            EventManager.instance.ExecuteEvent(Constants.PARTICLE_SET, new object[] { Constants.PARTICLE_ENEMY_EXPLOTION_NAME, absE.transform.position });

        } 
    }  

    //la idea es vaciar todo cuando se reinicia y se termina una seccion, asi no le pega nada cuaando la gana
    void StopAllEnemies() {
        OffScreenIndicatorManager.instance.CleanIndicators();
        StopAllCoroutines();

        EventManager.instance.ExecuteEvent(Constants.STOP_BERSERK);

        EnemyBulletManager.instance.ReturnAllBullets();

        _dicQuantityInWave[SectionManager.WaveNumber.First] = 0;
        _dicQuantityInWave[SectionManager.WaveNumber.Second] = 0;
        _dicQuantityInWave[SectionManager.WaveNumber.Third] = 0;

        Utility.DeactivateList(_allNormalActives);
        Utility.DeactivateList(_allChargerActives);
        Utility.DeactivateList(_allTurretsActives);
        Utility.DeactivateList(_allChasersActives);
        Utility.DeactivateList(_allCubeActives);
        Utility.DeactivateList(_allMisilEnemiesActive);

        _allNormalActives.Clear();
        _allChargerActives.Clear();
        _allTurretsActives.Clear();
        _allChasersActives.Clear();
        _allCubeActives.Clear();
        _allMisilEnemiesActive.Clear();

        Utility.DestroyAllInAndClearList(_allMiniBoss);

        if (isBossNode) {
            Destroy(bossOnScreen.gameObject);
            var foundMisiles = FindObjectsOfType<Missile>();
            foreach (var misil in foundMisiles)
            {
                misil.DestroyMissile();
            }

        }
    }

    public void RestartSection() {
        StopAllEnemies();
        SpawnPlayerInSpawnPoint(EnemiesManager.instance.player);

        if (Configuration.instance != null) {
            if (Configuration.instance.Multiplayer())  {
                SpawnPlayerInSpawnPoint(EnemiesManager.instance.player2);
            }
        }

        EventManager.instance.ExecuteEvent(Constants.START_SECTION);

        if (isBossNode) {
            SetBoss();
            //return;
        }

        foreach (var trigger in triggerSpawners) {
            if (trigger != null)
                trigger.SetActive(true);
        }

        if(_allProps.Length > 0) { 
            foreach (var prop in _allProps) {
                prop.RestartProp();
            }
        }

        IPowerUp[] powerUps = FindObjectsOfType<IPowerUp>();
        foreach (IPowerUp powerUp in powerUps)
        {
            powerUp.gameObject.SetActive(false);
        }


        GameObject p = EnemiesManager.instance.player;
        DropIfNeededPowerUpHelp(p.transform.position);

        SpawnEnemyAtStart();

        if (!wavesStartAtTrigger && !isBossNode) {
            StartCoroutine(WavesNodeRoutine());
        }

        if (TutorialBehaviour.instance != null) {
            TutorialBehaviour.instance.RestartTutorial();
        } 
    }

    private void DropIfNeededPowerUpHelp(Vector3 position) {

        Vector3 spawnPosition = position + new Vector3(0, 0, 4f); 
        timesDead++; 
        if (timesDead >= timesDeadTillPowerHelp && Configuration.instance.dificulty == Configuration.Dificulty.Easy) {
            LootTableManager.instance.DropPowerUp(spawnPosition, false);
        }
    }

    public void SpawnPlayerInSpawnPoint(GameObject player)
    {
        player.transform.position = playerSpawnPoint.position;
        player.gameObject.SetActive(true);
    }

    public void StartNodeRoutine() { 
        StartCoroutine(WavesNodeRoutine());
    } 

    public void TriggerSpawn(EnemySpawner[] allTriggerSpawners) {
        StartCoroutine(TriggerRoutine(allTriggerSpawners));
    }
    
    IEnumerator TriggerRoutine(EnemySpawner[] allTriggerSpawners) {
        yield return _waitBetweenWaves;

        foreach (var sP in allTriggerSpawners) {
            SpawnEnemy(sP, SectionManager.WaveNumber.Trigger);
        }
    }

    IEnumerator WavesNodeRoutine() {
        SetWaves(SectionManager.WaveNumber.First);
        yield return _waitBetweenWaves;

        while (_dicQuantityInWave[SectionManager.WaveNumber.First] > 0)
            yield return null;

        if (TutorialBehaviour.instance != null) {
            TutorialBehaviour.instance.FirstEnemyKIlled();

            while (!TutorialBehaviour.instance.GreenKilled) {
                yield return null;
            }
        }

        SetWaves(SectionManager.WaveNumber.Second);
        yield return _waitBetweenWaves;

        while (_dicQuantityInWave[SectionManager.WaveNumber.Second] > 0)
            yield return null;

        SetWaves(SectionManager.WaveNumber.Third);
        yield return _waitBetweenWaves; 

        while (_dicQuantityInWave[SectionManager.WaveNumber.Third] > 0)
            yield return null; 
    }

    void SetWaves(SectionManager.WaveNumber wave) {
        SoundManager.instance.PlaySpawnEnemy();
        if (!_dicSpawn.ContainsKey(wave))
            return; 

        foreach (var spawnPoint in _dicSpawn[wave]) {
            if ( spawnPoint.waveOfSpawn != wave || spawnPoint.triggerSpawn)
                return;
            _dicQuantityInWave[wave]++;

            SpawnEnemy(spawnPoint,wave);
        }
    } 

    void SpawnEnemy(EnemySpawner spawnPoint, SectionManager.WaveNumber wave) {
         switch (spawnPoint.typeOfEnemy) {
                case EnemiesManager.TypeOfEnemy.Normal:
                    var n = EnemiesManager.instance.giveMeNormalEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves).SubscribeToIndicator() as NormalEnemyBehaviour;
                    n.SetTarget(EnemiesManager.instance.player.transform).SetPosition(spawnPoint.transform.position).gameObject.SetActive(true);
                    _allNormalActives.Add(n);
                    break;

                case EnemiesManager.TypeOfEnemy.Charger:
                    var c = EnemiesManager.instance.giveMeChargerEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves).SetTimeAndRenderer().SubscribeToIndicator() as ChargerEnemyBehaviour;
                    c.SetTarget(EnemiesManager.instance.player.transform).SetPosition(spawnPoint.transform.position).gameObject.SetActive(true);
                    _allChargerActives.Add(c);
                    break;

                case EnemiesManager.TypeOfEnemy.TurretBurst:
                    var t = EnemiesManager.instance.giveMeTurretEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves).SetTimeAndRenderer().SubscribeToIndicator() as EnemyTurretBehaviour;
                    t.SetPosition(spawnPoint.transform.position).SetType(EnemiesManager.TypeOfEnemy.TurretBurst).gameObject.SetActive(true);
                    _allTurretsActives.Add(t);
                    break;

                case EnemiesManager.TypeOfEnemy.TurretLaser:
                    var tL = EnemiesManager.instance.giveMeTurretEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves).SubscribeToIndicator() as EnemyTurretBehaviour;
                    tL.SetPosition(spawnPoint.transform.position).SetForward(spawnPoint.transform.forward).SetType(EnemiesManager.TypeOfEnemy.TurretLaser).gameObject.SetActive(true);
                    _allTurretsActives.Add(tL);
                    break;

                case EnemiesManager.TypeOfEnemy.Cube:
                    var cu = EnemiesManager.instance.GiveMeCubeEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves).SetTimeAndRenderer().SubscribeToIndicator() as CubeEnemyBehaviour;
                    cu.SetTarget(EnemiesManager.instance.player.transform).SetPosition(spawnPoint.transform.position).gameObject.SetActive(true);
                    _allCubeActives.Add(cu);
                    break;
          /*  case EnemiesManager.TypeOfEnemy.MisilEnemy:
                var m = EnemiesManager.instance.GiveMeMisilEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves).SetTimeAndRenderer().SubscribeToIndicator() as MisilEnemy;

                m.SetPosition(spawnPoint.transform.position).SetTarget(EnemiesManager.instance.player.transform).gameObject.SetActive(true);
                _allMisilEnemiesActive.Add(m);
                break;*/
        }
    } 
    
    public void SpawnEnemyAtPoint(Transform spawnTransform, EnemiesManager.TypeOfEnemy type, Transform parent) {
        var wave = SectionManager.WaveNumber.NoCuentaParaTerminarNodo;
        switch (type) {
            case EnemiesManager.TypeOfEnemy.Normal:
                var n = EnemiesManager.instance.giveMeNormalEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(0f) as NormalEnemyBehaviour;
                n.SetTarget(EnemiesManager.instance.player.transform).SetPosition(spawnTransform.transform.position).gameObject.SetActive(true);
                if (parent != null) n.transform.SetParent(parent);
                _allNormalActives.Add(n);
                break;

            case EnemiesManager.TypeOfEnemy.Charger:
                var c = EnemiesManager.instance.giveMeChargerEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(0f).SetTimeAndRenderer() as ChargerEnemyBehaviour;
                c.SetTarget(EnemiesManager.instance.player.transform).SetPosition(spawnTransform.transform.position).gameObject.SetActive(true);
                if (parent != null) c.transform.SetParent(parent);
                _allChargerActives.Add(c);
                break;
            case EnemiesManager.TypeOfEnemy.TurretBurst:
                var t = EnemiesManager.instance.giveMeTurretEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(0f).SetTimeAndRenderer() as EnemyTurretBehaviour;
                t.SetPosition(spawnTransform.transform.position).SetType(EnemiesManager.TypeOfEnemy.TurretBurst).gameObject.SetActive(true);
                if (parent != null) t.transform.SetParent(parent);
                _allTurretsActives.Add(t);
                break;

            case EnemiesManager.TypeOfEnemy.TurretLaser:
                var tL = EnemiesManager.instance.giveMeTurretEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(0f) as EnemyTurretBehaviour;
                tL.SetPosition(spawnTransform.transform.position).SetForward(spawnTransform.transform.forward).SetType(EnemiesManager.TypeOfEnemy.TurretLaser).gameObject.SetActive(true);
                _allTurretsActives.Add(tL);
                if (parent != null) tL.transform.SetParent(parent);
                break;
        }
    }

    public void SpawnEnemyAtPoint(Vector3 spawnPoint, EnemiesManager.TypeOfEnemy type) {
        var wave = SectionManager.WaveNumber.NoCuentaParaTerminarNodo;
        switch (type) {
            case EnemiesManager.TypeOfEnemy.Normal:
                    var n = EnemiesManager.instance.giveMeNormalEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves).SubscribeToIndicator() as NormalEnemyBehaviour;
                    n.SetTarget(EnemiesManager.instance.player.transform).SetPosition(spawnPoint).gameObject.SetActive(true);
                    _allNormalActives.Add(n);
                    break;

                case EnemiesManager.TypeOfEnemy.Charger:
                    var c = EnemiesManager.instance.giveMeChargerEnemy().SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves).SetTimeAndRenderer().SubscribeToIndicator() as ChargerEnemyBehaviour;
                    c.SetTarget(EnemiesManager.instance.player.transform).SetPosition(spawnPoint).gameObject.SetActive(true);
                    _allChargerActives.Add(c);
                    break; 
        }
    }


    void OnPowerUpDropped(params object [] paramss) {
        if (id == 0) LootTableManager.instance.SetDefaultProbavility();

        if ((SectionNode)paramss[0] != this || isBossNode && !spawnGreenEnemiesInBossNode)
            return;

        var go = (GameObject)paramss[1];

        var pos = Utility.RandomVector3InRadiusCountingBoundaries(go.transform.position,radiusToSetPowerUpChaser,objectToDetectConnectingNodes);

        onGizmosSafeZoneChaser = pos;

        _enemiesRemaining++;

        var pUC = EnemiesManager.instance.GiveMeChaserEnemy().SetActualNode(this).SetActualWave(SectionManager.WaveNumber.Trigger).SetIntegration(timeBetweenWaves).SetTimeAndRenderer() as PowerUpChaserEnemy;
        pUC.SetStart().SetPosition(pos).gameObject.SetActive(true);

        _allChasersActives.Add(pUC);

        //foreach (var c in _allChasersActives) {
        //    c.OnPowerUpDropped();
        //}
    }

    public void SpawnMiniBoss(Vector3 pos, SectionManager.WaveNumber wave) {
        _enemiesRemaining++;

        var mB = Instantiate(EnemiesManager.instance.miniBossPrefab, new Vector3(pos.x,pos.y-1f,pos.z), Quaternion.Euler(Utility.SetYInVector3( EnemiesManager.instance.player.transform.position,pos.y) - pos))
                                     .GetComponent<AbstractEnemy>();
        mB.SetActualNode(this).SetActualWave(wave).SetIntegration(timeBetweenWaves ).SetTimeAndRenderer();//deberia ser trigger la wave
        _allMiniBoss.Add(mB as MiniBossBehaviour);
    }

    public void SetEnemiesRemaining() {
        _enemiesRemaining = _allSpawns.Length;
    } 

    public MapNode GetClosestMapNode(Vector3 pos) {
        MapNode t = null;
        float returnMNDistance = float.MaxValue;
        foreach (var mN in _allMapNodes) { 
            var distanceFromActualMN = Vector3.Distance(mN.transform.position, pos);
            
            if (distanceFromActualMN < returnMNDistance) {
                t = mN;
                returnMNDistance = distanceFromActualMN;
            }
        }
        if (t == null)
            return null;

        return t;
    }

    public bool sectionHasMapNodes { get { return _allMapNodes.Length > 0; } }

    void ConnectMapNodes() { 
        foreach (var actual in _allMapNodes) {
            foreach (var maybeAdjacent in _allMapNodes) {
                if (maybeAdjacent == actual)
                    continue;

                var direction = maybeAdjacent.transform.position - actual.transform.position;
                var dirNormalized = direction.normalized;
                var dirMag = direction.magnitude;
                if (!Physics.Raycast(actual.transform.position, dirNormalized, dirMag, objectToDetectConnectingNodes))
                    actual.adjacent.Add(maybeAdjacent);
            }
        } 
    } 

    void OnDrawGizmos() {
        if (_allMapNodes == null) return;
        foreach (var node in _allMapNodes) {
            if (node == null) return;
            foreach (var adj in node.adjacent) {
                if (adj == null) return;
                Gizmos.color = Color.grey;
                Gizmos.DrawLine(node.transform.position, adj.transform.position); 
            } 
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(onGizmosSafeZoneChaser, radiusToSetPowerUpChaser);
    }
}
