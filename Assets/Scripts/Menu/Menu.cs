using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour {

    public GameObject menuObjects;
    public GameObject settingsObjects;
    public GameObject ultObject;
    public GameObject qualitySettingsObjects;
    public static Menu instance;
    public GameObject cheatMode;
    public GameObject easyDescription;
    public GameObject meduimDescription;
    public GameObject hardDescription;


    void Start () {
        OpenMenu();
        instance = this;
    }

    public void OpenMenu() {
        settingsObjects.SetActive(false);
        ultObject.SetActive(false);
        menuObjects.SetActive(true);
        cheatMode.SetActive(false);
        qualitySettingsObjects.SetActive(false);
    }

    public void OpenSttings() {
        menuObjects.SetActive(false);
        settingsObjects.SetActive(true);
        ultObject.SetActive(false);
        cheatMode.SetActive(false);
        qualitySettingsObjects.SetActive(false);
    }
    public void OpenUlt()
    {
        menuObjects.SetActive(false);
        settingsObjects.SetActive(false);
        ultObject.SetActive(true);
        cheatMode.SetActive(false);
        qualitySettingsObjects.SetActive(false);
    }

    public void OpenQualitySettingsPannel()
    {
        qualitySettingsObjects.SetActive(true);
        menuObjects.SetActive(false);
        settingsObjects.SetActive(false);
        ultObject.SetActive(false);
        cheatMode.SetActive(false);
    }

    public void CloseGame() {
        Application.Quit(); 
    }

    public void SetSinglePlayer() {
        Configuration.instance.SetSinglePlayer();
        OpenSttings();
    }

    public void SetTwoPlayer() {
        Configuration.instance.SetTwoPlayer();
        OpenSttings();
    }
    public void SetLvl1()
    {
        Configuration.instance.SetLvl1();
        OpenSttings();
    }

    public void SetLvl2()
    {
        Configuration.instance.SetLvl2();
        OpenSttings();
    }

    public void QualitySettingsPannel()
    {
        OpenQualitySettingsPannel();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)&& menuObjects.activeSelf) {
            cheatMode.SetActive(true);

        }
    }

    public void Seccion1_1 (){
        SetLvl1();
        Configuration.instance.SetNode(1);
    }

    public void Seccion1_2()
    {
        SetLvl1();
        Configuration.instance.SetNode(2);
    }

    public void Seccion1_3()
    {
        SetLvl1();
        Configuration.instance.SetNode(3);
    }

    public void Seccion1_4()
    {
        SetLvl1();
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

    public void ShowEasyDescription() {
        easyDescription.SetActive(true);
    }

    public void HideEasyDescription()
    {
        easyDescription.SetActive(false);
    }

    public void ShowMediumDescription()
    {
        meduimDescription.SetActive(true);
    }

    public void HideMediumDescription()
    {
        meduimDescription.SetActive(false);
    }

    public void ShowHardDescription()
    {
        hardDescription.SetActive(true);
    }

    public void HideHardDescription()
    {
        hardDescription.SetActive(false);
    }
    /* public void PlayTutorial() {
         SceneManager.LoadScene("Tutorial");
     }*/
}
