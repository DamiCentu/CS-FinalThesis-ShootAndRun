using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueLikeUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal void SetPhase(int phase)
    {
        //TODO ver los powerups que se consiguen
    }

    public void ApplyChanges() {
        //TODO aplicar los cambios y salir

        var section = FindObjectOfType<SectionNodeRoguelike>();
        section.shouldShowRogueLikeUI = false;
    }

}
