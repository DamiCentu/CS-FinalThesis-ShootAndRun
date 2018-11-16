using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : IBullet , IPauseable {
    public LayerMask hitLayers;
    public float maxRange=1.5f;

    /*
        public void MoreRange(float multiplier)
        {
            Mathf.Clamp(lifeTime, lifeTime * multiplier, maxRange);
        }*/
    bool _paused;
    public void OnPauseChange(bool v) {
        _paused = v;
    }

    void OnTriggerEnter(Collider c) {
        
        if ((hitLayers & 1 << c.gameObject.layer) == 1 << c.gameObject.layer) {
            IHittable ihittable = c.gameObject.GetComponent<IHittable>();
            //Debug.Log(c.gameObject.name);
            if (ihittable != null)
            {
                ihittable.OnHit(damage);
                //Debug.Log("hit");
            }

            DestroyBullet();
        }

        if(c.gameObject.layer == 12) {//enemy
            EventManager.instance.ExecuteEvent(Constants.PARTICLE_SET, new object[] { Constants.PARTICLE_HERO_BULLET_HIT_NAME, transform.position });
            return;
        }

        if(c.gameObject.layer == 19) {
            EventManager.instance.ExecuteEvent(Constants.SOUND_BULLET_HIT);
        }

        RaycastHit rh;
        var pPos = new Vector3(EnemiesManager.instance.player.transform.position.x, transform.position.y, EnemiesManager.instance.player.transform.position.z);
        var dirFromPlayerToBullet = transform.position - pPos;
        if(Physics.Raycast(pPos, dirFromPlayerToBullet, out rh, dirFromPlayerToBullet.magnitude + 5f, hitLayers))
            EventManager.instance.ExecuteEvent(Constants.PARTICLE_SET, new object[] { Constants.PARTICLE_HERO_BULLET_HIT_NAME, rh.point });
        else
           if(rh.collider != null && !rh.collider.isTrigger)
                EventManager.instance.ExecuteEvent(Constants.PARTICLE_SET, new object[] { Constants.PARTICLE_HERO_BULLET_HIT_NAME, transform.position });

    }
    public void FixedUpdate()
    {
        if (_paused)
            return;

        this.transform.position += this.transform.forward * speed;
        // rb.velocity = this.transform.forward * speed;
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            DestroyBullet();
        }

    }
}
