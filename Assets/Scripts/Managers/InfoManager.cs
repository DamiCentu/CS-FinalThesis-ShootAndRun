using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InfoManager : MonoBehaviour , IPauseable {
    public static InfoManager instance;
    public Text info;
    public float notificationTime=1;
    Timer timer;
    private float myTimer;

    bool _paused;
    public void OnPauseChange(bool v)
    {
        _paused = v;
    }

    void Awake()
    {
        instance = this;

    }



    public void Info(string text)
    {
        info.text = text;
        timer = new Timer(notificationTime, StopShowingInfo);
    }

    private void StopShowingInfo()
    {
        info.text = "";
        timer = null;
    }

    private void Update()
    {
        if (_paused)
            return;

        if (timer != null) {
            timer.CheckAndRun();
        }
        if (myTimer > 0) {
            myTimer -= Time.deltaTime;
            if (myTimer >= 1)
            {
                info.text = ((int)myTimer).ToString();
            }
            else {
                StopShowingInfo();
            }

        }
    }

    internal void CountDown(float time)
    {
        myTimer = time;

    }

}
