using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Timer {

    private Action action;
    private DateTime finishDate;
    private float sec; 
    public Timer(float seconds, Action action) {
        sec = seconds;
        this.action = action;
        this.finishDate = DateTime.Now.AddSeconds(seconds);
    }

    public bool CheckAndRun()
    {
        if (DateTime.Now > this.finishDate)
        {
            this.action.Invoke();
            return true;
        }

        return false;
    }

    public void Reset() {
        this.finishDate = DateTime.Now.AddSeconds(sec);

    }
    public void Reset(float segundos)
    {
        this.finishDate = DateTime.Now.AddSeconds(segundos);

    }
}
