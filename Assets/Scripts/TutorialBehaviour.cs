using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialBehaviour : MonoBehaviour {

    public static TutorialBehaviour instance { get; private set; }
    bool _greenKilled;

    public bool GreenKilled { get { return _greenKilled; } set { _greenKilled = value; } }

    void Awake() {
        Debug.Assert(FindObjectsOfType<TutorialBehaviour>().Length == 1);
        if (instance == null) { 
            instance = this;
        }
	} 

    public void RestartTutorial() {
        _greenKilled = false;
        LootTableManager.instance.SetTutoProbavility();
    }

    public void FirstEnemyKIlled() {
        LootTableManager.instance.SetDefaultProbavility();
    }

    public void EndTutorial() {
     //   SceneManager.LoadScene("Menu");
    }
}
