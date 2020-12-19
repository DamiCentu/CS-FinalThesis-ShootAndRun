using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UltimateColor : MonoBehaviour {
    Image image;
	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if (image.fillAmount == 1)
        {
            image.color = Color.white;
        }
        else {
            image.color = Color.gray;
        }
	}
}
