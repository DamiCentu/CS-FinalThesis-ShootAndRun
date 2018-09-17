using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraITween : MonoBehaviour {

    public Image blackScreen;
    public float timeToFade = 50f;
    float alpha;
    Timer timer;
    float fadeTimer;
    bool fadeIn=false;
    bool fadeOut=false;
    bool AllBlack = true;

    void Start() {
        EventManager.instance.SubscribeEvent(Constants.STARTED_SECTION, FadeInCamera);
        EventManager.instance.SubscribeEvent(Constants.BLACK_SCREEN, FadeInCamera);
        blackScreen.color = new Color(0, 0, 0, 1);
    }

    void FixedUpdate() {
        if (timer == null) {
            return;
        }

        timer.CheckAndRun();

        if (fadeIn || fadeOut) {
            fadeTimer += Time.deltaTime; 
        }

        if (fadeIn) {
            float value = Mathf.Lerp(0, 1, fadeTimer / timeToFade);
            
            tweenOnUpdateCallBack(value);
        }
        if (fadeOut) {
            float value = Mathf.Lerp(1, 0, fadeTimer / timeToFade);
            tweenOnUpdateCallBack(value);
        }
    } 

    void FadeInCamera(object[] parameterContainer) {
        //print("lolo");
        if (!AllBlack) {
            //print("lala");
            fadeIn = true;
            
            fadeOut = false;
            fadeTimer = 0;
        }
        timer = new Timer(timeToFade + 0.5f, FadeOutCamera);
    }
     
    void tweenOnUpdateCallBack(float newValue) {
        blackScreen.color = new Color(0, 0, 0, newValue);
    } 

    void FadeOutCamera() { 
        fadeIn = false;
        fadeOut = true;
        AllBlack = false;
        fadeTimer = 0;
        timer = new Timer(timeToFade, StopFade); 
    }

    void StopFade() {
        fadeOut = false;
        AllBlack = false;
    } 
}
