using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialBehaviour : MonoBehaviour {

    public static TutorialBehaviour instance { get; private set; }
    bool _greenKilled;
    public SectionNode tutorialNode;
    public bool isFirstEnemyKilled = false;

    public bool GreenKilled { get { return _greenKilled; } set { _greenKilled = value; } }

    void Awake() {
        Debug.Assert(FindObjectsOfType<TutorialBehaviour>().Length == 1);
        if (instance == null) { 
            instance = this;
        }
	} 

    public void RestartTutorial() {
        EventManager.instance.ExecuteEvent(Constants.UI_TUTORIAL_RESTART);
        EventManager.instance.ExecuteEvent(Constants.UI_TUTORIAL_CHANGE, UIManager.TUTORIAL_MOVE);
        _greenKilled = false;
        isFirstEnemyKilled = false;
        StartCoroutine(defaultProbRoutine()); 
    }

    IEnumerator defaultProbRoutine() {
        yield return new WaitForSeconds(1f);
        LootTableManager.instance.SetTutoProbability();
    }

    public void FirstEnemyKIlled() {
        EventManager.instance.ExecuteEvent(Constants.UI_TUTORIAL_CHANGE, UIManager.TUTORIAL_PICK_POWER_UP);
        LootTableManager.instance.SetDefaultProbavility();
        isFirstEnemyKilled = true;
    }

    public void EndTutorial() {
     //   SceneManager.LoadScene("Menu");
    }

    public bool IsTutorialNode { get { return tutorialNode == SectionManager.instance.actualNode; } }
}
