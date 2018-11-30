using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactivateSomethingTrigger : MonoBehaviour {

    public GameObject[] gameobjectsToReactivate;
    public WallThatDestroysWhenPlayerKillsEnemiesBehaviour [] wallsThatDestroy;

    private void OnTriggerEnter(Collider c) {
        if(c.gameObject.layer == 8) { // player
            foreach (var go in gameobjectsToReactivate) { 
                go.SetActive(true);
            }

            EventManager.instance.ExecuteEvent(Constants.DESTROY_ENEMIES_BEHIND_WALL);

            foreach (var w in wallsThatDestroy) { 
                w.Show(true);
            }
        }
    }
}
