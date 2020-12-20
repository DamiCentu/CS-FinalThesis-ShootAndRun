using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Compilation;
using UnityEngine;


[System.Serializable]
public class AchivementManager : MonoBehaviour {

//    List<string> allAchivements= new List<string> { "UI_ACHIVEMENT_NO_DEATH", "ACHIVEMENT_MINI_BOSS_DEFEAT", "ACHIVEMENT_DASH_DASH_DASH", "ACHIVEMENT_LVL2_COMPLETE", "ACHIVEMENT_LVL1_COMPLETE", "ACHIVEMENT_FIRST_STAGE_COMPLETE","ACHIVEMENT_UPGRADE_WEAPON", "ACHIVEMENT_POWER_UP_RECOVER", "ACHIVEMENT_MORE_RANGE", "ACHIVEMENT_SHIELD","ACHIVEMENT_CLOSE_DEATH","ACHIVEMENT_EXTRA_DASH", "ACHIVEMENT_100_ENEMIES_DEAD" };
    List<string> allAchivements = new List<string> { "UI_ACHIVEMENT_NO_DEATH", "ACHIVEMENT_MINI_BOSS_DEFEAT", "ACHIVEMENT_DASH_DASH_DASH", "ACHIVEMENT_LVL2_COMPLETE", "ACHIVEMENT_LVL1_COMPLETE", "ACHIVEMENT_FIRST_STAGE_COMPLETE", "ACHIVEMENT_UPGRADE_WEAPON", "ACHIVEMENT_POWER_UP_RECOVER", "ACHIVEMENT_100_ENEMIES_DEAD" };

    List<string> activeAchivements = new List<string>();
    AchivementsUI achivementsUI;
    public bool reset=false;
  


    void Start()
    {
        achivementsUI = FindObjectOfType<AchivementsUI>();
        EventManager.instance.SubscribeEvent("UI_ACHIVEMENT_NO_DEATH", x => ObtainAchivements("Not Today"));
        EventManager.instance.SubscribeEvent("ACHIVEMENT_ACHIVEMENT_MINI_BOSS_DEFEAT", x => ObtainAchivements("Elite Dead"));
        //EventManager.instance.SubscribeEvent("ACHIVEMENT_EXTRA_DASH", x=>ObtainAchivements("Faster"));
        EventManager.instance.SubscribeEvent("ACHIVEMENT_DASH_DASH_DASH", x=>ObtainAchivements("DASH, DASH, DASH"));
        EventManager.instance.SubscribeEvent("ACHIVEMENT_LVL2_COMPLETE", x=>ObtainAchivements("LVL 2 complete"));
        EventManager.instance.SubscribeEvent("ACHIVEMENT_LVL1_COMPLETE", x=>ObtainAchivements("LVL 1 complete"));
        //EventManager.instance.SubscribeEvent("ACHIVEMENT_UPGRADE_WEAPON", x=>ObtainAchivements("MORE BULLETS"));
        EventManager.instance.SubscribeEvent("ACHIVEMENT_POWER_UP_RECOVER", x=>ObtainAchivements("Power up recover"));
        //EventManager.instance.SubscribeEvent("ACHIVEMENT_MORE_RANGE", x=>ObtainAchivements("Far away"));
        //EventManager.instance.SubscribeEvent("ACHIVEMENT_SHIELD", x=>ObtainAchivements("She protect"));
        EventManager.instance.SubscribeEvent("ACHIVEMENT_CLOSE_DEATH", x=>ObtainAchivements("Almost Dead"));
        EventManager.instance.SubscribeEvent("ACHIVEMENT_100_ENEMIES_DEAD", x=>ObtainAchivements("100 enemies dead"));


        //LoadAchivements();
        LoadAchivements();
        print(activeAchivements[0]);
    }

    private void ACHIVEMENT_EXTRA_DASH(object[] parameterContainer)
    {
        throw new NotImplementedException();
    }

    private void ObtainAchivementsMINI_BOSS_DEFEAT(object[] parameterContainer)
    {
        throw new NotImplementedException();
    }

    private void ObtainAchivementsUnTouchable(object[] parameterContainer)
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        if (reset) {
            activeAchivements = new List<string>();
            SaveAchivements();
        }
    }

    private void ObtainAchivements(string achivement)
    {
        if (!activeAchivements.Contains(achivement)) {
            if (activeAchivements.Count == 0) activeAchivements = new List<string>();
            activeAchivements.Add(achivement);
            achivementsUI.ShowEvent(achivement);
            SaveAchivements();
        }
    }

    public List<string> ActiveAchivements() {
        return activeAchivements;
    }

    public void LoadAchivements() {
        if (File.Exists(getAchivementJson())) {
            string JsonString = File.ReadAllText(getAchivementJson());
            activeAchivements = JsonConvert.DeserializeObject<List<string>>(JsonString);
        }

        print(activeAchivements);
    }

    public void SaveAchivements()
    {
        print(getAchivementJson());
        string jsonString = JsonConvert.SerializeObject(activeAchivements, Formatting.Indented);
        File.WriteAllText(getAchivementJson(), jsonString);

        ;
    }

    private static string getAchivementJson()
    {
        return Application.dataPath + "/achivements.json";
    }
}
