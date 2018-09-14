using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour {

    public GameObject menuObjects;
    public GameObject settingsObjects;

	void Start () {
        settingsObjects.SetActive(false);
    }

    private void OpenSttings() {
        menuObjects.SetActive(false);
        settingsObjects.SetActive(true);
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

    public void PlayTutorial() {
        SceneManager.LoadScene("Tutorial");
    }
}
