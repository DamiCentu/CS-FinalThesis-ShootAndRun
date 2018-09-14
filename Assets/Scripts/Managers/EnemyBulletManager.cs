using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletManager : MonoBehaviour {
    public static EnemyBulletManager instance { get; private set; }

    Pool<EnemyBulletBehaviour> _poolOfEnemyBullets;
    List<EnemyBulletBehaviour> _allBullets;

    public GameObject enemyBulletPrefab;

    GameObject _bulletContainer;

    private void Awake() {
        Debug.Assert(FindObjectsOfType<EnemyBulletManager>().Length == 1);
        if (instance == null)
            instance = this;

        _bulletContainer = new GameObject("EnemyBulletContainer");

        _allBullets = new List<EnemyBulletBehaviour>();
        _poolOfEnemyBullets = new Pool<EnemyBulletBehaviour>(50, BulletFactoryMethod, null, null, true);
    }

    public EnemyBulletBehaviour BulletFactoryMethod() {
        var a = Instantiate(enemyBulletPrefab.GetComponent<EnemyBulletBehaviour>(), _bulletContainer.transform);
        a.gameObject.SetActive(false);
        return a;
    }

    public EnemyBulletBehaviour giveMeEnemyBullet() {
        var a = _poolOfEnemyBullets.GetObjectFromPool();
        if(!_allBullets.Contains(a))
            _allBullets.Add(a);
        return a;
    }

    public void ReturnEnemyBulletToPool(EnemyBulletBehaviour bullet) { 
        _poolOfEnemyBullets.DisablePoolObject(bullet);
    }

    public void ReturnAllBullets() {  
        for (int i = 0; i < _allBullets.Count; i++) {
            var b = _allBullets[i];
            if (!b.gameObject.activeSelf)
                continue;
            b.gameObject.SetActive(false);
            ReturnEnemyBulletToPool(b); 
        }
    } 
}
