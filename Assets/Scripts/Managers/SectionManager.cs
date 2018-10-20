using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SectionManager : MonoBehaviour {
    public SectionNode actualNode;
    public static SectionManager instance { get; private set; }

    public float waitTimeForNextNodeSectionWhenFinish = 2f;
    public float waitTimeForStartNode = 1f;
    public float timeToRestartAfterPlayerDead = 2f;

    public float enemiesMultiplicatorOnSlow = .0f;
    public float slowDuration = 2f;
    public int percentageOfEnemysToBerserk = 5;
    public float enemiesMultiplicatorOnBerserk = 1.3f;
    public float timeAfterWinning = 2f;
    public float timeAfterLosing = 2f;

    float _enemiesMultiplator = 1f; 

    public float EnemiesMultiplicator { get { return _enemiesMultiplator; } } 

    public float timeSplicingQuoteForSectionRoutine = 0.001f;

    void Awake()
    {
        Debug.Assert(FindObjectsOfType<SectionManager>().Length == 1);
        if (instance == null)
            instance = this;
    }
    void Start() {
        if (Configuration.instance.dificulty == Configuration.Dificulty.Easy)
        {
            Constants.ENEMIES_NORMAL_MULTIPLICATOR = 0.8f;
        }
        else if (Configuration.instance.dificulty == Configuration.Dificulty.Medium)
        {
            Constants.ENEMIES_NORMAL_MULTIPLICATOR = 1f;
        }
        else if (Configuration.instance.dificulty == Configuration.Dificulty.Hard)
        {
            Constants.ENEMIES_NORMAL_MULTIPLICATOR = 1.2f;
        }
        EventManager.instance.AddEvent(Constants.ENEMY_DEAD);
        EventManager.instance.AddEvent(Constants.PLAYER_DEAD);
        EventManager.instance.AddEvent(Constants.SLOW_TIME);
        EventManager.instance.AddEvent(Constants.BERSERK);
        EventManager.instance.AddEvent(Constants.STOP_BERSERK);

        EventManager.instance.SubscribeEvent(Constants.PLAYER_DEAD, AtPlayerDead);
        EventManager.instance.SubscribeEvent(Constants.SLOW_TIME, AtSlowTime);
        EventManager.instance.SubscribeEvent(Constants.BERSERK, AtBerserk);
        EventManager.instance.SubscribeEvent(Constants.STOP_BERSERK, AtStopBerserk);
        EventManager.instance.SubscribeEvent(Constants.GAME_OVER, OnGameOver);

        EventManager.instance.ExecuteEvent(Constants.STARTED_SECTION_solo_escucha_camera_iTween_noseporque);

        //para que no me rompa las bolas
        if (actualNode == null)
            return;

        actualNode.SpawnPlayerInSpawnPoint(EnemiesManager.instance.player);

        if(Configuration.instance != null) { 
            if (Configuration.instance.Multiplayer())
            {
                actualNode.SpawnPlayerInSpawnPoint(EnemiesManager.instance.player2);
            }
        }

        StartCoroutine(SectionsRoutine());
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            //EventManager.instance.ExecuteEvent(Constants.SLOW_TIME);
            //actualNode.SpawnEnemyAtPoint(new Vector3(), EnemiesManager.TypeOfEnemy.Normal);
        }
    }

    void AtBerserk(params object[] param) {
        _enemiesMultiplator = enemiesMultiplicatorOnBerserk;
        //Debug.Log("berserk");
    }

    void AtStopBerserk(params object[] param) {
        _enemiesMultiplator = Constants.ENEMIES_NORMAL_MULTIPLICATOR;
        //Debug.Log("stop");
    }

    void AtSlowTime(params object[] param) { 
        _enemiesMultiplator = enemiesMultiplicatorOnSlow;
        StartCoroutine(slowRoutine()); 
    }

    IEnumerator slowRoutine() {
        yield return new WaitForSeconds(slowDuration);
        _enemiesMultiplator = Constants.ENEMIES_NORMAL_MULTIPLICATOR; 
    }

    void AtPlayerDead(params object[] param) {
        _enemiesMultiplator = Constants.ENEMIES_NORMAL_MULTIPLICATOR;
        StartCoroutine(RestartSectionRoutine());
    }

    IEnumerator RestartSectionRoutine() {
        if (actualNode != null) {
            EventManager.instance.ExecuteEvent(Constants.STARTED_SECTION_solo_escucha_camera_iTween_noseporque);
            yield return new WaitForSeconds(timeToRestartAfterPlayerDead);
            actualNode.RestartSection();
         
            actualNode.SetEnemiesRemaining();
        }
    }

    IEnumerator SectionsRoutine() {
        while (actualNode != null) {
            EventManager.instance.ExecuteEvent(Constants.START_SECTION);
            yield return new WaitForSeconds(0.6f);
            EventManager.instance.ExecuteEvent(Constants.BLACK_SCREEN); 
            yield return new WaitForSeconds(waitTimeForStartNode-0.6f);

            if (actualNode.isBossNode) {
                EventManager.instance.ExecuteEvent(Constants.CAMERA_ON_BOSS);
                EnemiesManager.instance.player.transform.position = actualNode.playerSpawnPoint.position;
                EnemiesManager.instance.player.gameObject.SetActive(true);

                if (Configuration.instance != null) { 
                    if (Configuration.instance.Multiplayer()) {
                        EnemiesManager.instance.player2.transform.position = actualNode.playerSpawnPoint.position + new Vector3(3, 0, 0);
                        EnemiesManager.instance.player2.gameObject.SetActive(true);
                    }
                }

                EventManager.instance.ExecuteEvent(Constants.START_SECTION);
                actualNode.SetBoss();

                yield return new WaitForSeconds(actualNode.timeBetweenWaves);

                while (actualNode.BossAlive) {
                    yield return null;
                }
            }
            else {
                if (TutorialBehaviour.instance!=null && TutorialBehaviour.instance.IsTutorialNode) { 
                    //EventManager.instance.ExecuteEvent(Constants.UI_TUTORIAL_ACTIVATED);
                    EventManager.instance.ExecuteEvent(Constants.UI_TUTORIAL_CHANGE, UIManager.TUTORIAL_MOVE);
                }

                if (!actualNode.wavesStartAtTrigger) {
                    actualNode.StartNodeRoutine();
                }
                EnemiesManager.instance.player.transform.position = actualNode.playerSpawnPoint.position;
                EnemiesManager.instance.player.gameObject.SetActive(true);

                if(Configuration.instance != null) { 
                    if (Configuration.instance.Multiplayer()) {
                        EnemiesManager.instance.player2.transform.position = actualNode.playerSpawnPoint.position + new Vector3(3, 0, 0);
                        EnemiesManager.instance.player2.gameObject.SetActive(true);
                    }
                }

                EventManager.instance.ExecuteEvent(Constants.START_SECTION);
                actualNode.SetEnemiesRemaining();

                var wait = new WaitForEndOfFrame();
                var start = Time.realtimeSinceStartup; 
                while (!actualNode.SectionCleared) {
                    //Debug.Log(Time.realtimeSinceStartup - start + " arre "+ timeSplicingQuoteForSectionRoutine);
                    if (Time.realtimeSinceStartup - start > timeSplicingQuoteForSectionRoutine) {
                        yield return wait;
                        start = Time.realtimeSinceStartup;
                    }
                    //yield return null;
                }
                SoundManager.instance.StageComplete();
                InfoManager.instance.CountDown(waitTimeForNextNodeSectionWhenFinish);
                yield return new WaitForSeconds(waitTimeForNextNodeSectionWhenFinish);
                LootTableManager.instance.DestroyAllPowerUps();
            }
            actualNode = actualNode.next;
            EnemiesManager.instance.player.GetComponent<Player>().powerUpManager.RecalculatePowerUp();
            if(actualNode!=null && actualNode.next != null) {
                EventManager.instance.ExecuteEvent(Constants.SOUND_FADE_IN);
            }
        }

        //if (TutorialBehaviour.instance != null) {
        //    TutorialBehaviour.instance.EndTutorial();
        //}
        // throw new System.Exception("GAMEOVER");

        StartCoroutine(WinRoutine());
    }

    IEnumerator WinRoutine() {
        SoundManager.instance.Victory();
        yield return new WaitForSeconds(timeAfterWinning);
        if (Configuration.instance.lvl == 1)
        {
            SceneManager.LoadScene("LvlComplete");
        }
        else {
            SceneManager.LoadScene("VictoryScene");
        }
    }

    private void OnGameOver(object[] parameterContainer) {
        StartCoroutine(LoseRoutine());
    }

    IEnumerator LoseRoutine() {
        SoundManager.instance.GameOver();
        yield return new WaitForSeconds(timeAfterLosing);
        SceneManager.LoadScene("GameOverScene");
    }

    void OnDestroy() {
        EventManager.instance.DeleteEvent(Constants.ENEMY_DEAD);
    }

    public enum WaveNumber {
        First,
        Second,
        Third,
        Trigger,
        NoCuentaParaTerminarNodo
    }
}