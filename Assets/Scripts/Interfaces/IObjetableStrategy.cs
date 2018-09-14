using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjetableStrategy {
    bool ObjectiveComplete();
    void SetEnemyQuantityToReachObjective(int quantity);
}
