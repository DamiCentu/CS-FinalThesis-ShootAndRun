using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour {

    public PlayerPowerUpManager powerUpManager;
    // Use this for initialization
    void Start()
    {
        powerUpManager.ExtraDash();
        powerUpManager.ExtraDash();
        powerUpManager.ExtraDash();
        powerUpManager.AddRange();
        powerUpManager.AddRange();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
