using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBullet : MonoBehaviour {

    public float radius;
    public LayerMask hittableLayer;
    public float timeToBoom=2;
    Timer timer;
    public int damage = 3;
    public LineRenderer line;



    public void Boom()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, radius, hittableLayer, QueryTriggerInteraction.Collide);
        foreach (Collider item in hitColliders)
        {
            IHittable hittable = item.gameObject.GetComponent<IHittable>();
            if (hittable != null) hittable.OnHit(damage);
        }
        Destroy(this.gameObject);
    }



}
