using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePath : MonoBehaviour , IPauseable {
    private float speed;
    private Vector3 _startPosition;
    private float _distanceTraveled;
    private float _maxDistance = 10000;
    private Vector3 _direction;
    public float timeAlive;
    GameObject particles;
    public string particlesName= "Virtual_Fire";
    public string prefabName = "fire3";
    public float distanceBetweenSpawns;
     float _distanceToSpawn;
    private bool _shouldStopSpawning = true;
    private List<GameObject> AllGameObjects= new List<GameObject>();

    bool _paused;
    public GameObject prefab_collider;

    public void OnPauseChange(bool v)
    {
        _paused = v;
    }

    private void Start()
    {
        particles = GameObject.Find(particlesName);
        if (particles == null) print("che es null!");
        var aux = GameObject.Find(prefabName);
        if (aux == null) print("che es null!");
        prefab_collider = Instantiate(aux);
    }

    public void SpawnDirection(Vector3 spawnPos, Vector3 direction,float speed) {

        _direction = direction;

        set(spawnPos, speed);
        _maxDistance = 10000;
    }

    public void SpawnPosition(Vector3 spawnPos, Vector3 endPos, float speed)
    {
        _maxDistance = Vector3.Distance(spawnPos, endPos);
        _direction = endPos - spawnPos;
        set(spawnPos, speed);
    }

    private void set(Vector3 spawnPos, float speed)
    {
        _shouldStopSpawning = false;
        _startPosition = spawnPos;
        _distanceTraveled = 0;
        _distanceToSpawn = 0;
        this.speed = speed;
        particles.SetActive(false);
        particles.transform.position = spawnPos;
        particles.SetActive(true);
    }

    internal void DeleteAll()
    {
        foreach (var item in AllGameObjects)
        {
            if (item != null) Destroy(item);
        }
        AllGameObjects.Clear();
        _distanceTraveled = 0;
        _distanceToSpawn = 0;
    }

    // Update is called once per frame
    void Update () {
        if (_paused)
            return;

        if (_maxDistance > _distanceTraveled && !_shouldStopSpawning) {

            _distanceTraveled +=  speed * Time.deltaTime;
            _distanceToSpawn += speed * Time.deltaTime;
            particles.transform.position += speed * Time.deltaTime* _direction;
            if (_distanceToSpawn > distanceBetweenSpawns) { 
                _distanceToSpawn = 0;
                Vector3 spawnPos = _startPosition + _direction * _distanceTraveled;
                //      GameObject p= Instantiate(particles, spawnPos, this.transform.rotation);
                GameObject p = Instantiate(prefab_collider, spawnPos, this.transform.rotation);
                p.gameObject.SetActive(true);
                AllGameObjects.Add(p);
               Destroy(p.gameObject, timeAlive);

            }
        }
	}

    public void Stop() {
        _shouldStopSpawning = true;

    }
}
