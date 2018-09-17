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

        EventManager.instance.SubscribeEvent(Constants.PLAYER_DEAD, OnPlayerDead); 
        StartCoroutine(UpdateRoutine());
    }

    IEnumerator UpdateRoutine() {
        yield return new WaitForSeconds(0.5f);
        EventManager.instance.ExecuteEvent(Constants.UPDATE_PLAYER_LIFE, new object[] { _currentLifes , _currentCredits });
    }

    void OnPlayerDead(object[] parameterContainer) {
        _currentLifes--;

        if (_currentLifes <= 0 ) {
            _currentCredits--;
            if (_currentCredits <= 0) {
                EventManager.instance.ExecuteEvent(Constants.GAME_OVER); 
            }
            else {
                _currentLifes = maxLifes;
                EventManager.instance.ExecuteEvent(Constants.UPDATE_PLAYER_LIFE, new object[] { _currentLifes, _currentCredits });
            }
        }
        else { 
            EventManager.instance.ExecuteEvent(Constants.UPDATE_PLAYER_LIFE, new object[] { _currentLifes , _currentCredits });
        }
    } 
}
