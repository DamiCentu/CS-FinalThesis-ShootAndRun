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
    public bool reset=false;
  


    void Start()
    {
        activeAchivements.Add("hols");
        achivementsUI = FindObjectOfType<AchivementsUI>();
        EventManager.instance.SubscribeEvent("ObtainAchivements", ObtainAchivements);
        //LoadAchivements();
        LoadAchivements();
        print(activeAchivements[0]);
    }

    private void Update()
    {
        if (reset) {
            activeAchivements = new List<string>();
            SaveAchivements();
        }
    }

    private void ObtainAchivements(object[] parameterContainer)
    {
        string achivement = (string)parameterContainer[0];
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
