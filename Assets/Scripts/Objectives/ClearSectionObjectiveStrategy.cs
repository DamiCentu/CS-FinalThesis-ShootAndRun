using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSectionObjective : IObjetableStrategy {
    int _enemiesRemaining = 0;

    public ClearSectionObjective() {
        EventManager.instance.SubscribeEvent("EnemyDead", EnemyDead);
    }

    void EnemyDead(params object[] param) {
        _enemiesRemaining--;
    }

    public bool ObjectiveComplete() {
        if(_enemiesRemaining <= 0)
            return true;
        return false;
    }

    public void SetEnemyQuantityToReachObjective(int quantity) {
        _enemiesRemaining = quantity;
    }
}
