using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour , IPauseable {
    Vector3 startPosition;
    public float maxDistance = 200;
    public float speed = 20;
    float _distanceTraveled=0;
    public float offset = 5;
    public float angle=20;
    private Quaternion startRotation;

    bool _paused;
    public void OnPauseChange(bool v)
    {
        _paused = v;
    }

    void Start () {
        startPosition = this.transform.position;
        startRotation = this.transform.rotation;
        Set();
    }

    private void Set()
    {
        this.transform.position= startPosition+ this.transform.right * UnityEngine.Random.Range(0,1)* offset;
        this.transform.rotation = startRotation;
        this.transform.Rotate(this.transform.right * UnityEngine.Random.Range(0, 1) * angle);
    }

    // Update is called once per frame
    void Update () {
        if (_paused)
            return;

        _distanceTraveled += speed * Time.deltaTime;
        if (_distanceTraveled > maxDistance) {
            _distanceTraveled = 0;
        }

        this.transform.position = this.startPosition + _distanceTraveled * this.transform.forward;
	}
}
