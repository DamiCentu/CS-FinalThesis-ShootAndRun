using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour {

    public PlayerPowerUpManager powerUpManager;
    public Player player;
    // Use this for initialization
    void Start()
    {
        powerUpManager.ExtraDash();
        powerUpManager.ExtraDash();
        powerUpManager.ExtraDash();
        powerUpManager.AddRange();
        powerUpManager.AddRange();
        powerUpManager.EnableShield(new object[1]);
        powerUpManager.RecalculatePowerUp();
        Player p = FindObjectOfType<Player>();
        object[] container = new object[1];
        container[0] = PrimaryWeaponManager.instance.GetNextPowerUp(p.primaryGun);
        EventManager.instance.ExecuteEvent("UpgradeWeapon", container);
       // player.debugMode = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
