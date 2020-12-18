using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDoubleShoot : IPowerUp {
    public IShootable powerUp;
    

    void OnTriggerEnter(Collider other)
    {
        EnemiesManager.instance.player.GetComponent<Player>().powerUpManager.RecalculatePowerUp();
        ///
        Player p = other.GetComponent<Player>();
        if (p != null) {
            object[] container = new object[1];
            container[0] = PrimaryWeaponManager.instance.GetNextPowerUp(p.primaryGun); 
            EventManager.instance.ExecuteEvent("UpgradeWeapon", container);
            //Destroy(this.gameObject);
        }
    }
}
