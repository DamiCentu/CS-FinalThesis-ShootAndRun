using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePath : MonoBehaviour {
    public float speed=10;
    private Vector3 _startPosition;
    private float _distanceTraveled;
    private float _maxDistance = 10000;
    private Vector3 _direction;
    public float timeAlive;
    GameObject particles;
    public string particlesName;
    public float maxDistanceToSpawn;
     float _distanceToSpawn;
    private bool _shouldStopSpawning = true;

    public void SpawnDirection(Vector3 spawnDirection, Vector3 direction) {
        _startPosition = spawnDirection;
        _direction = direction;
        _shouldStopSpawning = false;
        particles = GameObject.Find(particlesName);
        if (particles == null) print("che es null!");
    }

    public void SpawnPosition(Vector3 spawnDirection, Vector3 endPos)
    {
        _startPosition = spawnDirection;
        _maxDistance = Vector3.Distance(spawnDirection, endPos);
        _direction = endPos - spawnDirection;
        _shouldStopSpawning = false;
    }

    // Update is called once per frame
    void Update () {
        if (_maxDistance > _distanceTraveled && !_shouldStopSpawning) {
            print("entre");
            _distanceTraveled +=  speed * Time.deltaTime;
            _distanceToSpawn += speed * Time.deltaTime;
            if (_distanceToSpawn > maxDistanceToSpawn) {
                print("spawneo");
                _distanceToSpawn = 0;
                Vector3 spawnPos = _startPosition + _direction * _distanceTraveled;
                GameObject p= Instantiate(particles, spawnPos, this.transform.rotation);
                p.gameObject.SetActive(true);
               Destroy(p.gameObject, timeAlive);

            }
        }
	}

    public void Stop() {
        _shouldStopSpawning = true;
    }
}
