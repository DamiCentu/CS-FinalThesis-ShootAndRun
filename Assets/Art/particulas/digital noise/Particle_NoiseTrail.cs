using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_NoiseTrail : MonoBehaviour {

	ParticleSystem part;

	public float ChangeAxisTime = .1f;
	public float Strenght = 2;

	float[] Strenghts = {0,0,0 };
	float changeAxisTimer;

	void Start () {
		Strenghts [0] = Strenght;
		part = GetComponent<ParticleSystem> ();
	}


	void Update () {
		var noise = part.noise;
		changeAxisTimer += Time.deltaTime;
		if (changeAxisTimer >= ChangeAxisTime) {
			changeAxisTimer = 0;
			var val = Strenghts [0];
			Strenghts [0] = Strenghts [1];
			Strenghts [1] = Strenghts [2];
			Strenghts [2] = val;
			noise.strengthX = Strenghts[0];
			noise.strengthY = Strenghts[1];
			noise.strengthZ = Strenghts[2];
			if (Random.value >= .5)
				Strenght = -Strenght;
		}
	}
}
