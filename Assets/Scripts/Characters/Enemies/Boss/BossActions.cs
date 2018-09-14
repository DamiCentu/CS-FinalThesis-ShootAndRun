using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BossActions  {
    void Begin(Boss boss);
    void Finish(Boss boss);
    void Update(Transform boss, Vector3 playerPosition);
    void Upgrade();
}
