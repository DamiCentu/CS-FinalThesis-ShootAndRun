using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerBall : MonoBehaviour {
    public GameObject prefabBerserkerBall;
    public float radius;
    public float rotationSpeed; 

    BerserkerBall(int number) {
        for (int i = 0; i < number; i++)
        {
            float currentAngle = ((float)(i / number)) * 360; 
            //float shootPositionX = transform.position.x + (float)Math.Cos(currentAngle);
            //float shootPositionz = transform.position.z + (float)Math.Sin(currentAngle);
            //Vector3 offset = new Vector3(shootPositionX, transform.position.y, shootPositionz);

            Instantiate(prefabBerserkerBall);
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
