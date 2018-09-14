using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceEffect : MonoBehaviour
{
	Material _mat;
	//public float radius;
	public Transform ball;


	void Start()
	{
		_mat = GetComponent<Renderer> ().sharedMaterial;
		_mat.SetVector ("_HitWorldPos", ball.position);
	}

	/*private void OnTriggerEnter(Collider c)
	{
		_mat.SetVector ("_HitWorldPos", c.transform.position);
		_mat.SetVector("_ImpactVector", Vector3.up);
	}*/

	void Update()
	{
		_mat.SetVector ("_HitWorldPos", ball.position);
		_mat.SetVector ("_ImpactVector", -Vector3.up);
	}
}
