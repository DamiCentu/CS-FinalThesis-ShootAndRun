using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Configuration : MonoBehaviour {
    public enum Dificulty { Easy, Medium, Hard };
    public Dificulty dificulty= Dificulty.Easy;
    public enum PlayersQuantity { One, Two };
    public PlayersQuantity playerQuantity = PlayersQuantity.One;
    public int creditsEasy = 10;
    public int creditsMedium = 5;
    public int creditsHard = 1;
    public int lvl=1;
    public int node = -1;
    public bool activeDebugMode = false;
    public enum PlayerSpecial { DumbMine, Slow, Bomb }
    public PlayerSpecial special;

    
    public int Credits { get { return GetCreditsAmount(); } }

    public static Configuration instance = null;

    public Player.Ults playerUlt;
    public bool playerInmortal;

    void Awake() {
        if (instance == null)
            instance = this; 

        DontDestroyOnLoad(this.gameObject);
    }

    public void SetDebugMode(bool b) {
        activeDebugMode = b;
    }

    public void SetSpecialMine( )
    {
        special = PlayerSpecial.DumbMine;
    }

    public void SetSpecialBomb()
    {
        special = PlayerSpecial.Bomb;
    }

    public void SetSpecialSlow()
    {
        special = PlayerSpecial.Slow;
    }

    public  void SetSinglePlayer() {
        playerQuantity = PlayersQuantity.One;
    }

    public void SetTwoPlayer() {
        playerQuantity = PlayersQuantity.Two;
    }

    internal void SetSpawnMinions()
    {
        playerUlt = Player.Ults.Spawn;
    }

    internal void NextLvl()
    {
        lvl = 2;
        node = -1;
    }

    public void SetEasy() {
        dificulty = Dificulty.Easy;
    }

    public void SetHard() {
        dificulty = Dificulty.Hard;
    }

    internal void SetLvl1()
    {
        lvl = 1;
        node = -1;
    }

    internal void SetLvl2()
    {
        lvl = 2;
        node = -1;
    }

    public void SetMedium() {
        dificulty = Dificulty.Medium;
    }
    public bool Multiplayer() {
        return playerQuantity == PlayersQuantity.Two;
    }
    public void SetUltBerserker()
    {
        playerUlt = Player.Ults.Berserker;
        
    }

    public void SetUltScatter()
    {
        playerUlt = Player.Ults.Scatter;
    }


    public int GetCreditsAmount() {
        if (dificulty == Dificulty.Easy) {
            return creditsEasy;
        }
        else if (dificulty == Dificulty.Medium)
        {
            return creditsMedium;
        }
        else if (dificulty == Dificulty.Hard)
        {
            return creditsHard ;
        } 

        throw new Exception("la estas cagando");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetUltBerserker();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetUltScatter();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetSpawnMinions();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetEasy();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetMedium();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetHard();
        }
    }

    internal void SetNode(int value)
    {
        node = value;
    }
}
