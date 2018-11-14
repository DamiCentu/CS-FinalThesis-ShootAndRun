using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour, IPauseable {

    public float bulletSpeed = 5f;

    public LayerMask layerThatAffectEnemyBullet;

    TrailRenderer _trail;
    bool _paused;
    public void OnPauseChange(bool v) {
        _paused = v; 
    }

    public virtual void Update () {
        if (_paused)
            return;

        transform.position += bulletSpeed * Time.deltaTime * transform.forward * SectionManager.instance.EnemiesMultiplicator;
	}

    public EnemyBulletBehaviour SetPos(Vector3 pos) {
        if(_trail == null) {
            _trail = GetComponent<TrailRenderer>();
        }

        _trail.Clear();
        transform.position = pos;

        _trail.enabled = true;
        return this;
    }

    public EnemyBulletBehaviour SetDir (Vector3 forw) {
        transform.forward = forw;
        return this;
    }
    public EnemyBulletBehaviour SetDir(Quaternion rot)
    {
        transform.rotation = rot;
        return this;
    }

    public virtual void OnTriggerEnter(Collider c) {
        if(layerThatAffectEnemyBullet == (layerThatAffectEnemyBullet | (1 << c.gameObject.layer))) {
            _trail.Clear();
            _trail.enabled = false;
            EnemyBulletManager.instance.ReturnEnemyBulletToPool(this);
            gameObject.SetActive(false);
        }
    } 
}
