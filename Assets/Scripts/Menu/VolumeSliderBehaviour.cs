using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSliderBehaviour : MonoBehaviour {

    public AudioMixer mixer;
    public Slider slider;

    void Start () {
        float result = 0f;
        if (mixer.GetFloat("Volume",out result))
            slider.value = result;
    }
	
	public void SetVolume(float value)
    {
        mixer.SetFloat("Volume", value);
    }
}
