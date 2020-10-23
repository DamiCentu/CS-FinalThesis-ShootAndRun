using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoguelikePowerupManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void GetPowerUp() {
        FindObjectOfType<Player>().RogueGetRandomPowerUP();
    }
}
