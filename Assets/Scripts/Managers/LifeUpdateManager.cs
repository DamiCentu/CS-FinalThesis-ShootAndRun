using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUpdateManager : MonoBehaviour {
    public int maxLifes = 5; 

    int _currentLifes;
    int _currentCredits; 

    public static LifeUpdateManager instance { get; private set; }

    void Awake() {
        Debug.Assert(FindObjectsOfType<LifeUpdateManager>().Length == 1);
        if (instance == null)
            instance = this;
    }

	void Start () {
        _currentCredits = Configuration.instance.GetCreditsAmount(); 
        _currentLifes = maxLifes;
        if (Configuration.instance.mode ==Configuration.Mode.RogueLike) _currentLifes = 1;
        EventManager.instance.SubscribeEvent(Constants.PLAYER_DEAD, OnPlayerDead); 
        StartCoroutine(UpdateRoutine());
    }

    IEnumerator UpdateRoutine() {
        yield return new WaitForSeconds(0.5f);
        EventManager.instance.ExecuteEvent(Constants.UI_UPDATE_PLAYER_LIFE, new object[] { _currentLifes });
    }

    void OnPlayerDead(object[] parameterContainer) {
        _currentLifes--;

        if (_currentLifes <= 0 ) {
            _currentCredits--;
            if (_currentCredits <= 0) {
                EventManager.instance.ExecuteEvent(Constants.GAME_OVER); 
            }
            else {
                EventManager.instance.ExecuteEvent(Constants.CREDIT_LOSED, new object[] { _currentCredits });
                _currentLifes = maxLifes;
                EventManager.instance.ExecuteEvent(Constants.UI_UPDATE_PLAYER_LIFE, new object[] { _currentLifes });
            }
        }
        else { 
            EventManager.instance.ExecuteEvent(Constants.UI_UPDATE_PLAYER_LIFE, new object[] { _currentLifes });
        }
    } 
}
