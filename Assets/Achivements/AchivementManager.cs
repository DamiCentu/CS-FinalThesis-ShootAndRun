using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Compilation;
using UnityEngine;


[System.Serializable]
public class AchivementManager : MonoBehaviour {

    List<string> allAchivements= new List<string> { "No Deads", "LVL 1 Complete", "LVL 2 Complete", "Better, Faster, Stronger" };
    List<string> activeAchivements = new List<string> { "No Deads", "LVL 1 Complete", "LVL 2 Complete" };
    AchivementsUI achivementsUI;


    void Start()
    {
        activeAchivements.Add("hols");
        achivementsUI = FindObjectOfType<AchivementsUI>();
        EventManager.instance.SubscribeEvent("ObtainAchivements", ObtainAchivements);
        //LoadAchivements();
        SaveAchivements();
        print(activeAchivements[0]);
    }

    private void ObtainAchivements(object[] parameterContainer)
    {
        string achivement = (string)parameterContainer[0];
        if (!activeAchivements.Contains(achivement)) {
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

        LoadAchivements();
    }

    private static string getAchivementJson()
    {
        return Application.dataPath + "/achivements.json";
    }
}
