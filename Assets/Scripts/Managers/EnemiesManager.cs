using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour {
    public static EnemiesManager instance { get; private set; }

    public bool showThetaStarPathWithGizmos = false;
    public bool showFollowPathGizmos = false;

    public GameObject player;
    public GameObject player2;

    public GameObject normalEnemyPrefab;
    public GameObject chargerEnemyPrefab;
    public GameObject turretEnemyPrefab;
    public GameObject powerUpChaserPrefab;
    public GameObject miniBossPrefab;
    public GameObject bossPrefab;
    public GameObject cubeEnemyPrefab;
//    public GameObject misilEnemyPrefab;

    Pool<NormalEnemyBehaviour> _poolOfNormalEnemy;
    Pool<ChargerEnemyBehaviour> _poolOfChargerEnemy;
    Pool<EnemyTurretBehaviour> _poolOfTurretEnemy;
    Pool<PowerUpChaserEnemy> _poolOfChaserEnemy;
    Pool<CubeEnemyBehaviour> _poolOfCubeEnemy;
   // Pool<MisilEnemy> _poolOfMisilEnemy;

    GameObject _normalContainer;
    GameObject _turretContainer;
    GameObject _chargerContainer;
    GameObject _chaserContainer;
    GameObject _cubeContainer;
 //   GameObject _misilContainer;

    void Awake() {
        Debug.Assert(FindObjectsOfType<EnemiesManager>().Length == 1);
        if (instance == null) { 
            instance = this;
        }
        _normalContainer = new GameObject("NormalEnemyContainer");
        _turretContainer = new GameObject("TurretEnemyContainer");
        _chargerContainer = new GameObject("ChargerEnemyContainer");
        _chaserContainer = new GameObject("ChaserEnemyContainer");
        _cubeContainer = new GameObject("CubeEnemyContainer");
        //_misilContainer = new GameObject("MisilEnemyCointener");

        _poolOfNormalEnemy = new Pool<NormalEnemyBehaviour>(30, NormalFactoryMethod, null,null,true);
        _poolOfChargerEnemy = new Pool<ChargerEnemyBehaviour>(10, ChargerFactoryMethod, null, null, true);
        _poolOfTurretEnemy = new Pool<EnemyTurretBehaviour>(10, TurretFactoryMethod, null, null, true);
        _poolOfChaserEnemy = new Pool<PowerUpChaserEnemy>(5, ChaserFactoryMethod, null, null, true);
        _poolOfCubeEnemy = new Pool<CubeEnemyBehaviour>(5, CubeFactoryMethod, null, null, true);
      //  _poolOfMisilEnemy = new Pool<MisilEnemy>(5, MisilFactoryMethod, null, null, true);
    }

    #region NORMAL EnemyBehaviour methods
    public NormalEnemyBehaviour NormalFactoryMethod() {
        var a = Instantiate(normalEnemyPrefab.GetComponent<NormalEnemyBehaviour>(), _normalContainer.transform);
        a.gameObject.SetActive(false);
        return a;
    }

    public NormalEnemyBehaviour giveMeNormalEnemy() {
        return _poolOfNormalEnemy.GetObjectFromPool();
    }

    public void ReturnNormalEnemyToPool(NormalEnemyBehaviour enemy) {
        _poolOfNormalEnemy.DisablePoolObject(enemy);
    }
    #endregion

    #region CHARGER EnemyBehaviour methods
    public ChargerEnemyBehaviour ChargerFactoryMethod() {
        var a = Instantiate(chargerEnemyPrefab.GetComponent<ChargerEnemyBehaviour>(), _chargerContainer.transform);
        a.gameObject.SetActive(false);
        return a;
    }

    public ChargerEnemyBehaviour giveMeChargerEnemy() {
        return _poolOfChargerEnemy.GetObjectFromPool();
    }

    public void ReturnChargerEnemyToPool(ChargerEnemyBehaviour enemy) {
        _poolOfChargerEnemy.DisablePoolObject(enemy);
    }
    #endregion

    #region TURRET EnemyBehaviour methods
    public EnemyTurretBehaviour TurretFactoryMethod() {
        var a = Instantiate(turretEnemyPrefab.GetComponent<EnemyTurretBehaviour>(), _turretContainer.transform);
        a.gameObject.SetActive(false);
        return a;
    }

    public EnemyTurretBehaviour giveMeTurretEnemy() {
        return _poolOfTurretEnemy.GetObjectFromPool();
    }

    public void ReturnTurretEnemyToPool(EnemyTurretBehaviour enemy) {
        _poolOfTurretEnemy.DisablePoolObject(enemy);
    }
    #endregion

    #region CHASER EnemyBehaviour methods
    public PowerUpChaserEnemy ChaserFactoryMethod() {
        var a = Instantiate(powerUpChaserPrefab.GetComponent<PowerUpChaserEnemy>(), _chaserContainer.transform);
        a.gameObject.SetActive(false);
        return a;
    }

    public PowerUpChaserEnemy GiveMeChaserEnemy() {
        return _poolOfChaserEnemy.GetObjectFromPool();
    }

    public void ReturnChaserEnemyToPool(PowerUpChaserEnemy enemy) {
        _poolOfChaserEnemy.DisablePoolObject(enemy);
    }
    #endregion

    #region CUBE EnemyBehaviour methods
    public CubeEnemyBehaviour CubeFactoryMethod() {
        var a = Instantiate(cubeEnemyPrefab.GetComponent<CubeEnemyBehaviour>(), _cubeContainer.transform);
        a.gameObject.SetActive(false);
        return a;
    }

    public CubeEnemyBehaviour GiveMeCubeEnemy() {
        return _poolOfCubeEnemy.GetObjectFromPool();
    }

    public void ReturnCubeEnemyToPool(CubeEnemyBehaviour enemy) {
        _poolOfCubeEnemy.DisablePoolObject(enemy);
    }
    #endregion
    /*
    #region Misil EnemyBehaviour methods
    public MisilEnemy MisilFactoryMethod()
    {
        var a = Instantiate(misilEnemyPrefab.GetComponent<MisilEnemy>(), _misilContainer.transform);
        a.gameObject.SetActive(false);
        return a;
    }

    public MisilEnemy GiveMeMisilEnemy()
    {
        return _poolOfMisilEnemy.GetObjectFromPool();
    }

    public void ReturnMisilEnemyToPool(MisilEnemy enemy)
    {
        _poolOfMisilEnemy.DisablePoolObject(enemy);
    }
    #endregion
    */

    public enum TypeOfEnemy {
        Normal,
        Charger,
        TurretBurst,
        TurretLaser,
        PowerUpChaser,
        Cube,
        Boss
    } 
}
