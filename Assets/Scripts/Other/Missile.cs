using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile: MonoBehaviour , IPauseable
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
    public GameObject DecayMark;

    public ParticleSystem DestroyParticle1;
    private ParticleSystem DestroyParticle2;
    public string DestroyParticle2Name= "explosion misil";
    bool _paused;
    public void OnPauseChange(bool v)
    {
        _paused = v;
    }

    public void Set(Vector3 destination, float time) {
        t = 0;
        startPosition = transform.position;
        timeToReachTarget = time;
        target = destination;
        this.transform.Rotate(Vector3.right, 90);

        CreateMark();
        DestroyParticle1 = GameObject.Find("distorsionBoom").GetComponent<ParticleSystem>();
//        DestroyParticle2 = ((GameObject)Resources.Load(DestroyParticle2Name)).GetComponent<ParticleSystem>();
    }

    private void CreateMark()
    {
        mark= Instantiate(prefabMark);
        mark.transform.position = target;
    }

    private void Update()
    {
        if (_paused)
            return;

        t += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, target, t);


    }

    private void OnTriggerEnter(Collider c)
    {
        //print("asd");
        
        if (Utility.IsInLayerMask(c.gameObject.layer, hitLayers))
        {
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, radius, hitLayers, QueryTriggerInteraction.Collide);
            foreach (Collider item in hitColliders)
            {
                IHittable hittable = item.gameObject.GetComponent<IHittable>();
                if (hittable != null) hittable.OnHit(damage);
            }
            DestroyMissile();
        } 
    }
    private void OnTriggerStay(Collider c)
    {
        //print("asd");
        if (Utility.IsInLayerMask(c.gameObject.layer, hitLayers))
        {
            IHittable ihittable = c.gameObject.GetComponent<IHittable>();
            if (ihittable != null)
            {
                ihittable.OnHit(damage);
            }
            DestroyMissile();
        } 
    }

    public  virtual void DestroyMissile()
    {
        //print("chau misil");
  //      EventManager.instance.ExecuteEvent(Constants.MISILE_DESTROY);
        //print(this.gameObject);
        DestroyParticle1.transform.position = this.transform.position;
        DestroyParticle1.gameObject.SetActive(true);
        DestroyParticle1.Play();
        DestroyParticle2 = Instantiate ((GameObject)Resources.Load(DestroyParticle2Name),this.transform.position,this.transform.rotation).GetComponent<ParticleSystem>();
        DestroyParticle2.transform.position = this.transform.position;
        DestroyParticle2.gameObject.SetActive(true);
        DestroyParticle2.Play();
    //    GameObject g=Instantiate(DecayMark, new Vector3(mark.transform.position.x, -0.43f, mark.transform.position.z), mark.transform.rotation);
        //Destroy(g, 2);
        Destroy(mark);
        gameObject.SetActive(false);
        Destroy(this.gameObject);
        Destroy(DestroyParticle2.gameObject,3);
    }
}