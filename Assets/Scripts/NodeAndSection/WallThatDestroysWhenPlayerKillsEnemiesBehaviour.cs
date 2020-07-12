using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallThatDestroysWhenPlayerKillsEnemiesBehaviour : MonoBehaviour, IPauseable {

    public int id = 0;
    public bool destroyAtEnd = false;

    Renderer rend;
    Collider col;

    bool _pause;
    bool _isOn = true;

    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
    }

    public void Show(bool v) {
        if (col == null || rend == null) return;
        if (v)
        {
            if (destroyAtEnd)
                rend.enabled = true;

            StartCoroutine("Appear");
        }
        else {
            StartCoroutine("Disolve");
        }
    }

    public void OnPauseChange(bool p) {
        _pause = p;
    }


    IEnumerator Appear()
    {
        if (!_isOn) {
            //print("aparezco");
            col.enabled = true;
            rend.material.SetFloat("_time", 1); 
          
            while (rend.material.GetFloat("_time") > -2) {
                //print("aparezco");
                var time = rend.material.GetFloat("_time") - 0.03f;
                rend.material.SetFloat("_time", time);
                yield return new WaitForEndOfFrame();
                if (_pause)
                    yield return null;
            }
            _isOn = true;
        }
        yield return null;
    }

    IEnumerator Disolve()
    {
        if (_isOn) {
            rend.material.SetFloat("_time", -2);
         
            while (rend.material.GetFloat("_time") < 1)
            {
                //print("desaparezco");
                var time = rend.material.GetFloat("_time") + 0.03f;
                //print("time");
                rend.material.SetFloat("_time", time);
                yield return new WaitForEndOfFrame();
                if (_pause)
                    yield return null;
            }
            col.enabled = false;
            _isOn = false;
            if (destroyAtEnd)
                rend.enabled = false;
        }
        yield return null;
    }
}
