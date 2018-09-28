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

    public void SpawnDirection(Vector3 spawnPos, Vector3 direction,float speed) {

        _direction = direction;

        particles = GameObject.Find(particlesName);
        if (particles == null) print("che es null!");
        set(spawnPos, speed);
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
