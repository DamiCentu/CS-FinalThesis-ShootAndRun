using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
     Vector3 initialPosition;
    public float speed;
    float timer;
    public float width;
    public enum Direction {Right,Forward,Up};
    public Direction direction;
    private bool _shouldMove=true;

    // Use this for initialization
    void Start () {
        timer = 0;
        initialPosition = this.transform.position;
	}

	
	// Update is called once per frame
	void Update () {
        if (_shouldMove) {
            print("me muevo!");
            timer += Time.deltaTime;
            Vector3 dir=Vector3.zero;
            if (direction == Direction.Right) dir = this.transform.right;
            else if (direction == Direction.Forward) dir = this.transform.forward;
            else if (direction==Direction.Up) dir = this.transform.up;

            this.transform.position= initialPosition+ dir *Mathf.Sin(timer* speed)*width;
        }
	}

    internal void Move(bool v)
    {
        _shouldMove = v;
    }
}
