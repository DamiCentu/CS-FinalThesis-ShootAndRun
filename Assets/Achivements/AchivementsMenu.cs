using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchivementsMenu : MonoBehaviour {

    AchivementManager achivements;
    List<string> actives;
    public Text title;
    public Text description;

    public Text nO_DEATH;
    public Text mINI_BOSS_DEFEAT;
    public Text dASH;
    public Text lVL_2;
    public Text lVL_1;
    public Text rECOVER;
    public Text almost;
    public Text enemies100;



    void Start () {
        achivements = GetComponent<AchivementManager>();
        actives = achivements.ActiveAchivements();

    }

    public void NotToday() {
        
        if (actives.Contains("Not Today"))
            description.text = "Don`t lose any life in a platform"; 
        else description.text = "??????????????";
    }
    public void DashDash()
    {
        if (actives.Contains("Dash Dash"))
            description.text = "Dash 50 times in a level";
        else description.text = "??????????????";


    }
    public void Almost()
    {

        if (actives.Contains("Almost"))
            description.text = "Lose a shield but don't die in a platform";
        else description.text = "??????????????";

 
    }
    public void A100()
    {
        if (actives.Contains("100"))
            description.text = "Kill 100 enemies without dying";
        else description.text = "??????????????";



    }
    public void Frenesi()
    {
        if (actives.Contains("Frenesi"))
            description.text = "Kill 20 enemies in 5 seconds";
        else description.text = "??????????????";



    }
    public void NoElite()
    {
        if (actives.Contains("No Elite"))
            description.text = "Win level 1 without having an Elite";
        else description.text = "??????????????";



    }
    public void Easy()
    {
        if (actives.Contains("Easy"))
            description.text = "Win the game in Easy Mode";
        else description.text = "??????????????";


    }


    public void Medium()
    {
        if (actives.Contains("Medium"))
            description.text = "Win the game in Medium Mode";
        else description.text = "??????????????";


    }
    public void Hard()
    {
        if (actives.Contains("Hard"))
            description.text = "Win the game in Hard Mode";
        else description.text = "??????????????";


    }

    public void Nothing() {

        description.text = "";
    }
}
