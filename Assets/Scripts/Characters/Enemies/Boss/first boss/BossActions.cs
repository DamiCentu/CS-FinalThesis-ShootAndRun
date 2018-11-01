using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BossActions  {
    void Begin(AbstractBoss boss);
    void Finish(AbstractBoss boss);
    void DeleteAll();
    void Update(Transform boss, Vector3 playerPosition);
    void Upgrade();
}
