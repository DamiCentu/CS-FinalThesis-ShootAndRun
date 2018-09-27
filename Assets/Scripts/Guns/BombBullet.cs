using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBullet : IBullet {
    public float radius;
    public LayerMask hittableLayer;
    public LayerMask obstacleLayers;
	Vector3 posOfCol = new Vector3();
	bool _canDrawGizmo;
    private float timerd;
    public ParticleSystem DestroyParticle1;
    public ParticleSystem DestroyParticle2;
    public void Start()
	{
        DestroyParticle1 = GameObject.Find("distorsionBoom").GetComponent<ParticleSystem>();
        DestroyParticle2 = GameObject.Find("ExplotionBoom").GetComponent<ParticleSystem>();
    }
    public void FixedUpdate()
    {
        this.transform.position += this.transform.forward * speed;
        timerd += Time.deltaTime;
        if (timerd > lifeTime) {
            Destroy(this.gameObject);
        }


    }
    void OnTriggerEnter(Collider c)
	{
        if ((obstacleLayers & 1 << c.gameObject.layer) == 1 << c.gameObject.layer)
        {
            _canDrawGizmo = true;
            posOfCol = transform.position;

            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, radius, hittableLayer, QueryTriggerInteraction.Collide);
            foreach (Collider item in hitColliders)
            {
                IHittable hittable = item.gameObject.GetComponent<IHittable>();
                if (hittable != null) hittable.OnHit(damage);
            }

            var a = GetComponent<Renderer>();
            var col = GetComponent<Collider>();
            col.enabled = false;
            a.enabled = false;

            DestroyParticle1.transform.position = this.transform.position;
            DestroyParticle1.gameObject.SetActive(true);
            DestroyParticle1.Play();
            DestroyParticle2.transform.position = this.transform.position;
            DestroyParticle2.gameObject.SetActive(true);
            DestroyParticle2.Play();
            Destroy(this.gameObject);
        }
	}

	/*IEnumerator gizmoRoutine() {
		yield return new WaitForSeconds(3f);
		_canDrawGizmo = false;
		Destroy(this.gameObject);
	}
    */
	private void OnDrawGizmos()
	{
		if (_canDrawGizmo) { 
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(posOfCol, radius);
		}
	}

}
