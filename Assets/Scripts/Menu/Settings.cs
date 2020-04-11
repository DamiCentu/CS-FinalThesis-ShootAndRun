using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public GameObject splash;
    public Image slider;

    enum Dificulty { Easy, Medium, Hard };
    Dificulty dificulty;

    public void SetEasy()
    {
        Configuration.instance.SetEasy();
        Menu.instance.OpenUlt();
        Menu.instance.HideEasyDescription();
    }

    public void SetHard()
    {
        Configuration.instance.SetHard();
        Menu.instance.OpenUlt();
        Menu.instance.HideHardDescription();
    }

    public void SetMedium()
    {
        Configuration.instance.SetMedium();
        Menu.instance.OpenUlt();
        Menu.instance.HideMediumDescription();
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
        if (Configuration.instance.lvl == 1)
        {
            StartCoroutine(LoadAsync(Constants.LEVEL_1_NAME));
        }
        else
        {
            StartCoroutine(LoadAsync(Constants.LEVEL_2_NAME));
        }
    }

    public void NextLvl()
    {
        Configuration.instance.NextLvl();
        StartCoroutine(LoadAsync(Constants.LEVEL_2_NAME));
    }

    public void SetLowGraphics()
    {
        QualitySettings.SetQualityLevel(0);
    }

    public void SetMediumGraphics()
    {
        QualitySettings.SetQualityLevel(2);
    }

    public void SetHighGraphics()
    {
        QualitySettings.SetQualityLevel(5);
    }

    IEnumerator LoadAsync(string name)
    {
        splash.SetActive(true);
        AsyncOperation async = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            var progress = Mathf.Clamp01(async.progress / 0.9f);
            slider.fillAmount = progress;

            if (async.progress >= 0.3f)
                yield return new WaitForSeconds(0.2f);

            if (async.progress >= 0.9f)
                async.allowSceneActivation = true;
        }
    }
}
