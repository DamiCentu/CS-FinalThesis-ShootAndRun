using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {
    bool pause = false;
    MonoBehaviour[] allMonoBehavior;
   GameObject[] allGameObjects;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !pause)
        {
            allMonoBehavior = FindObjectsOfType<MonoBehaviour>();
            allGameObjects = FindObjectsOfType<GameObject>();
            Pause(true);
            pause = true;
        }
       else  if (Input.GetKeyDown(KeyCode.P) )
        {
            pause = false;
            Pause(false);
        }
    }

    private void Pause(bool v)
    {
        foreach (var item in allGameObjects)
        {
          
          
            if (item.GetComponent<Player>() != null) print("no da null!");
            item.SetActive(!v);
        }

    }
}
