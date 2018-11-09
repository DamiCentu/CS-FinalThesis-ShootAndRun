using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPosition : MonoBehaviour {
    public float x;
    public float y;
    public float z;
    public bool fancyBar = false;

	// Use this for initialization
	void Awake () {
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.localPosition = new Vector3(x,y,z);
        
        
        if (fancyBar) {
            this.transform.Translate(new Vector3(0, -23, 0));
        }
		
	}
}
