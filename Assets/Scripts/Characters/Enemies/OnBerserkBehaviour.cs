using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBerserkBehaviour : MonoBehaviour {

    public ParticleSystem berserkParticleS;

	void Start () {
        EventManager.instance.SubscribeEvent(Constants.BERSERK, OnBerserk);
        EventManager.instance.SubscribeEvent(Constants.STOP_BERSERK, OnBerserkEnd); 
    }
	
	void OnBerserk(params object[] param) {
        if(berserkParticleS != null) { 
            berserkParticleS.Play();
        }
    }

    void OnBerserkEnd(params object[] param) {
        if(berserkParticleS != null) { 
            berserkParticleS.Stop();
        }
    }

    void OnDisable() {
        if(berserkParticleS != null) { 
            berserkParticleS.Stop();
        }
    }
}
