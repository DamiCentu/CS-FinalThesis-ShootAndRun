using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchivementsMenu : MonoBehaviour {

    AchivementManager achivements;
    List<string> actives;
    public Text title;
    public Text description;

    public GameObject nO_DEATH;
    public GameObject frenesi;
    public GameObject noElite;
    public GameObject a100;
    public GameObject dashDash;
    public GameObject medium ;
    public GameObject easy;
    public GameObject hard;
    public GameObject rECOVER;
    public GameObject almost;
    public Material active;


    void Start () {
        achivements = GetComponent<AchivementManager>();
        actives = achivements.ActiveAchivements();
        NotTodayActive();
        DashDashActive();
        FrenesiActive();
        NoElitective();
        A100Active();
        MediumActive();
        EasyActive();
        HardActive();
        AlmostActive();
    }

    public void NotTodayActive() {
        if (actives.Contains("Not Today"))
        {
            nO_DEATH.GetComponentInChildren<Text>().material = active;
            ChangeBorderColor(nO_DEATH);
        }

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


    public void DashDashActive()
    {
        if (actives.Contains("Dash Dash"))
        {
            dashDash.GetComponentInChildren<Text>().material = active;
            ChangeBorderColor(dashDash);
        }
    }


    public void Almost()
    {

        if (actives.Contains("Almost"))
            description.text = "Lose a shield but don't die in a platform";
        else description.text = "??????????????";
 
    }



    public void AlmostActive()
    {
        if (actives.Contains("Almost"))
        {
            almost.GetComponentInChildren<Text>().material = active;
            ChangeBorderColor(almost);
        }
    }

    public void A100()
    {
        if (actives.Contains("100"))
            description.text = "Kill 100 enemies without dying";
        else description.text = "??????????????";
    }

    public void A100Active()
    {
        if (actives.Contains("100"))
        {
            a100.GetComponentInChildren<Text>().material = active;
            ChangeBorderColor(a100);
        }
    }



    public void Frenesi()
    {
        if (actives.Contains("Frenesi"))
            description.text = "Kill 15 enemies in 3 seconds";
        else description.text = "??????????????";
    }

    public void FrenesiActive()
    {
        if (actives.Contains("Frenesi"))
        {
            frenesi.GetComponentInChildren<Text>().material = active;
            ChangeBorderColor(frenesi);
        }
    }



    public void NoElite()
    {
        if (actives.Contains("No Elite"))
            description.text = "Kill an Elite enemie";
        else description.text = "??????????????";

    }
    public void NoElitective()
    {
        if (actives.Contains("No Elite"))
        {
            noElite.GetComponentInChildren<Text>().material = active;
            ChangeBorderColor(noElite);
        }
    }



    public void Easy()
    {
        if (actives.Contains("Easy"))
            description.text = "Win the game in Easy Mode";
        else description.text = "??????????????";


    }

    public void EasyActive()
    {
        if (actives.Contains("Easy"))
        {
            easy.GetComponentInChildren<Text>().material = active;
            ChangeBorderColor(easy);
        }
    }


    public void Medium()
    {
        if (actives.Contains("Medium"))
            description.text = "Win the game in Medium Mode";
        else description.text = "??????????????";
    }

    public void MediumActive()
    {
        if (actives.Contains("Medium"))
        {
            medium.GetComponentInChildren<Text>().material = active;
            ChangeBorderColor(medium);
        }
    }



    public void Hard()
    {
        if (actives.Contains("Hard"))
            description.text = "Win the game in Hard Mode";
        else description.text = "??????????????";
    }

    public void HardActive()
    {
        if (actives.Contains("Hard"))
        {
            hard.GetComponentInChildren<Text>().material = active;
            ChangeBorderColor(hard);
        }



     }

    private void ChangeBorderColor(GameObject go)
    {
        var borders=go.GetComponent<MenuNeonButtonBehaviour>().ParentOfMeshRendererersToChangeMaterial[0];
        foreach (var rend in borders.GetComponentsInChildren<Renderer>())
        {
            rend.material = active;
        }
    }

    public void Nothing() {

        description.text = "";
    }
}
