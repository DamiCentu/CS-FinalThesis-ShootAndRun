using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerToDestroyEnemiesWhenWallCloses : MonoBehaviour {
    public LayerMask maskToDestroy;

	void Start () {
        EventManager.instance.SubscribeEvent(Constants.DESTROY_ENEMIES_BEHIND_WALL, OnDestroyEnemiesBehindWall);
	}

    private void OnDestroyEnemiesBehindWall(object[] parameterContainer) {
        foreach (var c in Physics.OverlapBox(transform.position, new Vector3(transform.localScale.x / 2, transform.localScale.y / 2, transform.localScale.z / 2), transform.rotation, maskToDestroy)) {
            var i = c.gameObject.GetComponent<ICustomOnHit>();
            var p = c.gameObject.GetComponent<IPowerUp>();
            if (i != null)
                i.CustomOnHit();
            else if(p != null) {
                Destroy(p.gameObject);
            }
        }
    }
}
