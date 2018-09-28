using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {
    Vector3 startPosition;
    public float maxDistance = 200;
    public float speed = 20;
    float _distanceTraveled=0;
	// Use this for initialization
	void Start () {
        startPosition = this.transform.position;

    }
	
	// Update is called once per frame
	void Update () {
        _distanceTraveled += speed * Time.deltaTime;
        if (_distanceTraveled > maxDistance) {
            _distanceTraveled = 0;
        }

        this.transform.position = this.startPosition + _distanceTraveled * this.transform.forward;
	}
}
