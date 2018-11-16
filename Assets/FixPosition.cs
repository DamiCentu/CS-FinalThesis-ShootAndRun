using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPosition : MonoBehaviour , IPauseable {
    public float x;
    public float y;
    public float z;
    public bool fancyBar = false;

    bool _paused;
    public void OnPauseChange(bool v)
    {
        _paused = v;
    }

    // Use this for initialization
    void Awake () {
	}
	
	// Update is called once per frame
	void Update () {
        if (_paused)
            return;

        this.transform.localPosition = new Vector3(x,y,z);
        
        
        if (fancyBar) {
            this.transform.Translate(new Vector3(0, -23, 0));
        }
		
	}
}
