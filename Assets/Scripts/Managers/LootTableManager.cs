using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTableManager : MonoBehaviour {
    
    public PowerUpDoubleShoot doubleShoot;
    public PowerUpExtraDash extraDash;
    public PowerUpMoreRange range;
    public Shield shield;
    public List<int> cantidades;
    public List<GameObject> powerUps;
    private float probability=0.2f;
    public float easyDefaultProbability = 0.15f;
    public float mediumDefaultProbability = 0.09f;
    public float hardDefaultProbability = 0.05f;
    public float defaultProbability;
    private int totalPowerUps;
    private int totalPowerAvailable;
    List<GameObject> _allGamePowerUps;
    public static LootTableManager instance { get; private set; }

    void Awake() {
        Debug.Assert(FindObjectsOfType<LootTableManager>().Length == 1);
        if (instance == null)
            instance = this; 
    } 

    public void Start()
    {
        SetDefaultProbability();

        SubscribeEvents();
        ConfigurePowerUps();
        probability = 1;


        if (Configuration.instance.lvl == 2)
        {
            probability = defaultProbability;
        }
    }

    private void ConfigurePowerUps()
    {
        _allGamePowerUps = new List<GameObject>();
        powerUps.Add(range.gameObject);
        powerUps.Add(doubleShoot.gameObject);
        powerUps.Add(extraDash.gameObject);
        powerUps.Add(shield.gameObject);
        foreach (var item in cantidades)
        {
            totalPowerUps += item;
        }
        totalPowerAvailable = totalPowerUps;
    }

    private void SubscribeEvents()
    {
        EventManager.instance.SubscribeEvent(Constants.QUANTITY_POWERUPS, UpdatePowerUpQuantity);
        EventManager.instance.AddEvent(Constants.POWER_UP_PICKED);
        EventManager.instance.AddEvent(Constants.POWER_UP_DROPED);
        EventManager.instance.SubscribeEvent(Constants.ENEMY_DEAD, ShouldDropPowerUp);
        EventManager.instance.SubscribeEvent(Constants.POWER_UP_PICKED, OnPowerUpPicked);
    }

    private void SetDefaultProbability()
    {
        if (Configuration.instance.dificulty == Configuration.Dificulty.Easy)
        {
            defaultProbability = easyDefaultProbability;
        }
        else if (Configuration.instance.dificulty == Configuration.Dificulty.Medium)
        {
            defaultProbability = mediumDefaultProbability;
        }
        else if (Configuration.instance.dificulty == Configuration.Dificulty.Hard)
        {
            defaultProbability = hardDefaultProbability;
        }
    }

    private void UpdatePowerUpQuantity(object[] parameterContainer)
    {
        cantidades[0] = 3 - (int)parameterContainer[0];
        cantidades[1] = 2 - (int)parameterContainer[1];
        cantidades[2] = 4 - (int)parameterContainer[2];
        cantidades[3] = 1 - (int)parameterContainer[3];
        totalPowerAvailable = cantidades[0] + cantidades[1] + cantidades[2] + cantidades[3];
        //print("powerUPsAvailable :" + totalPowerAvailable + cantidades[0] + cantidades[1] + cantidades[2] + cantidades[3]);
    }

    private void ShouldDropPowerUp(object[] parameterContainer) {
        if ((bool)parameterContainer[3] == true || parameterContainer.Length > 5 && (bool)parameterContainer[5] == false)//esto es para saber si deberia la muerte spawnear o no(pasaba que el verde agarraba el power up y es una muerte en teoria pero no esta mueriendo, no deberia dropear) el 3 es del bicho verde que te dice si exploto o se chupo el power up
            return;

        AbstractEnemy a = (AbstractEnemy)parameterContainer[2];

        Vector3 position = a.transform.position;
        if (0 != totalPowerAvailable) { //si no agarre todos lso power ups
            float random = UnityEngine.Random.Range(0f, 1f);
            if (random <= probability)
            {
                DropPowerUp(position,true,a);
            }
        }
    }

    internal void DropPowerUp(Vector3 position, bool withChaser, AbstractEnemy a=null)
    {
        int typeOfPowerup = TypeOfRandomPowerup();
        //print("typeOfPowerup" + typeOfPowerup);
        if (typeOfPowerup != -1)
        {
            GameObject go = InstantiatePowerUp(position, typeOfPowerup);
            if (withChaser)
            {
                EventManager.instance.ExecuteEvent(Constants.POWER_UP_DROPED, new object[] { a.GetCurrentSectionNode, go });
            }
        }
    }

    public int TypeOfRandomPowerup()
    {
        float random = UnityEngine.Random.Range(0f, 1f);
        float lastProb = 0;
        for (int i = 0; i < cantidades.Count; i++)
        {
            float value = lastProb + ((float)cantidades[i] / totalPowerUps);
            if (random <= value)
            {
                return i;
            }
            lastProb = value;
        }

        return -1;
    }

    public GameObject InstantiatePowerUp(Vector3 position, int i)
    {
        var go = Instantiate(powerUps[i], position, this.transform.rotation);
        go.SetActive(true);
        cantidades[i]--;
        totalPowerAvailable--;
        _allGamePowerUps.Add(go);
        return go;
    }

    void OnPowerUpPicked(params object[] param) {
        var go = (GameObject)param[0];
        if (_allGamePowerUps.Contains(go)) { 
            _allGamePowerUps.Remove(go);
            Destroy(go);
            if (TutorialBehaviour.instance!=null && TutorialBehaviour.instance.IsTutorialNode) {
                EventManager.instance.ExecuteEvent(Constants.UI_TUTORIAL_CHANGE, UIManager.TUTORIAL_SHOOT_SPECIAL);
            }
        }
        else throw new Exception("hay 1 power up que no esta en la lista, si hay un error es porque el power up se esta eliminando en su trigger ENTERRR, borrar esa linea de codigo");
    }

    public void DestroyAllPowerUps() {
        Utility.DestroyAllInAndClearList(_allGamePowerUps);
    }

    public bool ExistAPowerUp { get { return _allGamePowerUps.Count > 0; } }

    public GameObject ClosestPowerUp(Vector3 pos) { 

        GameObject go = null;
        float returnGODistance = float.MaxValue;
        foreach (var gObj in _allGamePowerUps) { 
            var distanceFromActualGameObject = Vector3.Distance(gObj.transform.position, pos);
            
            if (distanceFromActualGameObject < returnGODistance) {
                go = gObj;
                returnGODistance = distanceFromActualGameObject;
            }
        }
        if (go == null)
            return null;

        return go;
    }
    public void SetDefaultProbavility() {
        probability = defaultProbability;
    }
    public void SetTutoProbavility()
    {
        probability = 1;
    }
}
