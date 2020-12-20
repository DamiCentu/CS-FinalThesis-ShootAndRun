using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUpManager : MonoBehaviour {
    public Player player;
    internal int PowerUpRangeNumber = 0;
    internal int PowerUpDoubleShootNumber = 0;
    internal int PowerUpExtraDashNUmber = 0;
    internal int PowerUpShieldNumber = 0;
    public IShootable DoubleShoot;
  //  public GameObject prefabShoot;
  //  public GameObject prefabRange;
  //  public GameObject prefabDash;

    public void Start()
    {
        EventManager.instance.SubscribeEvent("UpgradeWeapon", UpgradeShoot);
        EventManager.instance.SubscribeEvent("PrimaryWeaponMoreRange", UpdateRange);
        EventManager.instance.SubscribeEvent(Constants.SOUL_RECOVER, SoulRecover);
        if (Configuration.instance.dificulty == Configuration.Dificulty.Easy) {
            PowerUpDoubleShootNumber++;
            ChangePrimaryWeapon(DoubleShoot);
            player.MaxDashCount++;
            PowerUpExtraDashNUmber++;
            player.RefreshDashUI();

        }
        RecalculatePowerUp();
    }

    private void SoulRecover(object[] parameterContainer)
    {
        EventManager.instance.ExecuteEvent(Constants.ACHIVEMENT_POWER_UP_RECOVER, new object[] { });
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
        RecalculatePowerUp();
    }
    private void ChangePrimaryWeapon(IShootable weapon)
    {
        player.primaryGun = weapon;
    }

    public void UpgradeShoot(object[] parameterContainer)
    {
        if (PowerUpDoubleShootNumber == 0) {
            SoundManager.instance.PlayDoubleShoot();
        }
        else 
        {
            SoundManager.instance.PlayQuadraShoot();
        }
        EventManager.instance.ExecuteEvent(Constants.ACHIVEMENT_UPGRADE_WEAPON, new object[] { });
        InfoManager.instance.Info("Double Shoot");
        PowerUpDoubleShootNumber++;
        ChangePrimaryWeapon((IShootable)parameterContainer[0]);

    }

    private void DowngradeShoot()
    {
        PowerUpDoubleShootNumber--;
        ChangePrimaryWeapon(PrimaryWeaponManager.instance.GetPreviousPowerUp(player.primaryGun));
    }

    public void UpdateRange(object[] parameterContainer)
    {

        if ((int)parameterContainer[0]>0) {
            SoundManager.instance.PlayHigherRange();
        }
        PowerUpRangeNumber += (int)parameterContainer[0];
    }

    private void ReduceRange()
    {
        PowerUpRangeNumber--;
        object[] conteiner = new object[1];
        conteiner[0] = -1;
        EventManager.instance.ExecuteEvent("PrimaryWeaponMoreRange", conteiner);
    }

    public void AddRange()
    {
        EventManager.instance.ExecuteEvent(Constants.ACHIVEMENT_MORE_RANGE, new object[] { });
        SoundManager.instance.PlayHigherRange();
        InfoManager.instance.Info("Higher Range");
        PowerUpRangeNumber++;
        object[] conteiner = new object[1];
        conteiner[0] = 1;
        EventManager.instance.ExecuteEvent("PrimaryWeaponMoreRange", conteiner);
    }

    
    public void EnableShield(object[] parameterContainer)
    {
        EventManager.instance.ExecuteEvent(Constants.ACHIVEMENT_SHIELD, new object[] { });
        SoundManager.instance.PlayShield();
        InfoManager.instance.Info("Shield");
        player._gotShield = true;
        player.shield.gameObject.SetActive(true);
        //print("active escudo");
        PowerUpShieldNumber++;
    }

    public void DisableShield()
    {
        EventManager.instance.ExecuteEvent(Constants.ACHIVEMENT_CLOSE_DEATH, new object[] { });
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

    public void ExtraDash()
    {
        EventManager.instance.ExecuteEvent(Constants.ACHIVEMENT_EXTRA_DASH, new object[] { });
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
        GameObject prefabPowerUp;
        int countDoubleShoot = PrimaryWeaponManager.instance.Index(player.primaryGun);

        if (PowerUpExtraDashNUmber > 0)
        {
            RemoveExtraDash();
            p = PowerUp.ExtraDash;
            prefabPowerUp = LootTableManager.instance.extraDash.gameObject;
        }
        else if (PowerUpRangeNumber > 0)
        {
            ReduceRange();
            p = PowerUp.ExtraRange;
            prefabPowerUp = LootTableManager.instance.range.gameObject;
        }
        else if (countDoubleShoot > 0)
        {
            DowngradeShoot();
            p = PowerUp.DoubleShoot;
            prefabPowerUp = LootTableManager.instance.doubleShoot.gameObject;
        }
        else
        {
            return;
        }
        // borro los souls anteriores
        var foundObjects = FindObjectsOfType<IPowerUp>();
        foreach (var power in foundObjects)
        {
            if (!power.shouldbeErased) {
                power.shouldbeErased = true;
          //      power.gameObject.SetActive(false);
            }
        }

        LootTableManager.instance.DestroyAllPowerUps();
        /*        if (foundObject != null)
                {
                    foundObject.transform.position = transform.position;
                    foundObject.Set(p);
                }
                else
                {

                    Soul s = Instantiate(player.soulPrefab, transform.position, transform.rotation).GetComponent<Soul>();
                    s.Set(p);
                }*/

        // creo el nuevo
       GameObject go= Instantiate(prefabPowerUp, transform.position, Quaternion.identity);
        go.GetComponent<IPowerUp>().shouldbeErased = false;
        LootTableManager.instance.AddDropedPowerUp(go);
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
