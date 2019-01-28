using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SavingAndLoading {

    const string PLAYER_INFO_PATH = "/playerInfo";
    const string EXTENTION_PATH = ".dat";

    public static void SaveRank(Data[] scoresAndNames, String levelName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + PLAYER_INFO_PATH + "_" + levelName + EXTENTION_PATH);
        var saveData = new SaveData();
        saveData.scoresAndNames = scoresAndNames;

        bf.Serialize(file, saveData);
        file.Close();
    }

    public static Data[] LoadRanks(String levelName)
    {
        var saveData = new SaveData();

        if (File.Exists(Application.persistentDataPath + PLAYER_INFO_PATH))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + PLAYER_INFO_PATH + "_" + levelName + EXTENTION_PATH , FileMode.Open);
            saveData = (SaveData)bf.Deserialize(file);

            file.Close();
        }
        return saveData.scoresAndNames;
    }
}

[Serializable]
class SaveData
{
    public Data[] scoresAndNames;
}

public class Data
{
    public int score;
    public string name;
}
