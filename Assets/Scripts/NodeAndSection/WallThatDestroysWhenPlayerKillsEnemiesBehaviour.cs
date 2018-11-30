using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallThatDestroysWhenPlayerKillsEnemiesBehaviour : MonoBehaviour, IPauseable {

    public int id = 0;
    Renderer rend;
    Collider col;

    bool _pause;

    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
    }

    public void Show(bool v) {
        if (col == null || rend == null) return;
        if (v)
        {
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
        print("aparezco");
        col.enabled = true;
        rend.material.SetFloat("_time", 6);

        while (rend.material.GetFloat("_time") > 0) {
            print("aparezco");
            var time = Mathf.Clamp(rend.material.GetFloat("_time") - 0.2f,0,10);
            rend.material.SetFloat("_time", time);
            yield return new WaitForSeconds(.05f);
            if (_pause)
                yield return null;
        }
    }

    IEnumerator Disolve()
    {
        rend.material.SetFloat("_time", 0);

        while (rend.material.GetFloat("_time") < 6)
        {
            print("desaparezco");
            var time = Mathf.Clamp(rend.material.GetFloat("_time") + 0.2f, 0, 10);
            print("time");
            rend.material.SetFloat("_time", time);
            yield return new WaitForSeconds(.05f);
            if (_pause)
                yield return null;
        }
        col.enabled = false;
    }
}
