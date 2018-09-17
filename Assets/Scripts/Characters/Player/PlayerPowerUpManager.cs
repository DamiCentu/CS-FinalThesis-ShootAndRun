using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUpManager : MonoBehaviour {
    public Player player;
    internal int PowerUpRangeNumber = 0;
    internal int PowerUpDoubleShootNumber = 0;
    internal int PowerUpExtraDashNUmber = 0;
    internal int PowerUpShieldNumber = 0;

    public void Start()
    {
        EventManager.instance.SubscribeEvent("UpgradeWeapon", UpgradeShoot);
        EventManager.instance.SubscribeEvent("PrimaryWeaponMoreRange", UpdateRange);
        EventManager.instance.SubscribeEvent(Constants.SOUL_RECOVER, SoulRecover);
    }

    private void SoulRecover(object[] parameterContainer)
    {
        PowerUp p = (PowerUp)parameterContainer[0];
        switch (p)
        {
            case PowerUp.DoubleShoot:
                object[] container = new object[1];
                container[0] = PrimaryWeaponManager.instance.GetNextPowerUp(player.primaryGun);
                UpgradeShoot(container);
                break;
            case PowerUp.ExtraDash:
                ExtraDash();
                break;
            case PowerUp.ExtraLife:
                break;
            case PowerUp.ExtraRange:
                AddRange();
                break;
        }
    }
    private void ChangePrimaryWeapon(IShootable weapon)
    {
        player.primaryGun = weapon;
    }

    private void UpgradeShoot(object[] parameterContainer)
    {
        SoundManager.instance.PlayDoubleShoot();
        InfoManager.instance.Info("Double Shoot");
        PowerUpDoubleShootNumber++;
        ChangePrimaryWeapon((IShootable)parameterContainer[0]);

    }

    private void DowngradeShoot()
    {
        PowerUpDoubleShootNumber--;
        ChangePrimaryWeapon(PrimaryWeaponManager.instance.GetPreviousPowerUp(player.primaryGun));
    }

    private void UpdateRange(object[] parameterContainer)
    {
        PowerUpRangeNumber += (int)parameterContainer[0];
    }

    private void ReduceRange()
    {
        PowerUpRangeNumber--;
        object[] conteiner = new object[1];
        conteiner[0] = -1;
        EventManager.instance.ExecuteEvent("PrimaryWeaponMoreRange", conteiner);
    }

    private void AddRange()
    {
        SoundManager.instance.PlayHigherRange();
        InfoManager.instance.Info("Higher Range");
        PowerUpRangeNumber++;
        object[] conteiner = new object[1];
        conteiner[0] = 1;
        EventManager.instance.ExecuteEvent("PrimaryWeaponMoreRange", conteiner);
    }

    
    public void EnableShield(object[] parameterContainer)
    {
        SoundManager.instance.PlayShield();
        InfoManager.instance.Info("Shield");
        player._gotShield = true;
        player.shield.gameObject.SetActive(true);
        PowerUpShieldNumber++;
    }

    public void DisableShield()
    {
        PowerUpShieldNumber--;
        player._gotShield = false;
        player.shield.gameObject.SetActive(false);

        player._isInvulnerable = true;

        Timer timer = new Timer(player.InvulnerableExtraTime, () =>
        {
            player._isInvulnerable = false;
        });
        player.timers.Add(timer);
        //print("se me fue, estoy inmunerable");
    }

    internal void ExtraDash()
    {
        SoundManager.instance.PlayExtraDash();
        InfoManager.instance.Info("Extra Dash");
        player.MaxDashCount++;
        PowerUpExtraDashNUmber++;
        player.RefreshDashUI();


    }

    internal void RemoveExtraDash()
    {
        player.MaxDashCount--;
        PowerUpExtraDashNUmber--;
        player.RefreshDashUI();
    }

    internal void CreateSoul()
    {
        RecalculatePowerUp();
        PowerUp p;
        int countDoubleShoot = PrimaryWeaponManager.instance.Index(player.primaryGun);

        if (PowerUpExtraDashNUmber > 0)
        {
            RemoveExtraDash();
            p = PowerUp.ExtraDash;
        }
        else if (PowerUpRangeNumber > 0)
        {
            ReduceRange();
            p = PowerUp.ExtraRange;
        }
        else if (countDoubleShoot > 0)
        {
            DowngradeShoot();
            p = PowerUp.DoubleShoot;
        }
        else
        {
            return;
        }
        // borro los souls anteriores
        var foundObject = FindObjectOfType<Soul>();
        if (foundObject != null)
        {
            foundObject.transform.position = transform.position;
            foundObject.Set(p);
        }
        else
        {

            Soul s = Instantiate(player.soulPrefab, transform.position, transform.rotation).GetComponent<Soul>();
            s.Set(p);
        }

        // creo el nuevo
    }

    public void RecalculatePowerUp() {
        //print("asd");
        object[] conteiner = new object[4];
        conteiner[0]= PowerUpRangeNumber ;
        conteiner[1]= PowerUpDoubleShootNumber ;
        conteiner[2] = PowerUpExtraDashNUmber;
        conteiner[3] = PowerUpShieldNumber;
        EventManager.instance.ExecuteEvent(Constants.QUANTITY_POWERUPS, conteiner);
        
    }

}
