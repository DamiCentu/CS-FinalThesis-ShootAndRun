using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : IShootable {

    public AudioSource slowAudio;


    public override void Shoot(Transform shootPosition, Vector3 forward)
    {
        SoundManager.instance.PlaySlow();

        print("slow!!!");
        EventManager.instance.ExecuteEvent(Constants.SLOW_TIME);
    }
}
