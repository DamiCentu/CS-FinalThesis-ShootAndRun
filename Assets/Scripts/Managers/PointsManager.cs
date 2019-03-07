using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour {

    public PointsScriptableObject pointsSO;

    int _currentPoints = 0;
    int _currentPointsInSection = 0;

    int _enemiesInRowComboToMultiply = 0; // esto es la cantidad de enemigos que hay que matar para que se pueda usar el multiplicador
    int _enemyInRowCount = 0; //estos son los enemigos que voy matando a menos que se me pase el tiempo

    float _currentMultiplier = 0; // esto es el multiplicador que se va a devolver si se puede multiplicar
    float _timerResetCombo = 0;

    bool _playerDied = false;
    bool _canDispatchEvent = false;

    SectionNode _currentNode;

    public int CurrentPoints { get { return _currentPoints; } }

    void Start ()
    {
        EventManager.instance.SubscribeEvent(Constants.PLAYER_DEAD, OnPlayerDead);
        EventManager.instance.SubscribeEvent(Constants.ENEMY_DEAD, OnEnemyDead);
        EventManager.instance.SubscribeEvent(Constants.START_SECTION, OnStartOrEndSection);
        EventManager.instance.SubscribeEvent(Constants.POWER_UP_PICKED, OnPowerUpPicked);
        EventManager.instance.SubscribeEvent(Constants.BOSS_DESTROYED, OnBossDestroyed);

        _enemiesInRowComboToMultiply = pointsSO.baseEnemiesInRowComboToMultiply;
        _currentMultiplier = pointsSO.baseAcumulativeMultiplier;
    }

    private void OnBossDestroyed(object[] parameterContainer)
    {
        _currentPoints += pointsSO.bossEnemyPoints;
        _currentPointsInSection += pointsSO.bossEnemyPoints;
        EventManager.instance.ExecuteEvent(Constants.UI_POINTS_UPDATE, new object[] { _currentPoints, _currentMultiplier });
    }

    private void OnPowerUpPicked(object[] param)
    {
        if((string)param[1] == "isPlayer")
        {
            _currentPoints += pointsSO.pickPowerUp;
            _currentPointsInSection += pointsSO.pickPowerUp;
            EventManager.instance.ExecuteEvent(Constants.UI_POINTS_UPDATE, new object[] { _currentPoints, _currentMultiplier });
            EventManager.instance.ExecuteEvent(Constants.UI_NOTIFICATION_TEXT_UPDATE, new object[] { "power up picked! +" + pointsSO.pickPowerUp.ToString() });
        }
    }

    private void OnStartOrEndSection(object[] param)
    {
        var node = (SectionNode)param[1];
        if ((string)param[0] == "in")
        {
            _currentPointsInSection = 0;
            if(_currentNode != node)
            {
                _playerDied = false;
                _currentNode = node;
            }

        }
        if ((string)param[0] == "out")
        {
            if (!_playerDied)
            {
                _currentPoints += pointsSO.noDieInSectionPoints;
                _currentPointsInSection += pointsSO.noDieInSectionPoints;
                EventManager.instance.ExecuteEvent(Constants.UI_POINTS_UPDATE, new object[] { _currentPoints, _currentMultiplier });
                EventManager.instance.ExecuteEvent(Constants.UI_NOTIFICATION_TEXT_UPDATE, new object[] { "No death section! +" + pointsSO.noDieInSectionPoints.ToString() });
            }
        }
    }

    private void OnPlayerDead(object[] parameterContainer)
    {
        _playerDied = true;
        _currentPoints -= _currentPointsInSection;
        _currentMultiplier = pointsSO.baseAcumulativeMultiplier;
        EventManager.instance.ExecuteEvent(Constants.UI_POINTS_UPDATE, new object[] { _currentPoints, _currentMultiplier });
        EventManager.instance.ExecuteEvent(Constants.UI_NOTIFICATION_TEXT_UPDATE, new object[] { "Points in section lost! -" + _currentPointsInSection });
    }

    void Update ()
    {
		if(_timerResetCombo < pointsSO.timeToResetCombo)
        {
            _timerResetCombo += Time.deltaTime;
            _canDispatchEvent = true;
        }
        else
        {
            _enemiesInRowComboToMultiply = pointsSO.baseEnemiesInRowComboToMultiply;
            _enemyInRowCount = 0;
            _currentMultiplier = pointsSO.baseAcumulativeMultiplier;

            if(_canDispatchEvent)
            {
                _canDispatchEvent = false;
                EventManager.instance.ExecuteEvent(Constants.UI_CLEAR_MULTIPLIER);
            }
        }
	}

    void OnEnemyDead(params object[] param)
    {
        if ((SectionNode)param[1] == SectionManager.instance.actualNode)
        {
            _enemyInRowCount++;
            _timerResetCombo = 0;

            UpdateCurrentMultiplier();

            var pointsToSum = Mathf.RoundToInt(GetEnemyPoints((AbstractEnemy)param[2]) * _currentMultiplier);

            _currentPoints += pointsToSum;
            _currentPointsInSection += pointsToSum;

            EventManager.instance.ExecuteEvent(Constants.UI_POINTS_UPDATE, new object[] { _currentPoints, _currentMultiplier });
        }
    }

    void UpdateCurrentMultiplier()
    {
        if (_enemyInRowCount < pointsSO.baseEnemiesInRowComboToMultiply)
        {
            _currentMultiplier = pointsSO.baseAcumulativeMultiplier;
        }
        else if (_enemyInRowCount >= pointsSO.baseEnemiesInRowComboToMultiply && _enemyInRowCount < pointsSO.baseEnemiesInRowComboToMultiply + pointsSO.baseEnemiesInRowComboToMultiply)
        {
            _currentMultiplier = pointsSO.baseAcumulativeMultiplier + pointsSO.acumulativeAmountMultiplier;
        }
        else if (_enemyInRowCount >= _enemiesInRowComboToMultiply)
        {
            if(_enemyInRowCount >= _enemiesInRowComboToMultiply + pointsSO.baseEnemiesInRowComboToMultiply)
            {
                _enemiesInRowComboToMultiply += pointsSO.baseEnemiesInRowComboToMultiply;
                _currentMultiplier += pointsSO.acumulativeAmountMultiplier;
            }
        }
    }

    int GetEnemyPoints(AbstractEnemy enemy)
    {
        if (enemy is NormalEnemyBehaviour)
        {
            return pointsSO.normalPoints;
        }
        else if (enemy is ChargerEnemyBehaviour)
        {
            return pointsSO.chargerPoints;
        }
        else if (enemy is EnemyTurretBehaviour)
        {
            return pointsSO.turretPoints;
        }
        else if (enemy is PowerUpChaserEnemy)
        {
            return pointsSO.powerUpChaserPoints;
        }
        else if (enemy is MiniBossBehaviour)
        {
            return pointsSO.minibossPointsPoints;
        }
        else if (enemy is CubeEnemyBehaviour)
        {
            return pointsSO.cubeEnemyPoints;
        }
        else if (enemy is MisilEnemy)
        {
            return pointsSO.misilEnemyPoints;
        }
        else if (enemy is FireEnemy)
        {
            return pointsSO.fireEnemyPoints;
        }
        return 0;
    }
}
