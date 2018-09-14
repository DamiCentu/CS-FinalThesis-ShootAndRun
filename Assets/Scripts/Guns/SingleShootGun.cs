using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShootGun : IShootable {

    public override void Shoot(Transform shootPosition, Vector3 forward) {
        if (_canShoot)
        {
            _timer = cooldown;

            NormalBullet b = BulletManager.instance.GetBulletFromPool();
            BulletManager.instance.SetBullet(b, shootPosition.position, forward);

        }
    }
}
