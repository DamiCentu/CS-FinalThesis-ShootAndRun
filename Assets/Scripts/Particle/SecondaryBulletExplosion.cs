using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryBulletExplosion : MonoBehaviour {

    public float speed = 50;
    public float speedShrink = 75;
    public GameObject parent;
    public GameObject lightGO;
    public Transform Mesh;

    private ParticleSystem _radiusParticleSystem;
    private float _radius;

    private float _currentRadius;
    private bool _endRoutinePlayed;
    private bool _haveToShrink;

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

        if(_haveToShrink)
        {
            if (Mesh.localScale.x > 0)
            {
                var newScale = Mesh.localScale.x - (speedShrink * Time.deltaTime);
                shape.scale = new Vector3(newScale, newScale, newScale);
                Mesh.localScale = new Vector3(newScale, newScale, newScale);
            }
            else
            {
                Destroy(parent);
            }

            return;
        }

        if (Mesh.localScale.x / 2 < _radius)
        {
            var newScale = Mesh.localScale.x + (speed * Time.deltaTime);
            shape.scale = new Vector3(newScale, newScale, newScale);
            Mesh.localScale = new Vector3(newScale, newScale, newScale);
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
        yield return new WaitForSeconds(0.3f);
        _haveToShrink = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 5);
    }
}
