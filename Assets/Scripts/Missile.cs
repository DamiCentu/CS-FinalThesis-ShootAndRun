using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile: MonoBehaviour
{
    float t;
    Vector3 startPosition;
    Vector3 target;
    float timeToReachTarget;
    public LayerMask hitLayers;
    public int damage = 3;
    public GameObject prefabMark;
    GameObject mark;
    public float radius=2.8f;

    public void Set(Vector3 destination, float time) {
        t = 0;
        startPosition = transform.position;
        timeToReachTarget = time;
        target = destination;
        this.transform.Rotate(Vector3.right, 90);

        CreateMark();

    }

    private void CreateMark()
    {
        mark= Instantiate(prefabMark);
        mark.transform.position = target;
    }

    private void Update()
    {
        t += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, target, t);
    }


  

    private void OnTriggerEnter(Collider c)
    {
        //print("asd");
        if ((hitLayers & 1 << c.gameObject.layer) == 1 << c.gameObject.layer)
        {
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, radius, hitLayers, QueryTriggerInteraction.Collide);
            foreach (Collider item in hitColliders)
            {
                IHittable hittable = item.gameObject.GetComponent<IHittable>();
                if (hittable != null) hittable.OnHit(damage);
            }

        }
        DestroyMissile();
    }
    private void OnTriggerStay(Collider c)
    {
        //print("asd");
        if ((hitLayers & 1 << c.gameObject.layer) == 1 << c.gameObject.layer)
        {
            IHittable ihittable = c.gameObject.GetComponent<IHittable>();
            if (ihittable != null)
            {
                ihittable.OnHit(damage);
            }
        }
        DestroyMissile();
    }

    public void DestroyMissile()
    {
        //print("chau misil");
  //      EventManager.instance.ExecuteEvent(Constants.MISILE_DESTROY);
        //print(this.gameObject);
        Destroy(this.gameObject);
        Destroy(mark);
    }
}