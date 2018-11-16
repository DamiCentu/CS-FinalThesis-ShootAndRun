using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IShootable : MonoBehaviour , IPauseable
{
    public float cooldown =1;
    protected float _timer = 0.0f;
    protected bool _canShoot = true;

    public abstract void Shoot(Transform shootPosition, Vector3 forward);

    bool _paused;
    public void OnPauseChange(bool v)
    {
        _paused = v;
    }

    void Update()
    {
        if (_paused)
            return;

        if (_timer <= 0.0f )
        {
            _canShoot = true;
        }
        else {
            _canShoot = false;
            _timer -= Time.deltaTime;
            //print(this.GetType());
        }

    }

    internal void addRange(float v)
    {
        
    }
}
