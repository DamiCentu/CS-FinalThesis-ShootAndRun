using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : IBullet {
    public LayerMask hitLayers;
    public float maxRange=1.5f;

    /*
        public void MoreRange(float multiplier)
        {
            Mathf.Clamp(lifeTime, lifeTime * multiplier, maxRange);
        }*/

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
        this.transform.position += this.transform.forward * speed;
        // rb.velocity = this.transform.forward * speed;
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            DestroyBullet();
        }

    }
}
