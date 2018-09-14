using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehaviur : MonoBehaviour {

    public List<GameObject> vfxs;
    public float particleTime = 3f;

    Dictionary<string, GameObject> _vfxsDic = new Dictionary<string, GameObject>();

    float _time = 0;
	
	void Update () {
        _time += Time.deltaTime;
        if(_time > particleTime) {
            EventManager.instance.ExecuteEvent(Constants.PARTICLE_RETURN_TO_POOL, new object []{this});
        }
	}

    public ParticleBehaviur SetVFX(string name) {
        if (_vfxsDic.ContainsKey(name))
            _vfxsDic[name].SetActive(true);

        else { 
            foreach (var vfx in vfxs) {
                if (vfx.tag == name) { 
                    _vfxsDic[name] = vfx;
                    vfx.SetActive(true);
                }
            }
        } 

        //if(name == Constants.PARTICLE_ENEMY_EXPLOTION_NAME) {
        //    _vfxsDic[name].GetComponent<ParticleSystem>().
        //}
        return this;
    }

    public ParticleBehaviur SetPosition(Vector3 position) {
        transform.position = position;
        return this;
    }

    public static void InitializeParticle(ParticleBehaviur particle) { }

    public void OnDispose() {
        foreach (var vfx in vfxs) {
            if (vfx.activeSelf)
                vfx.SetActive(false);
        }
        _time = 0f;
    }

    public static void DisposeParticle(ParticleBehaviur particle) {
        particle.OnDispose();
        particle.gameObject.SetActive(false);
    }
}
