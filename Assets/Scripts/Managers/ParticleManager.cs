using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

    Pool<ParticleBehaviur> _particlePool;

    public GameObject parentParticle;

    GameObject _container;

    void Start() {
        _container = new GameObject("ParentParticleContainer");
        _particlePool = new Pool<ParticleBehaviur>(15, ParticleFactory, ParticleBehaviur.InitializeParticle, ParticleBehaviur.DisposeParticle, true);
        EventManager.instance.AddEvent(Constants.PARTICLE_SET);
        EventManager.instance.AddEvent(Constants.PARTICLE_RETURN_TO_POOL);
        EventManager.instance.SubscribeEvent(Constants.PARTICLE_SET, SetParticle);
        EventManager.instance.SubscribeEvent(Constants.PARTICLE_RETURN_TO_POOL, ReturnParticleToPool);
    } 

    ParticleBehaviur giveMeParticle() {
        return _particlePool.GetObjectFromPool();
    }

    ParticleBehaviur ParticleFactory() {
        var a = Instantiate(parentParticle, _container.transform).GetComponent<ParticleBehaviur>();
        a.gameObject.SetActive(false);
        return a;
    }

    void ReturnParticleToPool(params object[] param) {
        _particlePool.DisablePoolObject((ParticleBehaviur)param[0]);
    } 

    void SetParticle(params object[] param) {
        var p = giveMeParticle().SetVFX((string)param[0]).SetPosition((Vector3)param[1]);
        p.gameObject.SetActive(true);
    }

    void OnDestroy() { 
        EventManager.instance.UnsubscribeEvent(Constants.PARTICLE_SET, SetParticle);
        EventManager.instance.UnsubscribeEvent(Constants.PARTICLE_RETURN_TO_POOL, ReturnParticleToPool);
    }
}
