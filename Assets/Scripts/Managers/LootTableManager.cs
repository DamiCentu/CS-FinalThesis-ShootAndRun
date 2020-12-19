using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootTableManager : MonoBehaviour {
    
    public PowerUpDoubleShoot doubleShoot;
    public PowerUpExtraDash extraDash;
    public PowerUpMoreRange range;
    public Shield shield;
    public List<int> availablePowerupDropQuantities = new List<int>();
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
    public LayerMask objectToDetectOnSpawnPowerUp;

    void Awake() {
        Debug.Assert(FindObjectsOfType<LootTableManager>().Length == 1);
        if (instance == null)
            instance = this; 
    } 

    public void Start()
    {
        SetDefaultProbability();

        SubscribeEvents();
        InitializePowerupAvailability();
        ConfigurePowerUps();
        probability = 1;

        if (Configuration.instance.lvl == 2)
        {
            probability = defaultProbability;
        }
    }

    public void AddDropedPowerUp(GameObject g) {
        _allGamePowerUps.Add(g);

    }

    private void ConfigurePowerUps()
    {
        _allGamePowerUps = new List<GameObject>();
        powerUps.Add(range.gameObject);
        powerUps.Add(doubleShoot.gameObject);
        powerUps.Add(extraDash.gameObject);
        powerUps.Add(shield.gameObject);
        print("ConfigurePowerUps," + availablePowerupDropQuantities.Count);
        foreach (var item in availablePowerupDropQuantities)
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

    private void OnPowerUpGain(object[] parameterContainer)
    {
        throw new NotImplementedException();
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
        InitializePowerupAvailability();
        availablePowerupDropQuantities[0] = 2 - (int)parameterContainer[0];
        availablePowerupDropQuantities[1] = 2 - (int)parameterContainer[1];
        availablePowerupDropQuantities[2] = 4 - (int)parameterContainer[2];
        availablePowerupDropQuantities[3] = 1 - (int)parameterContainer[3];
        print(String.Format("{0}, {1}, {2}, {3}",
            availablePowerupDropQuantities[0],
            availablePowerupDropQuantities[1],
            availablePowerupDropQuantities[2],
            availablePowerupDropQuantities[3]
        ));
        totalPowerAvailable = availablePowerupDropQuantities[0] + availablePowerupDropQuantities[1] + availablePowerupDropQuantities[2] + availablePowerupDropQuantities[3];
        //print("powerUPsAvailable :" + totalPowerAvailable + cantidades[0] + cantidades[1] + cantidades[2] + cantidades[3]);
    }

    private void InitializePowerupAvailability()
    {
        if (availablePowerupDropQuantities.Count < 4)
        {
            availablePowerupDropQuantities.Add(0);
            availablePowerupDropQuantities.Add(0);
            availablePowerupDropQuantities.Add(0);
            availablePowerupDropQuantities.Add(0);
        }
    }

    private void ShouldDropPowerUp(object[] parameterContainer) {//[3] son para saber si tiene que hacer particulas
        if ((bool)parameterContainer[3] == true || parameterContainer.Length > 6 && (bool)parameterContainer[6] == false) //esto es para saber si deberia la muerte spawnear o no(pasaba que el verde agarraba el power up y es una muerte en teoria pero no esta mueriendo, no deberia dropear) el 3 es del bicho verde que te dice si exploto o se chupo el power up
            return;

        AbstractEnemy a = (AbstractEnemy)parameterContainer[2];
        Vector3 position = a.transform.position;
        var dirToPlayer = EnemiesManager.instance.player.transform.position - position;

        // if primer tutorial
        // drop power up

        if (TutorialBehaviour.instance != null)
        {
            print("tutorial behaviour is online");
            if (!TutorialBehaviour.instance.isFirstEnemyKilled)
            {
                print("first enemy not killed");
                bool dropped = DropPowerUp(position, true, a);
                print("dropped? " + dropped.ToString());
                return;
            }
        }


        if( Physics.Raycast(position, dirToPlayer, dirToPlayer.magnitude, objectToDetectOnSpawnPowerUp)) {
            if( TutorialBehaviour.instance != null) {
                if(SectionManager.instance.actualNode != TutorialBehaviour.instance.tutorialNode) {
                    return;
                }
            }
            else {
                return;
            }
        }
        print("antes de imprimir");
        if (0 != totalPowerAvailable) { //si no agarre todos lso power ups
            print("es distinto de 0 total power Available");
            float random = UnityEngine.Random.Range(0f, 1f);
            if (random <= probability)
            {
                print("dropped? " + random+ " probability "+  probability);
                DropPowerUp(position,true,a);
            }
        }
        print(String.Format("{0}, {1}, {2}, {3}",
            availablePowerupDropQuantities[0],
            availablePowerupDropQuantities[1],
            availablePowerupDropQuantities[2],
            availablePowerupDropQuantities[3]
));
    }

    internal bool DropPowerUp(Vector3 position, bool withChaser, AbstractEnemy a=null)
    {
        int typeOfPowerup = TypeOfRandomPowerup();
        //print("typeOfPowerup" + typeOfPowerup);
        if (typeOfPowerup != -1)
        {
            GameObject go = InstantiatePowerUp(position, typeOfPowerup);
            if (withChaser)
            {
                EventManager.instance.ExecuteEvent(Constants.POWER_UP_DROPED, new object[] { a.GetCurrentSectionNode, go });
                return true;
            }
        }
        return false;
    }

    public int TypeOfRandomPowerup()
    {
        var availablePowerupsList = new List<int>();
        for (int i = 0; i < availablePowerupDropQuantities.Count; i++)
        {
            if (availablePowerupDropQuantities[i] <= 0) continue;
            for (int j = 0; j < availablePowerupDropQuantities[i]; j++)
            {
                availablePowerupsList.Add(i);
            }
        }

        if (availablePowerupsList.Count == 0)
        {
            return -1;
        }

        int index = (new System.Random()).Next(availablePowerupsList.Count);

        return availablePowerupsList[index];
    }

    public GameObject InstantiatePowerUp(Vector3 position, int i)
    {
        var go = Instantiate(powerUps[i], position, this.transform.rotation);
        go.SetActive(true);
        availablePowerupDropQuantities[i]--;
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

        var _allGamePowerUps2 = new List<GameObject>();
        foreach (var power in _allGamePowerUps)
        {

            if (power.GetComponent<IPowerUp>().shouldbeErased)
            {
                Destroy(power);
                //      power.gameObject.SetActive(false);
            } else
            {
                _allGamePowerUps2.Add(power);
            }
        }
        _allGamePowerUps = _allGamePowerUps2;

    //    Utility.DestroyAllInAndClearList(_allGamePowerUps);
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
    public void SetTutoProbability()
    {
        probability = 1;
    }

    public void SetRoguelikeProbability()
    {
        probability = 0;
    }
}
