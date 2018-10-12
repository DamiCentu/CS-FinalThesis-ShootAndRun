using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPosition : MonoBehaviour {
    public float x;
    public float y;
    public float z;

	// Use this for initialization
	void Awake () {
        this.transform.localPosition= new Vector3(x,y,z);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
