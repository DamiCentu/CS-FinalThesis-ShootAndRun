using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryBulletExplosion : MonoBehaviour {

    public float speed = 3;
    public GameObject parent;
    public GameObject lightGO;

    private ParticleSystem _radiusParticleSystem;
    private float _radius;

    private float _currentRadius;
    private bool _endRoutinePlayed;

    public float Radius
    {
        get
        {
            return _radius;
        }

        set
        {
            _radius = value;
        }
    }

    void Start () {
        _radiusParticleSystem = GetComponent<ParticleSystem>();
    }

	void Update () {
        var shape = _radiusParticleSystem.shape;
        if (shape.normalOffset < _radius)
        {
            shape.normalOffset += speed * Time.deltaTime;
        }
        else
        {
            if(!_endRoutinePlayed)
            {
                _endRoutinePlayed = true;
                StartCoroutine(endRoutine());
            }
        }
	}

    IEnumerator endRoutine()
    {
        yield return new WaitForSeconds(1);
        _radiusParticleSystem.Stop();
        lightGO.SetActive(false);
        Destroy(parent, 3);
    }
}
