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
        Menu.instance.OpenUlt();
    }

    public void SetHard() {
        Configuration.instance.SetHard();
        Menu.instance.OpenUlt();
    }

    public void SetMedium() {
        Configuration.instance.SetMedium();
        Menu.instance.OpenUlt();
    }
    public void SetBerseker()
    {
        Configuration.instance.SetUltBerserker();
        StartGame();
    }

    public void SetScatter()
    {
        Configuration.instance.SetUltScatter();
        StartGame();
    }
    public void SetSpawnMinions()
    {
        Configuration.instance.SetSpawnMinions();
        StartGame();
    }
    void StartGame()
    {
        SceneManager.LoadScene("Scene1");
    }



}
