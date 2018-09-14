using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public SectionManager.WaveNumber waveOfSpawn;
    public EnemiesManager.TypeOfEnemy typeOfEnemy;
    public bool triggerSpawn = false;
    public Slider slider;

    float _timeToSpawn = 0f;
    float _timer = 0f;
    bool _spawning = false;

    public float radiusOfDebugSphere = 1f;
    public float sizeDebugCube = 1.5f; 
    public bool showGizmos = true;

    public void OnSpawn(float timeToSpawn) {
        slider.maxValue = timeToSpawn;
        _timeToSpawn = timeToSpawn;
        _spawning = true;
    }

    void Update() {
        if (_spawning) {
            _timer += Time.deltaTime; 
            slider.value = _timer;
            if (_timer >= _timeToSpawn) {
                _spawning = false;
                _timer = 0f;
                slider.value = 0f;
            }
        }
    }

    void OnDrawGizmos() {
        if (!showGizmos)
            return;

        if(triggerSpawn) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(sizeDebugCube, sizeDebugCube, sizeDebugCube));
            return;
        }

        if (waveOfSpawn == SectionManager.WaveNumber.First) { 
            Gizmos.color = Color.green; 
            Gizmos.DrawWireSphere(transform.position, radiusOfDebugSphere - .3f);
        }
        else if(waveOfSpawn == SectionManager.WaveNumber.Second) { 
            Gizmos.color = Color.yellow; 
            Gizmos.DrawWireSphere(transform.position, radiusOfDebugSphere);
        } 
        else if(waveOfSpawn == SectionManager.WaveNumber.Third) { 
            Gizmos.color = Color.red; 
            Gizmos.DrawWireSphere(transform.position, radiusOfDebugSphere + .3f);
        }
    }
}
