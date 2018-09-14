using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour {
    enum Dificulty { Easy, Medium, Hard };
    Dificulty dificulty;

    public void SetEasy() {
        Configuration.instance.SetEasy();
        StartGame();
    }

    public void SetHard() {
        Configuration.instance.SetHard();
        StartGame();
    }

    public void SetMedium() {
        Configuration.instance.SetMedium();
        StartGame();
    }

    void StartGame() {
        SceneManager.LoadScene("Scene1");
    }
}
