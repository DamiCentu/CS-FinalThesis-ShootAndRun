﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour {

    public GameObject menuObjects;
    public GameObject settingsObjects;
    public GameObject ultObject;
    public static Menu instance;
    void Start () {
        settingsObjects.SetActive(false);
        ultObject.SetActive(false);
        instance = this;
    }

    private void OpenSttings() {
        menuObjects.SetActive(false);
        settingsObjects.SetActive(true);
        ultObject.SetActive(false);
    }
    public void OpenUlt()
    {
        menuObjects.SetActive(false);
        settingsObjects.SetActive(false);
        ultObject.SetActive(true);
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

    /* public void PlayTutorial() {
         SceneManager.LoadScene("Tutorial");
     }*/
}
