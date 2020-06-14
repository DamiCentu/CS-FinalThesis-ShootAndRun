using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBullet : IBullet , IPauseable {
    public float radius;
    public LayerMask hittableLayer;
    public LayerMask obstacleLayers;
	Vector3 posOfCol = new Vector3();
	bool _canDrawGizmo;
    private float timerd;

    public string SecondaryBombExplosionParticlesName = "SecondaryBombExplosionParticles";

    public ParticleSystem DestroyParticle1;
    public ParticleSystem DestroyParticle2;

    bool _paused;
    private string DestroyParticle2Name;
    private string DestroyParticle1Name;
    private string DestroyParticleName= "explo digital";
    private GameObject DestroyParticle;
    private GameObject destroyParticleSizeOfExplotion;

    public void OnPauseChange(bool v)
    {
        _paused = v;
    }

    public void FixedUpdate()
    {
        if (_paused)
            return;

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

            /*   DestroyParticle1 = Instantiate((GameObject)Resources.Load(DestroyParticle1Name), this.transform.position, this.transform.rotation).GetComponent<ParticleSystem>();
               DestroyParticle1.transform.position = this.transform.position;
               DestroyParticle1.gameObject.SetActive(true);
               DestroyParticle1.Play();

               DestroyParticle2 = Instantiate((GameObject)Resources.Load(DestroyParticle2Name), this.transform.position, this.transform.rotation).GetComponent<ParticleSystem>();
               DestroyParticle2.transform.position = this.transform.position;
               DestroyParticle2.gameObject.SetActive(true);
               DestroyParticle2.Play();

               Destroy(DestroyParticle1.gameObject,3);
               Destroy(DestroyParticle2.gameObject,3);*/

            DestroyParticle = Instantiate((GameObject)Resources.Load(DestroyParticleName), this.transform.position, this.transform.rotation);
            destroyParticleSizeOfExplotion = Instantiate((GameObject)Resources.Load(SecondaryBombExplosionParticlesName), this.transform.position, this.transform.rotation);
            destroyParticleSizeOfExplotion.GetComponentInChildren<SecondaryBulletExplosion>().Radius = radius;
           
            Destroy(DestroyParticle.gameObject,1);
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
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(posOfCol, radius);
		
	}

}
