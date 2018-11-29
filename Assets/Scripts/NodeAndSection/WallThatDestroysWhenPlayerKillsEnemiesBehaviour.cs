using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallThatDestroysWhenPlayerKillsEnemiesBehaviour : MonoBehaviour {

    public int id = 0;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    public void Show(bool v) {
        if (v)
        {
            StartCoroutine("Appear");
        }
        else {
            StartCoroutine("Disolve");
        }
    }
    IEnumerator Appear()
    {
        print("aparezco");
        this.GetComponent<Collider>().enabled = true;
        rend.material.SetFloat("_time", 6);

        while (rend.material.GetFloat("_time") > 0) {
            print("aparezco");
            var time = Mathf.Clamp(rend.material.GetFloat("_time") - 0.2f,0,10);
            rend.material.SetFloat("_time", time);
            yield return new WaitForSeconds(.05f);
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
        }
        this.GetComponent<Collider>().enabled = false;
    }
}
