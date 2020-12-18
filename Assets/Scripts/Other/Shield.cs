using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : IPowerUp {

    void OnTriggerEnter(Collider other)
    {
        EnemiesManager.instance.player.GetComponent<Player>().powerUpManager.RecalculatePowerUp();
        Player p = other.GetComponent<Player>();
        if (p != null)
        {
            EventManager.instance.ExecuteEvent("GetShield");
            //Destroy(this.gameObject);
        }
    }
}
