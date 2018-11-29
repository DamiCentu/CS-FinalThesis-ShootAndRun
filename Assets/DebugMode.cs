using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour {

    public PlayerPowerUpManager powerUpManager;
    public Player player;
    // Use this for initialization
    void Start()
    {
        if (Configuration.instance.activeDebugMode)
            Upgrade();
        if (Configuration.instance.playerInmortal)
            Inmortal();

    }

    private void Inmortal()
    {
        player.debugMode = true;
    }

    private void Upgrade()
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
    }



    // Update is called once per frame
    void Update()
    {

    }
}
