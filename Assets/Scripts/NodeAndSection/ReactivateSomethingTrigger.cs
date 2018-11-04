using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactivateSomethingTrigger : MonoBehaviour {

    public GameObject[] gameobjectsToReactivate;

    private void OnTriggerEnter(Collider c) {
        if(c.gameObject.layer == 8) { // player
            foreach (var go in gameobjectsToReactivate) { 
                go.SetActive(true);
            }
        }
    }
}
