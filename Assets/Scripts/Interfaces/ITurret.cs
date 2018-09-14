using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurret {
    void OnUpdate();  
    void SetHitsCanTake();
    void SetStartValues();
    //retorna true fue destruido
    bool OnHitReturnIfDestroyed(int damage);
}

