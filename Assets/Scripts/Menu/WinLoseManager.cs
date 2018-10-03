using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseManager : MonoBehaviour {
    System.Array values;

    void Start () {
        values = System.Enum.GetValues(typeof(KeyCode));
    }
	 
	void Update () { 
        foreach (KeyCode code in values) {
            if (Input.GetKeyDown(code)) {
                //print(System.Enum.GetName(typeof(KeyCode), code));
                SceneManager.LoadScene("Menu");
            }
        }
    }
}
