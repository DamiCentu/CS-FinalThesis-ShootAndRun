using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAndDeactivateGO : MonoBehaviour {
    public GameObject[] allToDeactivate;

	public void ActivateGO()
    {
        foreach (var go in allToDeactivate)
        { 
            go.SetActive(true);
        }
    }

    public void DisableGO()
    {
        foreach (var go in allToDeactivate)
        {
            go.SetActive(false);
        }
    }
}
