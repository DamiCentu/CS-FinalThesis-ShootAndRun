using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpExtraDash : IPowerUp {

    void OnTriggerEnter(Collider other)
    {
        Player p = other.GetComponent<Player>();
        if (p != null)
        {
            p.powerUpManager.ExtraDash();
            //Destroy(this.gameObject);
        }
    }
}
