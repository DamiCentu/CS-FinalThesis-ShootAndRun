using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstTurretStrategy : ITurret {

    bool _shooting = false;
    int _hitsRemaining = 0;

    EnemyTurretBehaviour _parent;

    public BurstTurretStrategy(EnemyTurretBehaviour parent) {
        _parent = parent;
    } 

    public void OnUpdate() {
        if (Utility.InRange(_parent.transformToRotate.position, EnemiesManager.instance.player.transform.position, _parent.distanceToShoot)) {
            var pPos = new Vector3(EnemiesManager.instance.player.transform.position.x, _parent.transformToRotate.position.y, EnemiesManager.instance.player.transform.position.z);
            _parent.transformToRotate.rotation = Quaternion.LookRotation(pPos - _parent.transformToRotate.position) * Quaternion.Euler(new Vector3(0f,-180,-90f)); 
            if (!_shooting)
                _parent.StartCoroutine(ShootRoutine());
        }
    }

    IEnumerator ShootRoutine() {
        _shooting = true;
        yield return new WaitForSeconds(_parent.timeToStartShootingBurst);
        for (int i = 0; i < _parent.shotsInBurst; i++) {
            var s = EnemyBulletManager.instance.giveMeEnemyBullet();
            s.SetPos(_parent.shotSpawn.position).SetDir(_parent.shotSpawn.forward).gameObject.SetActive(true);
            yield return new WaitForSeconds(_parent.timeDelayInBurst / SectionManager.instance.EnemiesMultiplicator);
        }
        yield return new WaitForSeconds(_parent.timeToWaitBetweenShots / SectionManager.instance.EnemiesMultiplicator);
        _shooting = false;
    }

    public bool OnHitReturnIfDestroyed(int damage) {
        _hitsRemaining -= damage;
        _parent.StartHitRoutine();
        if (_hitsRemaining <= 0) {
            _shooting = false;
            EnemiesManager.instance.ReturnTurretEnemyToPool(_parent);
            _parent.gameObject.SetActive(false);
            return true;
        }
        return false;
    }

    public void SetStartValues() {
        _parent.shieldGO.SetActive(false);
        _shooting = false;
    }

    public void SetHitsCanTake() {
        _hitsRemaining = _parent.hitsCanTakeBurst;
    }
}
