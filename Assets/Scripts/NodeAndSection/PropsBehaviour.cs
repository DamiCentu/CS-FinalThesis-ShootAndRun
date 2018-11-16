using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsBehaviour : MonoBehaviour,IHittable , IPauseable {

    public GameObject particlePrefab;

    ParticleSystem _particle;
    MeshRenderer _rend;
    Collider _col;

    bool _paused;
    public void OnPauseChange(bool v)
    {
        _paused = v;
    }

    void Start () {
        _particle = Instantiate(particlePrefab,transform).GetComponent<ParticleSystem>();
        _rend = GetComponent<MeshRenderer>();
        var s = _particle.shape;
        _particle.transform.position = transform.position;
        s.scale = transform.localScale;
        //_particle.transform.localScale = new Vector3(_particle.transform.localScale.x/ transform.localScale.x , _particle.transform.localScale.y/ transform.localScale.y , _particle.transform.localScale.z/  transform.localScale.z);
        //s.meshRenderer = _rend;
        _col = GetComponent<Collider>();
        _particle.Stop();
    }

    public void OnHit(int Damage) {
        StartCoroutine(PlayParticle()); 
    }

    public void RestartProp() {
        gameObject.SetActive(true);
        _particle.Stop();
        _rend.enabled = true;
        _col.enabled = true;
    }

    IEnumerator PlayParticle() {
        _particle.Play();
        _rend.enabled = false;
        _col.enabled = false;
        yield return new WaitForSeconds(_particle.main.duration);
        while (_paused)
            yield return null;
        gameObject.SetActive(false);
    }
}
