using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour, IPauseable {

    public List<GameObject> dash;
    public SimpleHealthBar bossLife;
    public List<Image> lifeImages;
    public Text creditsText;
    public Text tutorialText;
    public float timeToDisapearTutoText = 4f;

    public static UIManager instance = null;

    public const string TUTORIAL_MOVE = "Use W,A,S,D to move";
    public const string TUTORIAL_DASH = "Use spacebar to dash";
    public const string TUTORIAL_ULTIMATE = "Use alt to use your ULTIMATE!";
    public const string TUTORIAL_SHOOT = "Use left click to shoot";
    public const string TUTORIAL_SHOOT_SPECIAL = "Use right click to shoot your special weapon";
    public const string TUTORIAL_PICK_POWER_UP = "Pick power ups to upgrade your champion";

    WaitForSeconds _waitToDisapearTutoText;
    HashSet<string> _hashToCompare = new HashSet<string>();

    bool _paused;
    public void OnPauseChange(bool v)
    {
        _paused = v;
    }

    void Start() {
        EventManager.instance.SubscribeEvent(Constants.UPDATE_BOSS_LIFE, UpdateBossLife);
        EventManager.instance.SubscribeEvent(Constants.UI_UPDATE_PLAYER_LIFE, OnUpdatePlayerLife);
        EventManager.instance.SubscribeEvent(Constants.UI_TUTORIAL_RESTART, OnTutorialRestart);
        EventManager.instance.SubscribeEvent(Constants.UI_TUTORIAL_CHANGE, OnTutorialChange);
        EventManager.instance.SubscribeEvent(Constants.UI_TUTORIAL_DEACTIVATED, OnTutorialDeactivated);
        if (instance == null) { 
            instance = this;
        }
        tutorialText.text = "";
        _waitToDisapearTutoText = new WaitForSeconds(timeToDisapearTutoText);
    }

    private void OnTutorialRestart(object[] parameterContainer) {
        _hashToCompare.Clear();
    }

    private void OnTutorialDeactivated(object[] parameterContainer) {
        tutorialText.enabled = false;
    }

    private void OnTutorialChange(object[] parameterContainer) {
        if(_hashToCompare.Contains((string) parameterContainer[0])) {
            return;
        }
        _hashToCompare.Add((string)parameterContainer[0]);
        tutorialText.enabled = true;
        StopAllCoroutines();
        tutorialText.text = (string)parameterContainer[0];
        StartCoroutine(TextDisapearRoutine());
    }

    IEnumerator TextDisapearRoutine() {
        yield return _waitToDisapearTutoText;
        while (_paused)
            yield return null;
        tutorialText.enabled = false;
    } 

    private void UpdateBossLife(object[] parameterContainer) {
        if (bossLife == null)
            return;

        int life = (int)parameterContainer[0];
        int maxLife = (int)parameterContainer[1];

        if (life > 0) {
            bossLife.transform.parent.gameObject.SetActive(true);
            bossLife.UpdateBar(life, maxLife);
            if (life < (float)maxLife / 4) {
                bossLife.UpdateColor(Color.red);
            }
            else if (life < (float)maxLife / 2) {
                bossLife.UpdateColor(Color.yellow);

            }
            else {
                bossLife.UpdateColor(Color.green);
            }
        }
        else {
            bossLife.transform.parent.gameObject.SetActive(false);
        } 
    }

    public void OnUpdatePlayerLife(object[] parameterContainer) { 
        int lifeCount = (int)parameterContainer[0];
        
        if (lifeCount <= 0) {
            foreach (var i in lifeImages) { 
                i.gameObject.SetActive(false);
            } 
            return;
        }

        for (int i = 0; i < lifeCount; i++) 
            lifeImages[i].gameObject.SetActive(true); 

        for (int i = lifeCount; i < lifeImages.Count; i++)
            lifeImages[i].gameObject.SetActive(false);

        creditsText.text = "Credits: " + (int)parameterContainer[1]; //creditsRemaining 
    }
}
