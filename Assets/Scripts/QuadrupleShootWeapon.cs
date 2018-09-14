using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadrupleShootWeapon : IShootable {
    public float offset = 0.5f;
    public float anglePercentage = 0.5f;
    public override void Shoot(Transform shootPosition, Vector3 forward)
    {
        if (_canShoot)
        {
            _timer = cooldown;

            Vector3 right = Vector3.Cross(forward, Vector3.up);

            NormalBullet b = BulletManager.instance.GetBulletFromPool();
            BulletManager.instance.SetBullet(b, shootPosition.position + right * offset * 2, forward + right * anglePercentage);

            b = BulletManager.instance.GetBulletFromPool();
            BulletManager.instance.SetBullet(b, shootPosition.position + right * offset * 1, forward);

            b = BulletManager.instance.GetBulletFromPool();
            BulletManager.instance.SetBullet(b, shootPosition.position + right * offset * -1, forward );

            b = BulletManager.instance.GetBulletFromPool();
            BulletManager.instance.SetBullet(b, shootPosition.position + right * offset * -2, forward - right * anglePercentage);

        }
    }
}
