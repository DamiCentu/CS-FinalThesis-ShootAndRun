using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour {
    
    public static Menu instance;
    public GameObject cheatMode;

    void Start () {
        instance = this;
    }

    public void CloseGame() {
        Application.Quit(); 
    }

    public void SetSinglePlayer(Waypoint waypoint) {
        Configuration.instance.SetSinglePlayer();

        if (waypoint != null)
            EventManager.instance.ExecuteEvent(Constants.MENU_CAMERA_NAVIGATE, new object[] { waypoint });
    }

    public void SetTwoPlayer(Waypoint waypoint) {
        Configuration.instance.SetTwoPlayer();

        if (waypoint != null)
            EventManager.instance.ExecuteEvent(Constants.MENU_CAMERA_NAVIGATE, new object[] { waypoint });
    }
    public void SetLvl1(Waypoint waypoint)
    {
        Configuration.instance.SetLvl1();

        if(waypoint != null)
            EventManager.instance.ExecuteEvent(Constants.MENU_CAMERA_NAVIGATE, new object[] { waypoint });
    }

    public void GoToUltimates(Waypoint waypoint)
    {
        if (waypoint != null)
            EventManager.instance.ExecuteEvent(Constants.MENU_CAMERA_NAVIGATE, new object[] { waypoint });
    }

    public void GoToMainMenu(Waypoint waypoint)
    {
        if (waypoint != null)
            EventManager.instance.ExecuteEvent(Constants.MENU_CAMERA_NAVIGATE, new object[] { waypoint });
    }

    public void GoToSettings(Waypoint waypoint)
    {
        if (waypoint != null)
            EventManager.instance.ExecuteEvent(Constants.MENU_CAMERA_NAVIGATE, new object[] { waypoint });
    }

    public void SetLvl2()
    {
        Configuration.instance.SetLvl2();
        //OpenSttings();
    }

    public void QualitySettingsPannel(Waypoint waypoint)
    {
        if (waypoint != null)
            EventManager.instance.ExecuteEvent(Constants.MENU_CAMERA_NAVIGATE, new object[] { waypoint });
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) {
            cheatMode.SetActive(true);

        }
    }

    public void Seccion1_1 (){
        SetLvl1(null);
        Configuration.instance.SetNode(1);
    }

    public void Seccion1_2()
    {
        SetLvl1(null);
        Configuration.instance.SetNode(2);
    }

    public void Seccion1_3()
    {
        SetLvl1(null);
        Configuration.instance.SetNode(3);
    }

    public void Seccion1_4()
    {
        SetLvl1(null);
        Configuration.instance.SetNode(4);
    }

    public void Seccion2_1()
    {
        SetLvl2();
        Configuration.instance.SetNode(1);
    }

    public void Seccion2_2()
    {
        SetLvl2();
        Configuration.instance.SetNode(2);

    }

    public void Seccion2_3()
    {
        SetLvl2();
        Configuration.instance.SetNode(3);
    }
    public void Seccion2_4()
    {
        SetLvl2();
        Configuration.instance.SetNode(4);
    }

    public void Seccion2_5()
    {
        SetLvl2();
        Configuration.instance.SetNode(5);
    }
    public void ActiveDebugMode()
    {
        Configuration.instance.SetDebugMode(true);
    }
    public void UnactiveDebugMode()
    {
        Configuration.instance.SetDebugMode(false);
    }
}
