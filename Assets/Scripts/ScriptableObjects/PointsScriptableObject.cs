using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PointsValues", menuName = "PointValue")]
public class PointsScriptableObject : ScriptableObject {

    [Header("EnemyPoints")]
    public int normalPoints = 100;
    public int chargerPoints = 250;
    public int turretPoints = 300;
    public int powerUpChaserPoints = 200;
    public int minibossPointsPoints = 600;
    public int cubeEnemyPoints = 400;
    public int misilEnemyPoints = 300;
    public int fireEnemyPoints = 450;
    public int bossEnemyPoints = 100000;

    [Header("OtherPoints")]
    public int noDieInSectionPoints = 10000;
    public int pickPowerUp = 150;

    [Header("MultipliersThings")]
    public int baseEnemiesInRowComboToMultiply = 5;
    public float baseAcumulativeMultiplier = 1.0f; //la base de multiplicador (si se puede multiplicar sino sera 1)
    public float acumulativeAmountMultiplier = 0.5f; // lo que se le sumara por vez que se supere el numero de combo
    public float timeToResetCombo = 1.5f;
}
