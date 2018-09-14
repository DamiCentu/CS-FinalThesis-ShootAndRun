using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour {
    PowerUp powerUp;

    void OnTriggerEnter(Collider other)
    {
        Player p = other.GetComponent<Player>();
        if (p != null && !p.isDead)
        {
            object[] container = new object[1];
            container[0] = powerUp;
            EventManager.instance.ExecuteEvent(Constants.SOUL_RECOVER, container);
            Destroy(this.gameObject);

        }
    }

    internal void Set(PowerUp p)
    {
        powerUp = p;
    }
}
